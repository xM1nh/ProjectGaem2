using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.DataStructures;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    internal static class Settings
    {
        internal const int MaxGJKIterations = 20;
        internal const float Epsilon = 1.192092896e-7f;
    }

    internal struct GJKProxy
    {
        internal Vector2[] Vertices;

        internal int Count;

        internal float Radius;

        internal void Set(Shape shape)
        {
            switch (shape)
            {
                case Circle circle:
                    Vertices = [circle.Center];
                    Count = 1;
                    Radius = circle.Radius;
                    break;
                case Box2D box:
                    Vertices =
                    [
                        box.Min,
                        new Vector2(box.Max.X, box.Min.Y),
                        box.Max,
                        new Vector2(box.Min.X, box.Max.Y)
                    ];
                    Count = 4;
                    Radius = 0.01f;
                    break;
                case Capsule2D capsule:
                    Vertices = [capsule.Start, capsule.End];
                    Count = 2;
                    Radius = capsule.Radius;
                    break;
                case Polygon poly:
                    Radius = 0.01f;
                    Count = poly.Vertices.Count;
                    Vertices = new Vector2[Count];
                    for (var i = 0; i < Count; i++)
                    {
                        Vertices[i] = poly.Vertices[i];
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal int GetSupport(Vector2 direction)
        {
            int bestIndex = 0;
            float bestValue = Vector2.Dot(Vertices[0], direction);
            for (int i = 1; i < Count; ++i)
            {
                float value = Vector2.Dot(Vertices[i], direction);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return bestIndex;
        }

        internal Vector2 GetSupportVertex(Vector2 direction)
        {
            int bestIndex = 0;
            float bestValue = Vector2.Dot(Vertices[0], direction);
            for (int i = 1; i < Count; ++i)
            {
                float value = Vector2.Dot(Vertices[i], direction);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return Vertices[bestIndex];
        }

        internal ref readonly Vector2 GetVertex(int index)
        {
            Debug.Assert(0 <= index && index < Count);
            return ref Vertices[index];
        }
    }

    internal struct SimplexCache
    {
        /// <summary>
        /// Length or area
        /// </summary>
        internal ushort Count;

        /// <summary>
        /// Vertices on shape A
        /// </summary>
        internal FixedArray3<byte> IndexA;

        /// <summary>
        /// Vertices on shape B
        /// </summary>
        internal FixedArray3<byte> IndexB;

        internal float Metric;
    }

    internal struct GJKOutput
    {
        internal float Distance;

        /// <summary>
        /// Number of GJK iterations used
        /// </summary>
        internal int Iterations;

        /// <summary>
        /// Closest point on shapeA
        /// </summary>
        internal Vector2 PointA;

        /// <summary>
        /// Closest point on shapeB
        /// </summary>
        internal Vector2 PointB;
    }

    struct SimplexVertex
    {
        /// <summary>
        /// Barycentric coordinate for closest point
        /// </summary>
        internal float A;

        /// <summary>
        /// wA index
        /// </summary>
        internal int IndexA;

        /// <summary>
        /// wB index
        /// </summary>
        internal int IndexB;

        /// <summary>
        /// wB - wA
        /// </summary>
        internal Vector2 W;

        /// <summary>
        /// Support point in proxyA
        /// </summary>
        internal Vector2 Wa;

        /// <summary>
        /// Support point in proxyB
        /// </summary>
        internal Vector2 Wb;
    }

    struct Simplex
    {
        internal int Count;
        internal FixedArray3<SimplexVertex> V;

        internal void ReadCache(
            in SimplexCache cache,
            GJKProxy proxyA,
            PhysicsInternalTransform transformA,
            GJKProxy proxyB,
            PhysicsInternalTransform transformB
        )
        {
            Debug.Assert(cache.Count <= 3);

            // Copy data from cache.
            Count = cache.Count;
            for (int i = 0; i < Count; ++i)
            {
                ref var v = ref V[i];
                v.IndexA = cache.IndexA[i];
                v.IndexB = cache.IndexB[i];
                var wALocal = proxyA.Vertices[v.IndexA];
                var wBLocal = proxyB.Vertices[v.IndexB];
                v.Wa = GJKHelper.Mul(transformA, wALocal);
                v.Wb = GJKHelper.Mul(transformB, wBLocal);
                v.W = v.Wb - v.Wa;
                v.A = 0.0f;
                V[i] = v;
            }

            // Compute the new simplex metric, if it is substantially different than
            // old metric then flush the simplex.
            if (Count > 1)
            {
                float metric1 = cache.Metric;
                float metric2 = GetMetric();
                if (metric2 < 0.5f * metric1 || 2.0f * metric1 < metric2 || metric2 < float.Epsilon)
                {
                    // Reset the simplex.
                    Count = 0;
                }
            }

            // If the cache is empty or invalid ...
            if (Count == 0)
            {
                ref var v = ref V[0];
                v.IndexA = 0;
                v.IndexB = 0;
                var wALocal = proxyA.Vertices[0];
                var wBLocal = proxyB.Vertices[0];
                v.Wa = GJKHelper.Mul(transformA, wALocal);
                v.Wb = GJKHelper.Mul(transformB, wBLocal);
                v.W = v.Wb - v.Wa;
                v.A = 1.0f;
                V[0] = v;
                Count = 1;
            }
        }

        internal void WriteCache(ref SimplexCache cache)
        {
            cache.Metric = GetMetric();
            cache.Count = (ushort)Count;
            for (int i = 0; i < Count; ++i)
            {
                cache.IndexA[i] = (byte)(V[i].IndexA);
                cache.IndexB[i] = (byte)(V[i].IndexB);
            }
        }

        internal Vector2 GetSearchDirection()
        {
            switch (Count)
            {
                case 1:
                    return -V[0].W;

                case 2:
                {
                    Vector2 e12 = V[1].W - V[0].W;
                    float sgn = GJKHelper.Cross(e12, -V[0].W);
                    if (sgn > 0.0f)
                    {
                        // Origin is left of e12.
                        return GJKHelper.Cross(1.0f, e12);
                    }
                    else
                    {
                        // Origin is right of e12.
                        return GJKHelper.Cross(e12, 1.0f);
                    }
                }

                default:
                    Debug.Assert(false);
                    return Vector2.Zero;
            }
        }

        internal Vector2 GetClosestPoint()
        {
            switch (Count)
            {
                case 0:
                    Debug.Assert(false);
                    return Vector2.Zero;

                case 1:
                    return V[0].W;

                case 2:
                    return V[0].A * V[0].W + V[1].A * V[1].W;

                case 3:
                    return Vector2.Zero;

                default:
                    Debug.Assert(false);
                    return Vector2.Zero;
            }
        }

        internal void GetWitnessPoints(out Vector2 pA, out Vector2 pB)
        {
            switch (Count)
            {
                case 1:
                    pA = V[0].Wa;
                    pB = V[0].Wb;
                    break;

                case 2:
                    pA = V[0].A * V[0].Wa + V[1].A * V[1].Wa;
                    pB = V[0].A * V[0].Wb + V[1].A * V[1].Wb;
                    break;

                case 3:
                    pA = V[0].A * V[0].Wa + V[1].A * V[1].Wa + V[2].A * V[2].Wa;
                    pB = pA;
                    break;

                default:
                    throw new Exception();
            }
        }

        internal float GetMetric()
        {
            switch (Count)
            {
                case 0:
                    Debug.Assert(false);
                    return 0.0f;
                case 1:
                    return 0.0f;

                case 2:
                    return (V[0].W - V[1].W).Length();

                case 3:
                    return GJKHelper.Cross(V[1].W - V[0].W, V[2].W - V[0].W);

                default:
                    Debug.Assert(false);
                    return 0.0f;
            }
        }

        // Solve a line segment using barycentric coordinates.
        //
        // p = a1 * w1 + a2 * w2
        // a1 + a2 = 1
        //
        // The vector from the origin to the closest point on the line is
        // perpendicular to the line.
        // e12 = w2 - w1
        // dot(p, e) = 0
        // a1 * dot(w1, e) + a2 * dot(w2, e) = 0
        //
        // 2-by-2 linear system
        // [1      1     ][a1] = [1]
        // [w1.e12 w2.e12][a2] = [0]
        //
        // Define
        // d12_1 =  dot(w2, e12)
        // d12_2 = -dot(w1, e12)
        // d12 = d12_1 + d12_2
        //
        // Solution
        // a1 = d12_1 / d12
        // a2 = d12_2 / d12

        internal void Solve2()
        {
            ref var v0 = ref V.Value0;
            ref var v1 = ref V.Value1;
            var w1 = v0.W;
            var w2 = v1.W;
            var e12 = w2 - w1;

            // w1 region
            var d12_2 = -Vector2.Dot(w1, e12);
            if (d12_2 <= 0.0f)
            {
                // a2 <= 0, so we clamp it to 0
                v0.A = 1.0f;
                Count = 1;
                return;
            }

            // w2 region
            var d12_1 = Vector2.Dot(w2, e12);
            if (d12_1 <= 0.0f)
            {
                // a1 <= 0, so we clamp it to 0
                v1.A = 1.0f;
                Count = 1;
                V.Value0 = V.Value1;
                return;
            }

            // Must be in e12 region.
            var inv_d12 = 1.0f / (d12_1 + d12_2);
            v0.A = d12_1 * inv_d12;
            v1.A = d12_2 * inv_d12;
            Count = 2;
        }

        // Possible regions:
        // - points[2]
        // - edge points[0]-points[2]
        // - edge points[1]-points[2]
        // - inside the triangle
        internal void Solve3()
        {
            ref var v0 = ref V.Value0;
            ref var v1 = ref V.Value1;
            ref var v2 = ref V.Value2;
            var w1 = v0.W;
            var w2 = v1.W;
            var w3 = v2.W;

            // Edge12
            // [1      1     ][a1] = [1]
            // [w1.e12 w2.e12][a2] = [0]
            // a3 = 0
            var e12 = w2 - w1;
            var w1e12 = Vector2.Dot(w1, e12);
            var w2e12 = Vector2.Dot(w2, e12);
            var d12_1 = w2e12;
            var d12_2 = -w1e12;

            // Edge13
            // [1      1     ][a1] = [1]
            // [w1.e13 w3.e13][a3] = [0]
            // a2 = 0
            var e13 = w3 - w1;
            var w1e13 = Vector2.Dot(w1, e13);
            var w3e13 = Vector2.Dot(w3, e13);
            var d13_1 = w3e13;
            var d13_2 = -w1e13;

            // Edge23
            // [1      1     ][a2] = [1]
            // [w2.e23 w3.e23][a3] = [0]
            // a1 = 0
            var e23 = w3 - w2;
            var w2e23 = Vector2.Dot(w2, e23);
            var w3e23 = Vector2.Dot(w3, e23);
            var d23_1 = w3e23;
            var d23_2 = -w2e23;

            // Triangle123
            var n123 = GJKHelper.Cross(e12, e13);

            var d123_1 = n123 * GJKHelper.Cross(w2, w3);
            var d123_2 = n123 * GJKHelper.Cross(w3, w1);
            var d123_3 = n123 * GJKHelper.Cross(w1, w2);

            // w1 region
            if (d12_2 <= 0.0f && d13_2 <= 0.0f)
            {
                v0.A = 1.0f;
                Count = 1;
                return;
            }

            // e12
            if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
            {
                var inv_d12 = 1.0f / (d12_1 + d12_2);
                v0.A = d12_1 * inv_d12;
                v1.A = d12_2 * inv_d12;
                Count = 2;
                return;
            }

            // e13
            if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
            {
                var inv_d13 = 1.0f / (d13_1 + d13_2);
                v0.A = d13_1 * inv_d13;
                v2.A = d13_2 * inv_d13;
                Count = 2;
                v1 = v2;
                return;
            }

            // w2 region
            if (d12_1 <= 0.0f && d23_2 <= 0.0f)
            {
                v1.A = 1.0f;
                Count = 1;
                v0 = v1;
                return;
            }

            // w3 region
            if (d13_1 <= 0.0f && d23_1 <= 0.0f)
            {
                v2.A = 1.0f;
                Count = 1;
                v0 = v2;
                return;
            }

            // e23
            if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
            {
                var inv_d23 = 1.0f / (d23_1 + d23_2);
                v1.A = d23_1 * inv_d23;
                v2.A = d23_2 * inv_d23;
                Count = 2;
                v0 = v2;
                return;
            }

            // Must be in triangle123
            var inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
            v0.A = d123_1 * inv_d123;
            v1.A = d123_2 * inv_d123;
            v2.A = d123_3 * inv_d123;
            Count = 3;
        }
    }

    internal static class GJK
    {
        internal static void Compute(
            Shape shapeA,
            PhysicsInternalTransform transformA,
            Shape shapeB,
            PhysicsInternalTransform transformB,
            bool useRadii,
            out GJKOutput output,
            out SimplexCache cache
        )
        {
            cache = new SimplexCache();
            var proxyA = new GJKProxy();
            var proxyB = new GJKProxy();
            proxyA.Set(shapeA);
            proxyB.Set(shapeB);

            // Initialize the simplex.
            var simplex = new Simplex();
            simplex.ReadCache(in cache, proxyA, transformA, proxyB, transformB);

            // Get simplex vertices as an array.
            ref var vertices = ref simplex.V;
            const int maxIters = 20;

            // These store the vertices of the last simplex so that we
            // can check for duplicates and prevent cycling.
            Span<int> saveA = stackalloc int[3];
            Span<int> saveB = stackalloc int[3];

            // Main iteration loop.
            var iter = 0;
            while (iter < maxIters)
            {
                // Copy simplex so we can identify duplicates.
                var saveCount = simplex.Count;
                for (var i = 0; i < simplex.Count; ++i)
                {
                    saveA[i] = vertices[i].IndexA;
                    saveB[i] = vertices[i].IndexB;
                }

                switch (simplex.Count)
                {
                    case 1:
                        break;

                    case 2:
                        simplex.Solve2();
                        break;

                    case 3:
                        simplex.Solve3();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(simplex.Count));
                }

                // If we have 3 points, then the origin is in the corresponding triangle.
                if (simplex.Count == 3)
                {
                    break;
                }

                // Get search direction.
                var d = simplex.GetSearchDirection();

                // Ensure the search direction is numerically fit.
                if (d.LengthSquared() < Settings.Epsilon * Settings.Epsilon)
                {
                    // The origin is probably contained by a line segment
                    // or triangle. Thus the shapes are overlapped.

                    // We can't return zero here even though there may be overlap.
                    // In case the simplex is a point, segment, or triangle it is difficult
                    // to determine if the origin is contained in the CSO or very close to it.
                    break;
                }

                // Compute a tentative new simplex vertex using support points.
                ref var vertex = ref vertices[simplex.Count];
                vertex.IndexA = proxyA.GetSupport(GJKHelper.MulT(transformA.Rotation, -d));
                vertex.Wa = GJKHelper.Mul(transformA, proxyA.GetVertex(vertex.IndexA));

                vertex.IndexB = proxyB.GetSupport(GJKHelper.MulT(transformB.Rotation, d));
                vertex.Wb = GJKHelper.Mul(transformB, proxyB.GetVertex(vertex.IndexB));
                vertex.W = vertex.Wb - vertex.Wa;

                // Check for duplicate support points. This is the main termination criteria.
                var duplicate = false;
                for (var i = 0; i < saveCount; ++i)
                {
                    if (vertex.IndexA == saveA[i] && vertex.IndexB == saveB[i])
                    {
                        duplicate = true;
                        break;
                    }
                }

                // If we found a duplicate support point we must exit to avoid cycling.
                if (duplicate)
                {
                    break;
                }

                // New vertex is ok and needed.
                ++simplex.Count;
            }

            // Prepare output.
            simplex.GetWitnessPoints(out output.PointA, out output.PointB);
            output.Distance = Vector2.Distance(output.PointA, output.PointB);
            output.Iterations = iter;

            // Cache the simplex.
            simplex.WriteCache(ref cache);

            // Apply radii if requested.
            if (useRadii)
            {
                if (output.Distance < Settings.Epsilon)
                {
                    // Shapes are too close to safely compute normal
                    var p = 0.5f * (output.PointA + output.PointB);
                    output.PointA = p;
                    output.PointB = p;
                    output.Distance = 0.0f;
                }
                else
                {
                    // Keep closest points on perimeter even if overlapped, this way
                    // the points move smoothly.
                    var rA = proxyA.Radius;
                    var rB = proxyB.Radius;
                    var normal = output.PointB - output.PointA;
                    normal.Normalize();
                    output.Distance = MathF.Max(0.0f, output.Distance - rA - rB);
                    output.PointA += rA * normal;
                    output.PointB -= rB * normal;
                }
            }
        }
    }
}

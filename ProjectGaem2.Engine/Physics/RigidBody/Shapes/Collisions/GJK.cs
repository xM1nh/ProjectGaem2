using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions
{
    public static class Settings
    {
        public const int MaxGJKIterations = 20;
    }

    public class GJKProxy
    {
        internal float Radius;
        internal List<Vector2> Vertices = [];

        public void Set(Shape shape)
        {
            if (shape is Circle circle)
            {
                Vertices.Add(circle.Center);
                Radius = circle.Radius;
                return;
            }

            if (shape is Capsule2D capsule)
            {
                Vertices.Add(capsule.Start);
                Vertices.Add(capsule.End);
                Radius = capsule.Radius;
                return;
            }
        }

        public int GetSupport(Vector2 direction)
        {
            int bestIndex = 0;
            float bestValue = Vector2.Dot(Vertices[0], direction);
            for (int i = 1; i < Vertices.Count; ++i)
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

        public Vector2 GetSupportVertex(Vector2 direction)
        {
            int bestIndex = 0;
            float bestValue = Vector2.Dot(Vertices[0], direction);
            for (int i = 1; i < Vertices.Count; ++i)
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
    }

    public struct SimplexCache
    {
        /// <summary>
        /// Length or area
        /// </summary>
        public ushort Count;

        /// <summary>
        /// Vertices on shape A
        /// </summary>
        public byte[] IndexA;

        /// <summary>
        /// Vertices on shape B
        /// </summary>
        public byte[] IndexB;

        public float Metric;

        public SimplexCache()
        {
            IndexA = new byte[3];
            IndexB = new byte[3];
        }
    }

    public struct GJKOutput
    {
        public float Distance;

        /// <summary>
        /// Number of GJK iterations used
        /// </summary>
        public int Iterations;

        /// <summary>
        /// Closest point on shapeA
        /// </summary>
        public Vector2 PointA;

        /// <summary>
        /// Closest point on shapeB
        /// </summary>
        public Vector2 PointB;
    }

    struct SimplexVertex
    {
        /// <summary>
        /// Barycentric coordinate for closest point
        /// </summary>
        public float A;

        /// <summary>
        /// wA index
        /// </summary>
        public int IndexA;

        /// <summary>
        /// wB index
        /// </summary>
        public int IndexB;

        /// <summary>
        /// wB - wA
        /// </summary>
        public Vector2 W;

        /// <summary>
        /// Support point in proxyA
        /// </summary>
        public Vector2 WA;

        /// <summary>
        /// Support point in proxyB
        /// </summary>
        public Vector2 WB;
    }

    struct Simplex
    {
        internal int Count;
        internal SimplexVertex[] V;

        public Simplex()
        {
            V = new SimplexVertex[3];
        }

        internal void ReadCache(
            ref SimplexCache cache,
            GJKProxy proxyA,
            ref Transform transformA,
            GJKProxy proxyB,
            ref Transform transformB
        )
        {
            Debug.Assert(cache.Count <= 3);

            // Copy data from cache.
            Count = cache.Count;
            for (int i = 0; i < Count; ++i)
            {
                var v = V[i];
                v.IndexA = cache.IndexA[i];
                v.IndexB = cache.IndexB[i];
                var wALocal = proxyA.Vertices[v.IndexA];
                var wBLocal = proxyB.Vertices[v.IndexB];
                v.WA = MathHelperExt.Mul(ref transformA, wALocal);
                v.WB = MathHelperExt.Mul(ref transformB, wBLocal);
                v.W = v.WB - v.WA;
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
                var v = V[0];
                v.IndexA = 0;
                v.IndexB = 0;
                var wALocal = proxyA.Vertices[0];
                var wBLocal = proxyB.Vertices[0];
                v.WA = MathHelperExt.Mul(ref transformA, wALocal);
                v.WB = MathHelperExt.Mul(ref transformB, wBLocal);
                v.W = v.WB - v.WA;
                v.A = 1.0f;
                V[0] = v;
                Count = 1;
            }
        }

        internal void WriteCache(ref SimplexCache cache)
        {
            cache.Metric = GetMetric();
            cache.Count = (UInt16)Count;
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
                    float sgn = Vector2Ext.Cross(e12, -V[0].W);
                    if (sgn > 0.0f)
                    {
                        // Origin is left of e12.
                        return new Vector2(-e12.Y, e12.X);
                    }
                    else
                    {
                        // Origin is right of e12.
                        return new Vector2(e12.Y, -e12.X);
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
                case 0:
                    pA = Vector2.Zero;
                    pB = Vector2.Zero;
                    Debug.Assert(false);
                    break;

                case 1:
                    pA = V[0].WA;
                    pB = V[0].WB;
                    break;

                case 2:
                    pA = V[0].A * V[0].WA + V[1].A * V[1].WA;
                    pB = V[0].A * V[0].WB + V[1].A * V[1].WB;
                    break;

                case 3:
                    pA = V[0].A * V[0].WA + V[1].A * V[1].WA + V[2].A * V[2].WA;
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
                    return Vector2Ext.Cross(V[1].W - V[0].W, V[2].W - V[0].W);

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
            Vector2 w1 = V[0].W;
            Vector2 w2 = V[1].W;
            Vector2 e12 = w2 - w1;

            // w1 region
            float d12_2 = -Vector2.Dot(w1, e12);
            if (d12_2 <= 0.0f)
            {
                // a2 <= 0, so we clamp it to 0
                SimplexVertex v0 = V[0];
                v0.A = 1.0f;
                V[0] = v0;
                Count = 1;
                return;
            }

            // w2 region
            float d12_1 = Vector2.Dot(w2, e12);
            if (d12_1 <= 0.0f)
            {
                // a1 <= 0, so we clamp it to 0
                SimplexVertex v1 = V[1];
                v1.A = 1.0f;
                V[1] = v1;
                Count = 1;
                V[0] = V[1];
                return;
            }

            // Must be in e12 region.
            float inv_d12 = 1.0f / (d12_1 + d12_2);
            SimplexVertex v0_2 = V[0];
            SimplexVertex v1_2 = V[1];
            v0_2.A = d12_1 * inv_d12;
            v1_2.A = d12_2 * inv_d12;
            V[0] = v0_2;
            V[1] = v1_2;
            Count = 2;
        }

        // Possible regions:
        // - points[2]
        // - edge points[0]-points[2]
        // - edge points[1]-points[2]
        // - inside the triangle
        internal void Solve3()
        {
            Vector2 w1 = V[0].W;
            Vector2 w2 = V[1].W;
            Vector2 w3 = V[2].W;

            // Edge12
            // [1      1     ][a1] = [1]
            // [w1.e12 w2.e12][a2] = [0]
            // a3 = 0
            Vector2 e12 = w2 - w1;
            float w1e12 = Vector2.Dot(w1, e12);
            float w2e12 = Vector2.Dot(w2, e12);
            float d12_1 = w2e12;
            float d12_2 = -w1e12;

            // Edge13
            // [1      1     ][a1] = [1]
            // [w1.e13 w3.e13][a3] = [0]
            // a2 = 0
            Vector2 e13 = w3 - w1;
            float w1e13 = Vector2.Dot(w1, e13);
            float w3e13 = Vector2.Dot(w3, e13);
            float d13_1 = w3e13;
            float d13_2 = -w1e13;

            // Edge23
            // [1      1     ][a2] = [1]
            // [w2.e23 w3.e23][a3] = [0]
            // a1 = 0
            Vector2 e23 = w3 - w2;
            float w2e23 = Vector2.Dot(w2, e23);
            float w3e23 = Vector2.Dot(w3, e23);
            float d23_1 = w3e23;
            float d23_2 = -w2e23;

            // Triangle123
            float n123 = Vector2Ext.Cross(e12, e13);

            float d123_1 = n123 * Vector2Ext.Cross(w2, w3);
            float d123_2 = n123 * Vector2Ext.Cross(w3, w1);
            float d123_3 = n123 * Vector2Ext.Cross(w1, w2);

            // w1 region
            if (d12_2 <= 0.0f && d13_2 <= 0.0f)
            {
                SimplexVertex v0_1 = V[0];
                v0_1.A = 1.0f;
                V[0] = v0_1;
                Count = 1;
                return;
            }

            // e12
            if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
            {
                float inv_d12 = 1.0f / (d12_1 + d12_2);
                SimplexVertex v0_2 = V[0];
                SimplexVertex v1_2 = V[1];
                v0_2.A = d12_1 * inv_d12;
                v1_2.A = d12_2 * inv_d12;
                V[0] = v0_2;
                V[1] = v1_2;
                Count = 2;
                return;
            }

            // e13
            if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
            {
                float inv_d13 = 1.0f / (d13_1 + d13_2);
                SimplexVertex v0_3 = V[0];
                SimplexVertex v2_3 = V[2];
                v0_3.A = d13_1 * inv_d13;
                v2_3.A = d13_2 * inv_d13;
                V[0] = v0_3;
                V[2] = v2_3;
                Count = 2;
                V[1] = V[2];
                return;
            }

            // w2 region
            if (d12_1 <= 0.0f && d23_2 <= 0.0f)
            {
                SimplexVertex v1_4 = V[1];
                v1_4.A = 1.0f;
                V[1] = v1_4;
                Count = 1;
                V[0] = V[1];
                return;
            }

            // w3 region
            if (d13_1 <= 0.0f && d23_1 <= 0.0f)
            {
                SimplexVertex v2_5 = V[2];
                v2_5.A = 1.0f;
                V[2] = v2_5;
                Count = 1;
                V[0] = V[2];
                return;
            }

            // e23
            if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
            {
                float inv_d23 = 1.0f / (d23_1 + d23_2);
                SimplexVertex v1_6 = V[1];
                SimplexVertex v2_6 = V[2];
                v1_6.A = d23_1 * inv_d23;
                v2_6.A = d23_2 * inv_d23;
                V[1] = v1_6;
                V[2] = v2_6;
                Count = 2;
                V[0] = V[2];
                return;
            }

            // Must be in triangle123
            float inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
            SimplexVertex v0_7 = V[0];
            SimplexVertex v1_7 = V[1];
            SimplexVertex v2_7 = V[2];
            v0_7.A = d123_1 * inv_d123;
            v1_7.A = d123_2 * inv_d123;
            v2_7.A = d123_3 * inv_d123;
            V[0] = v0_7;
            V[1] = v1_7;
            V[2] = v2_7;
            Count = 3;
        }
    }

    public static class GJK
    {
        public static void Compute(
            Shape shapeA,
            Transform transformA,
            Shape shapeB,
            Transform transformB,
            bool useRadii,
            out GJKOutput output,
            out SimplexCache cache
        )
        {
            cache = new SimplexCache();

            // Initialize the simplex.
            var simplex = new Simplex();

            var proxyA = new GJKProxy();
            var proxyB = new GJKProxy();
            proxyA.Set(shapeA);
            proxyB.Set(shapeB);

            simplex.ReadCache(ref cache, proxyA, ref transformA, proxyB, ref transformB);

            // These store the Vertices of the last simplex so that we
            // can check for duplicates and prevent cycling.
            var saveA = new int[3];
            var saveB = new int[3];

            //float distanceSqr1 = Settings.MaxFloat;

            // Main iteration loop.
            int iter = 0;
            while (iter < Settings.MaxGJKIterations)
            {
                // Copy simplex so we can identify duplicates.
                int saveCount = simplex.Count;
                for (int i = 0; i < saveCount; ++i)
                {
                    saveA[i] = simplex.V[i].IndexA;
                    saveB[i] = simplex.V[i].IndexB;
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
                        Debug.Assert(false);
                        break;
                }

                // If we have 3 points, then the origin is in the corresponding triangle.
                if (simplex.Count == 3)
                    break;

                //FPE: This code was not used anyway.
                // Compute closest point.
                //Vector2 p = simplex.GetClosestPoint();
                //float distanceSqr2 = p.LengthSquared();

                // Ensure progress
                //if (distanceSqr2 >= distanceSqr1)
                //{
                //break;
                //}
                //distanceSqr1 = distanceSqr2;

                // Get search direction.
                var d = simplex.GetSearchDirection();

                // Ensure the search direction is numerically fit.
                if (d.LengthSquared() < float.Epsilon * float.Epsilon)
                {
                    // The origin is probably contained by a line segment
                    // or triangle. Thus the shapes are overlapped.

                    // We can't return zero here even though there may be overlap.
                    // In case the simplex is a point, segment, or triangle it is difficult
                    // to determine if the origin is contained in the CSO or very close to it.
                    break;
                }

                // Compute a tentative new simplex vertex using support points.
                var vertex = simplex.V[simplex.Count];
                vertex.IndexA = proxyA.GetSupport(MathHelperExt.MulT(transformA.Rotation, -d));
                vertex.WA = MathHelperExt.Mul(ref transformA, proxyA.Vertices[vertex.IndexA]);

                vertex.IndexB = proxyB.GetSupport(MathHelperExt.MulT(transformB.Rotation, d));
                vertex.WB = MathHelperExt.Mul(ref transformB, proxyB.Vertices[vertex.IndexB]);
                vertex.W = vertex.WB - vertex.WA;
                simplex.V[simplex.Count] = vertex;

                // Iteration count is equated to the number of support point calls.
                ++iter;

                // Check for duplicate support points. This is the main termination criteria.
                bool duplicate = false;
                for (int i = 0; i < saveCount; ++i)
                {
                    if (vertex.IndexA == saveA[i] && vertex.IndexB == saveB[i])
                    {
                        duplicate = true;
                        break;
                    }
                }

                // If we found a duplicate support point we must exit to avoid cycling.
                if (duplicate)
                    break;

                // New vertex is ok and needed.
                ++simplex.Count;
            }

            // Prepare output.
            simplex.GetWitnessPoints(out output.PointA, out output.PointB);
            output.Distance = (output.PointA - output.PointB).Length();
            output.Iterations = iter;

            // Cache the simplex.
            simplex.WriteCache(ref cache);

            // Apply radii if requested.
            if (useRadii)
            {
                var rA = proxyA.Radius;
                var rB = proxyB.Radius;

                if (output.Distance > rA + rB && output.Distance > float.Epsilon)
                {
                    // Shapes are still no overlapped.
                    // Move the witness points to the outer surface.
                    output.Distance -= rA + rB;
                    var normal = output.PointB - output.PointA;
                    Vector2.Normalize(ref normal, out normal);
                    output.PointA += rA * normal;
                    output.PointB -= rB * normal;
                }
                else
                {
                    // Shapes are overlapped when radii are considered.
                    // Move the witness points to the middle.
                    var p = 0.5f * (output.PointA + output.PointB);
                    output.PointA = p;
                    output.PointB = p;
                    output.Distance = 0.0f;
                }
            }
        }
    }
}

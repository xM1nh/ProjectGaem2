using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Extensions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool CircleToCircle(Circle first, Circle second)
        {
            return Vector2.DistanceSquared(first.Center, second.Center)
                < (first.Radius + second.Radius) * (first.Radius + second.Radius);
        }

        public static bool CircleToBox2D(Circle circle, Box2D box)
        {
            var closestPoint = Vector2.Clamp(circle.Center, box.Min, box.Max);
            var dSquared = Vector2.DistanceSquared(circle.Center, closestPoint);
            return dSquared < circle.Radius * circle.Radius;
        }

        public static bool CircleToCapsule2D(Circle circle, Capsule2D capsule)
        {
            var direction = capsule.End - capsule.Start;
            var dFromHead = circle.Center - capsule.Start;
            var dFromHeadAngle = Vector2.Dot(dFromHead, direction);
            float dSquared;

            if (dFromHeadAngle < 0)
            {
                dSquared = dFromHead.LengthSquared();
            }
            else
            {
                var dFromEnd = circle.Center - capsule.End;
                var dFromEndAngle = Vector2.Dot(dFromEnd, direction);
                if (dFromEndAngle < 0)
                {
                    var dFromDirection =
                        dFromHead - direction * dFromHeadAngle / direction.LengthSquared();
                    dSquared = dFromDirection.LengthSquared();
                }
                else
                {
                    dSquared = dFromEnd.LengthSquared();
                }
            }

            var radiiSum = circle.Radius + capsule.Radius;
            return dSquared < radiiSum * radiiSum;
        }

        public static bool CircleToPolygon(Circle circle, Polygon polygon)
        {
            GJK.Compute(
                circle,
                circle.Transform,
                polygon,
                polygon.Transform,
                true,
                out GJKOutput output,
                out SimplexCache cache
            );

            return output.Distance < 10.0f * float.Epsilon;
        }

        public static bool CircleToCircleManifold(
            Circle first,
            Circle second,
            out Manifold manifold
        )
        {
            manifold = new Manifold();

            var dVector = second.Center - first.Center;
            var dSquared = Vector2.DistanceSquared(first.Center, second.Center);
            var radiiSum = first.Radius + second.Radius;

            if (dSquared < radiiSum * radiiSum)
            {
                var d = MathF.Sqrt(dSquared);
                var n = d != 0 ? Vector2.Normalize(dVector) : Vector2.UnitX;

                manifold.Count = 1;
                manifold.Depths[0] = radiiSum - d;
                manifold.ContactPoints[0] = second.Center - n * second.Center;
                manifold.Normal = n;

                return true;
            }

            return false;
        }

        public static bool CircleToBox2DManifold(Circle circle, Box2D box, out Manifold manifold)
        {
            manifold = new Manifold();

            var closestPoint = Vector2.Clamp(circle.Center, box.Min, box.Max);
            var dSquared = Vector2.DistanceSquared(circle.Center, closestPoint);

            if (dSquared < circle.Radius * circle.Radius)
            {
                //center of circle not inside of box
                if (dSquared != 0)
                {
                    var d = MathF.Sqrt(dSquared);
                    var normal = Vector2.Normalize(closestPoint - circle.Center);

                    manifold.Count = 1;
                    manifold.Depths[0] = circle.Radius - d;
                    manifold.ContactPoints[0] = circle.Center + normal * d;
                    manifold.Normal = normal;
                }
                //clamp center to edge then form manifold
                else
                {
                    var boxCenter = (box.Min + box.Max) * 0.5f;
                    var e = (box.Max - box.Min) * 0.5f;
                    var dVector = circle.Center - boxCenter;
                    var absDVector = Vector2Ext.Abs(dVector);

                    var depthX = e.X - absDVector.X;
                    var depthY = e.Y - absDVector.Y;

                    float depth;
                    Vector2 normal;

                    if (depthX < depthY)
                    {
                        depth = depthX;
                        normal = new Vector2(1, 0);
                        if (dVector.X >= 0)
                        {
                            Vector2.Negate(ref normal, out normal);
                        }
                    }
                    else
                    {
                        depth = depthY;
                        normal = new Vector2(0, 1);
                        if (dVector.Y >= 0)
                        {
                            Vector2.Negate(ref normal, out normal);
                        }
                    }

                    manifold.Count = 1;
                    manifold.Depths[0] = circle.Radius + depth;
                    manifold.ContactPoints[0] = circle.Center - normal * depth;
                    manifold.Normal = normal;
                }

                return true;
            }

            return false;
        }

        public static bool CircleToCapsule2DManifold(
            Circle circle,
            Capsule2D capsule,
            out Manifold manifold
        )
        {
            manifold = new Manifold();
            var radiiSum = circle.Radius + capsule.Radius;

            GJK.Compute(
                circle,
                circle.Transform,
                capsule,
                capsule.Transform,
                false,
                out GJKOutput output,
                out SimplexCache cache
            );

            if (output.Distance < radiiSum)
            {
                Vector2 normal;
                if (output.Distance == 0)
                {
                    normal = Vector2.Normalize(Vector2Ext.Skew(capsule.End - capsule.Start));
                }
                else
                {
                    normal = Vector2.Normalize(output.PointB - output.PointA);
                }

                manifold.Count = 1;
                manifold.Depths[0] = radiiSum - output.Distance;
                manifold.ContactPoints[0] = output.PointB - normal * capsule.Radius;
                manifold.Normal = normal;

                return true;
            }

            return false;
        }

        public static bool CircleToPolygonManifold(
            Circle circle,
            Polygon polygon,
            out Manifold manifold
        )
        {
            //throw new NotImplementedException();
            manifold = new Manifold();

            GJK.Compute(
                circle,
                circle.Transform,
                polygon,
                polygon.Transform,
                false,
                out GJKOutput output,
                out SimplexCache cache
            );

            //circle center is not inside polygon, use a and b from gjk to define collision
            if (output.Distance != 0)
            {
                var normal = output.PointB - output.PointA;
                var lengthSquared = normal.LengthSquared();

                if (lengthSquared < circle.Radius * circle.Radius)
                {
                    var length = MathF.Sqrt(lengthSquared);
                    manifold.Count = 1;
                    manifold.ContactPoints[0] = output.PointB;
                    manifold.Depths[0] = circle.Radius - length;
                    Vector2.Normalize(ref normal, out manifold.Normal);

                    return true;
                }

                return false;
            }
            //circle center is inside polygon, find the face closest to the center to form manifold
            else
            {
                var transform = polygon.Transform;
                var separation = float.MinValue;
                var index = -1;
                var local = GJKHelper.Mul(transform, circle.Center);

                for (var i = 0; i < polygon.Vertices.Count; i++)
                {
                    var h = HalfSpace.At(polygon, i);
                    var d = Vector2.Dot(h.Normal, local) - h.Distance;

                    if (d > circle.Radius)
                    {
                        return false;
                    }
                    if (d > separation)
                    {
                        separation = d;
                        index = i;
                    }
                }

                var h2 = HalfSpace.At(polygon, index);
                var p = h2.Project(local);

                manifold.Count = 1;
                manifold.ContactPoints[0] = GJKHelper.Mul(transform, p);
                manifold.Depths[0] = circle.Radius - separation;
                manifold.Normal = GJKHelper.Mul(transform.Rotation, polygon.Normals[index]) * -1.0f;

                return true;
            }
        }
    }
}

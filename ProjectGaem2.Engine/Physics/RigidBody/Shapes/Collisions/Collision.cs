using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions
{
    public static class Collision
    {
        public static bool Collides(
            Shape first,
            Transform firstT,
            Shape second,
            Transform secondT,
            out Manifold manifold
        )
        {
            manifold = new Manifold();

            switch (first)
            {
                case Circle:
                    switch (second)
                    {
                        case Circle:
                            return CircleToCircleManifold(
                                (Circle)first,
                                (Circle)second,
                                out manifold
                            );
                        case Capsule2D:
                            return CircleToCapsule2DManifold(
                                (Circle)first,
                                firstT,
                                (Capsule2D)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotImplementedException();
                    }
                case Capsule2D:
                    switch (second)
                    {
                        case Circle:
                            var result = CircleToCapsule2DManifold(
                                (Circle)second,
                                firstT,
                                (Capsule2D)first,
                                secondT,
                                out manifold
                            );
                            manifold.Invert();
                            return result;
                        case Capsule2D:
                            return Capsule2DToCapsule2DManifold(
                                (Capsule2D)first,
                                firstT,
                                (Capsule2D)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool Overlaps(Shape first, Transform firstT, Shape second, Transform secondT)
        {
            return first switch
            {
                Circle
                    => second switch
                    {
                        Circle => CircleToCircle((Circle)first, (Circle)second),
                        Capsule2D => CircleToCapsule2D((Circle)first, (Capsule2D)second),
                        _ => throw new NotImplementedException(),
                    },
                Capsule2D
                    => second switch
                    {
                        Circle => CircleToCapsule2D((Circle)second, (Capsule2D)first),
                        Capsule2D
                            => Capsule2DToCapsule2D(
                                (Capsule2D)first,
                                firstT,
                                (Capsule2D)second,
                                secondT
                            ),
                        _ => throw new NotImplementedException(),
                    },
                _ => throw new NotImplementedException()
            };
        }

        public static bool CircleToCircle(Circle first, Circle second)
        {
            return Vector2.DistanceSquared(first.Center, second.Center)
                < (first.Radius + second.Radius) * (first.Radius + second.Radius);
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

        public static bool Capsule2DToCapsule2D(
            Capsule2D first,
            Transform firstT,
            Capsule2D second,
            Transform secondT
        )
        {
            GJK.Compute(
                first,
                firstT,
                second,
                secondT,
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
                var n = d != 0 ? Vector2.Normalize(dVector) : new Vector2(0, 1f);

                manifold.Count = 1;
                manifold.Depths[0] = radiiSum - d;
                manifold.ContactPoints[0] = second.Center - n * second.Center;
                manifold.Normal = n;

                return true;
            }

            return false;
        }

        public static bool CircleToCapsule2DManifold(
            Circle circle,
            Transform circleT,
            Capsule2D capsule,
            Transform capsuleT,
            out Manifold manifold
        )
        {
            manifold = new Manifold();
            var radiiSum = circle.Radius + capsule.Radius;

            GJK.Compute(
                circle,
                circleT,
                capsule,
                capsuleT,
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

        public static bool Capsule2DToCapsule2DManifold(
            Capsule2D first,
            Transform firstT,
            Capsule2D second,
            Transform secondT,
            out Manifold manifold
        )
        {
            manifold = new Manifold();
            var radiiSum = first.Radius + second.Radius;

            GJK.Compute(
                first,
                firstT,
                second,
                secondT,
                false,
                out GJKOutput output,
                out SimplexCache cache
            );

            if (output.Distance < radiiSum)
            {
                Vector2 normal;
                if (output.Distance == 0)
                {
                    normal = Vector2.Normalize(Vector2Ext.Skew(first.End - first.Start));
                }
                else
                {
                    normal = Vector2.Normalize(output.PointB - output.PointA);
                }

                manifold.Count = 1;
                manifold.Depths[0] = radiiSum - output.Distance;
                manifold.ContactPoints[0] = output.PointB - normal * second.Radius;
                manifold.Normal = normal;

                return true;
            }

            return false;
        }
    }
}

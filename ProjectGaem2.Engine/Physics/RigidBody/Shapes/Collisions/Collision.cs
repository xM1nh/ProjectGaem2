using System;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions
{
    public static partial class Collision
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
                        case Box2D:
                            return CircleToBox2DManifold(
                                (Circle)first,
                                (Box2D)second,
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
                case Box2D:
                    switch (second)
                    {
                        case Circle:
                            var result = CircleToBox2DManifold(
                                (Circle)second,
                                (Box2D)first,
                                out manifold
                            );
                            manifold.Invert();
                            return result;
                        case Box2D:
                            return Box2DToBox2DManifold((Box2D)first, (Box2D)second, out manifold);
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
                        Box2D => CircleToBox2D((Circle)first, (Box2D)second),
                        Capsule2D => CircleToCapsule2D((Circle)first, (Capsule2D)second),
                        _ => throw new NotImplementedException(),
                    },
                Box2D
                    => second switch
                    {
                        Circle => CircleToBox2D((Circle)second, (Box2D)first),
                        Box2D => Box2DToBox2D((Box2D)first, (Box2D)second),
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
            ;
        }
    }
}

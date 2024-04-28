using System;
using ProjectGaem2.Engine.ECS.Components;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
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
                        case Polygon:
                            return CircleToPolygonManifold(
                                (Circle)first,
                                firstT,
                                (Polygon)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotSupportedException();
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
                        case Capsule2D:
                            return Box2DToCapsule2DManifold(
                                (Box2D)first,
                                firstT,
                                (Capsule2D)second,
                                secondT,
                                out manifold
                            );
                        case Polygon:
                            return Box2DToPolygonManifold(
                                (Box2D)first,
                                firstT,
                                (Polygon)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotSupportedException();
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
                        case Box2D:
                            var result2 = Box2DToCapsule2DManifold(
                                (Box2D)second,
                                secondT,
                                (Capsule2D)first,
                                firstT,
                                out manifold
                            );
                            manifold.Invert();
                            return result2;
                        case Polygon:
                            return Capsule2DToPolygonManifold(
                                (Capsule2D)first,
                                firstT,
                                (Polygon)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotSupportedException();
                    }
                case Polygon:
                    switch (second)
                    {
                        case Circle:
                            var result = CircleToPolygonManifold(
                                (Circle)second,
                                secondT,
                                (Polygon)first,
                                firstT,
                                out manifold
                            );
                            manifold.Invert();
                            return result;
                        case Box2D:
                            var result2 = Box2DToPolygonManifold(
                                (Box2D)second,
                                secondT,
                                (Polygon)first,
                                firstT,
                                out manifold
                            );
                            manifold.Invert();
                            return result2;
                        case Capsule2D:
                            var result3 = Capsule2DToPolygonManifold(
                                (Capsule2D)second,
                                secondT,
                                (Polygon)first,
                                firstT,
                                out manifold
                            );
                            manifold.Invert();
                            return result3;
                        case Polygon:
                            return PolygonToPolygonManifold(
                                (Polygon)first,
                                firstT,
                                (Polygon)second,
                                secondT,
                                out manifold
                            );
                        default:
                            throw new NotSupportedException();
                    }
                default:
                    throw new NotSupportedException();
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
                        Polygon => CircleToPolygon((Circle)first, firstT, (Polygon)second, secondT),
                        _ => throw new NotSupportedException(),
                    },
                Box2D
                    => second switch
                    {
                        Circle => CircleToBox2D((Circle)second, (Box2D)first),
                        Box2D => Box2DToBox2D((Box2D)first, (Box2D)second),
                        Capsule2D
                            => Box2DToCapsule2D((Box2D)first, firstT, (Capsule2D)second, secondT),
                        Polygon => Box2DToPolygon((Box2D)first, firstT, (Polygon)second, secondT),
                        _ => throw new NotSupportedException(),
                    },
                Capsule2D
                    => second switch
                    {
                        Circle => CircleToCapsule2D((Circle)second, (Capsule2D)first),
                        Box2D => Box2DToCapsule2D((Box2D)second, secondT, (Capsule2D)first, firstT),
                        Capsule2D
                            => Capsule2DToCapsule2D(
                                (Capsule2D)first,
                                firstT,
                                (Capsule2D)second,
                                secondT
                            ),
                        Polygon
                            => Capsule2DToPolygon(
                                (Capsule2D)first,
                                firstT,
                                (Polygon)second,
                                secondT
                            ),
                        _ => throw new NotSupportedException(),
                    },
                Polygon
                    => second switch
                    {
                        Circle => CircleToPolygon((Circle)second, secondT, (Polygon)first, firstT),
                        Box2D => Box2DToPolygon((Box2D)second, secondT, (Polygon)first, firstT),
                        Capsule2D
                            => Capsule2DToPolygon(
                                (Capsule2D)second,
                                secondT,
                                (Polygon)first,
                                firstT
                            ),
                        Polygon
                            => PolygonToPolygon((Polygon)first, firstT, (Polygon)second, secondT),
                        _ => throw new NotSupportedException()
                    },
                _ => throw new NotSupportedException()
            };
        }
    }
}

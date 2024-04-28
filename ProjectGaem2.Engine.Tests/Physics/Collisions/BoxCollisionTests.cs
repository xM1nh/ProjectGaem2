using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Collisions
{
    public class BoxCollisionTests
    {
        [Fact]
        public void BoxToBox_Collide_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = Vector2.One, Max = new Vector2(3, 4) };
            var expectedManifold = new Manifold() { Count = 1, Normal = new Vector2(0, 1) };
            expectedManifold.ContactPoints[0] = new Vector2(1, 2);
            expectedManifold.Depths[0] = 1;

            //Act
            var result = Collision.Box2DToBox2DManifold(first, second, out Manifold actualManifold);

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToBox_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = new Vector2(2, 0), Max = new Vector2(4, 2) };
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToBox2DManifold(first, second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToBox_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = new Vector2(3, 0), Max = new Vector2(4, 4) };
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToBox2DManifold(first, second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToCapsule_TopBottomCollide_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, 3), new Vector2(4, 3), 2);
            var transform = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToCapsule2DManifold(
                first,
                transform,
                second,
                transform,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToCapsule_SideCollide_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, -2), new Vector2(3, 5), 2);
            var transform = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToCapsule2DManifold(
                first,
                transform,
                second,
                transform,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToCapsule_TopBottomTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, 1), new Vector2(5, 1), 1);
            var transform = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToCapsule2DManifold(
                first,
                transform,
                second,
                transform,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToCapsule_SideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, -1), new Vector2(3, 3), 1);
            var transform = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToCapsule2DManifold(
                first,
                transform,
                second,
                transform,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void BoxToCapsule_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(4, -1), new Vector2(4, 3), 1);
            var transform = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Box2DToCapsule2DManifold(
                first,
                transform,
                second,
                transform,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }
    }
}

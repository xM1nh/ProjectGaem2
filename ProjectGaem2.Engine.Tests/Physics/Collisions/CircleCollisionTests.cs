using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Tests.Physics.Collisions
{
    public class CircleCollisionTests
    {
        [Fact]
        public void CircleToCircle_Collide_ReturnTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Circle(new Vector2(1, 0), 1);
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), Count = 1 };
            expectedManifold.ContactPoints[0] = Vector2.Zero;
            expectedManifold.Depths[0] = 2;

            //Act
            var result = Collision.CircleToCircleManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCircle_Tangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Circle(new Vector2(2, 0), 1);
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCircleManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCircle_NotCollide_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Circle(new Vector2(3, 0), 1);
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCircleManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToBox_CenterInsideCollide_ReturnsTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(3, 2), 2);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };
            var expectedManifold = new Manifold() { Count = 1, Normal = new Vector2(-1, 0) };
            expectedManifold.ContactPoints[0] = new Vector2(4, 2);
            expectedManifold.Depths[0] = 3;

            //Act
            var result = Collision.CircleToBox2DManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToBox_CenterOutsideCollide_ReturnsTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(5, 2), 2);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };
            var expectedManifold = new Manifold() { Count = 1, Normal = new Vector2(-1, 0) };
            expectedManifold.ContactPoints[0] = new Vector2(4, 2);
            expectedManifold.Depths[0] = 1;

            //Act
            var result = Collision.CircleToBox2DManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToBox_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(5, 2), 1);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.CircleToBox2DManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToBox_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(6, 2), 1);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.CircleToBox2DManifold(
                first,
                second,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_TopBottomCollide_ReturnTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Capsule2D(new Vector2(1, 0), new Vector2(2, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), Count = 1 };
            expectedManifold.ContactPoints[0] = Vector2.Zero;
            expectedManifold.Depths[0] = 2;

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_MiddleCollide_ReturnTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(1, 4), new Vector2(1, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), Count = 1 };
            expectedManifold.ContactPoints[0] = new Vector2(0, 2);
            expectedManifold.Depths[0] = 1;

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeTrue();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_TopBottomTangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(3, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_MiddleTangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(2, 4), new Vector2(2, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_TopBottomNotCollide_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Capsule2D(new Vector2(3, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
                out Manifold actualManifold
            );

            //Assert
            result.Should().BeFalse();
            actualManifold
                .Should()
                .BeEquivalentTo(expectedManifold, o => o.ComparingByMembers<Manifold>());
        }

        [Fact]
        public void CircleToCapsule_MiddleNotCollide_ReturnFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(3, 4), new Vector2(3, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { };

            //Act
            var result = Collision.CircleToCapsule2DManifold(
                first,
                transfrom,
                second,
                transfrom,
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

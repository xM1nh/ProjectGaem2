using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Collisions
{
    public partial class CollisionTests
    {
        [Fact]
        public void CircleToCircle_Collide_ReturnTrue()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 2 };
            var second = new CircleBody() { Position = new Vector2(1, 0), Radius = 1 };
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), };
            expectedManifold.ContactPoints[0] = Vector2.Zero;

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeTrue();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCircle_Tangent_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CircleBody() { Position = new Vector2(2, 0), Radius = 1 };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCircle_NotCollide_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CircleBody() { Position = new Vector2(3, 0), Radius = 1 };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_TopBottomCollide_ReturnTrue()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 2 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(1, 0),
                End = new Vector2(2, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), };
            expectedManifold.ContactPoints[0] = Vector2.Zero;

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeTrue();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_MiddleCollide_ReturnTrue()
        {
            //Arrange
            var first = new CircleBody() { Position = new Vector2(0, 2), Radius = 1 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(1, 4),
                End = new Vector2(1, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), };
            expectedManifold.ContactPoints[0] = new Vector2(0, 2);

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeTrue();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_TopBottomTangent_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(2, 0),
                End = new Vector2(3, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_MiddleTangent_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = new Vector2(0, 2), Radius = 1 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(2, 4),
                End = new Vector2(2, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_TopBottomNotCollide_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(3, 0),
                End = new Vector2(4, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }

        [Fact]
        public void CircleToCapsule_MiddleNotCollide_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = new Vector2(0, 2), Radius = 1 };
            var second = new CapsuleBody()
            {
                Start = new Vector2(3, 4),
                End = new Vector2(3, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { };

            //Act
            var result = first.Collides(second, out Manifold actualManifold);

            //Assert
            result.Should().BeFalse();
            actualManifold.Normal.Should().Be(expectedManifold.Normal);
            actualManifold
                .ContactPoints.Should()
                .BeEquivalentTo(expectedManifold.ContactPoints, o => o.WithStrictOrdering());
        }
    }
}

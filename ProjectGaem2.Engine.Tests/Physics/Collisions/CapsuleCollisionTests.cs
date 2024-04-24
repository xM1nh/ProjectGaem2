using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Collisions
{
    public class CapsuleCollisionTests
    {
        [Fact]
        public void CapsuleToCapsule_Collide_ReturnsTrue()
        {
            //Arrange
            var first = new CapsuleBody()
            {
                Start = Vector2.Zero,
                End = new Vector2(0, 4),
                Radius = 2
            };
            var second = new CapsuleBody()
            {
                Start = new Vector2(2, 0),
                End = new Vector2(4, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), };
            expectedManifold.ContactPoints[0] = new Vector2(1, 0);

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
        public void CapsuleToCapsule_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new CapsuleBody()
            {
                Start = Vector2.Zero,
                End = new Vector2(0, 4),
                Radius = 1
            };
            var second = new CapsuleBody()
            {
                Start = new Vector2(2, 0),
                End = new Vector2(4, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold();

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
        public void CapsuleToCapsule_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new CapsuleBody()
            {
                Start = Vector2.Zero,
                End = new Vector2(0, 4),
                Radius = 1
            };
            var second = new CapsuleBody()
            {
                Start = new Vector2(3, 0),
                End = new Vector2(4, 0),
                Radius = 1
            };
            var expectedManifold = new Manifold();

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

using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Collisions
{
    public class CapsuleCollisionTests
    {
        [Fact]
        public void CapsuleToCapsule_Collide_ReturnsTrue()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 2);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold() { Normal = new Vector2(1, 0), Count = 1 };
            expectedManifold.ContactPoints[0] = new Vector2(1, 0);
            expectedManifold.Depths[0] = 1;

            //Act
            var result = Collision.Capsule2DToCapsule2DManifold(
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
        public void CapsuleToCapsule_SideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(2, 4), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Capsule2DToCapsule2DManifold(
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
        public void CapsuleToCapsule_TopBottomTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Capsule2DToCapsule2DManifold(
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
        public void CapsuleToCapsule_TopBottomSideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 2), new Vector2(4, 2), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Capsule2DToCapsule2DManifold(
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
        public void CapsuleToCapsule_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(3, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();
            var expectedManifold = new Manifold();

            //Act
            var result = Collision.Capsule2DToCapsule2DManifold(
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

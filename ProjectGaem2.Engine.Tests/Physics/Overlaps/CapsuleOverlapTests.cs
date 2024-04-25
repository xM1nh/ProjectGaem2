using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class CapsuleOverlapTests
    {
        [Fact]
        public void CapsuleToCapsule_Collide_ReturnsTrue()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 2);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CapsuleToCapsule_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CapsuleToCapsule_NotCollide_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(3, 0), new Vector2(4, 0), 1);
            var transfrom = Transform.Identity();

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }
    }
}

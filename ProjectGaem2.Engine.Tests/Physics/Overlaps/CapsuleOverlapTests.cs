using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class CapsuleOverlapTests
    {
        [Fact]
        public void CapsuleToCapsule_Overlap_ReturnsTrue()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 2);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CapsuleToCapsule_SideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(2, 4), 1);
            var transfrom = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CapsuleToCapsule_TopBottomTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(4, 0), 1);
            var transfrom = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CapsuleToCapsule_TopBottomSideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(2, 2), new Vector2(4, 2), 1);
            var transfrom = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CapsuleToCapsule_NotOverlap_ReturnsFalse()
        {
            //Arrange
            var first = new Capsule2D(Vector2.Zero, new Vector2(0, 4), 1);
            var second = new Capsule2D(new Vector2(3, 0), new Vector2(4, 0), 1);
            var transfrom = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Capsule2DToCapsule2D(first, transfrom, second, transfrom);

            //Assert
            result.Should().BeFalse();
        }
    }
}

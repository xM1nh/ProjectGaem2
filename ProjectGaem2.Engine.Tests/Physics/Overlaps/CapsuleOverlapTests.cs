using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class CapsuleOverlapTests
    {
        [Fact]
        public void CapsuleToCapsule_Overlap_ReturnsTrue()
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

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeTrue();
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

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CapsuleToCapsule_NotOverlap_ReturnsFalse()
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

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeFalse();
        }
    }
}

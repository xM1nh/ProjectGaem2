using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public partial class OverlapTests
    {
        [Fact]
        public void CircleToCircle_SameCenter_ReturnTrue()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CircleBody() { Position = Vector2.Zero, Radius = 0 };

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCircle_Overlap_ReturnTrue()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 2 };
            var second = new CircleBody() { Position = new Vector2(1, 0), Radius = 1 };

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCircle_Tangent_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CircleBody() { Position = new Vector2(2, 0), Radius = 1 };

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCircle_NotOverlap_ReturnFalse()
        {
            //Arrange
            var first = new CircleBody() { Position = Vector2.Zero, Radius = 1 };
            var second = new CircleBody() { Position = new Vector2(3, 0), Radius = 1 };

            //Act
            var result = first.Overlaps(second);

            //Assert
            result.Should().BeFalse();
        }
    }
}

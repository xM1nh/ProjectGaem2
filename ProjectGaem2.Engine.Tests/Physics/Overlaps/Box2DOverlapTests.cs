using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class Box2DOverlapTests
    {
        [Fact]
        public void BoxToBox_Overlap_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = Vector2.One, Max = new Vector2(3, 4) };

            //Act
            var result = Collision.Box2DToBox2D(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BoxToBox_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = new Vector2(2, 0), Max = new Vector2(4, 2) };

            //Act
            var result = Collision.Box2DToBox2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void BoxToBox_NotOverlap_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Box2D() { Min = new Vector2(3, 0), Max = new Vector2(4, 4) };

            //Act
            var result = Collision.Box2DToBox2D(first, second);

            //Assert
            result.Should().BeFalse();
        }
    }
}

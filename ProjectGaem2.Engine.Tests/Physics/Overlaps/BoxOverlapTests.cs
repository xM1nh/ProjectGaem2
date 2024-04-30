using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class BoxOverlapTests
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

        [Fact]
        public void BoxToCapsule_TopBottomOverlap_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, 3), new Vector2(4, 3), 2);

            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Box2DToCapsule2D(first, transform, second, transform);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BoxToCapsule_SideOverlap_ReturnsTrue()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, -2), new Vector2(3, 5), 2);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Box2DToCapsule2D(first, transform, second, transform);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BoxToCapsule_TopBottomTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, 1), new Vector2(5, 1), 1);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Box2DToCapsule2D(first, transform, second, transform);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void BoxToCapsule_SideTangent_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(3, -1), new Vector2(3, 3), 1);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Box2DToCapsule2D(first, transform, second, transform);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void BoxToCapsule_NotOverlap_ReturnsFalse()
        {
            //Arrange
            var first = new Box2D() { Min = Vector2.Zero, Max = new Vector2(2, 2) };
            var second = new Capsule2D(new Vector2(4, -1), new Vector2(4, 3), 1);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.Box2DToCapsule2D(first, transform, second, transform);

            //Assert
            result.Should().BeFalse();
        }
    }
}

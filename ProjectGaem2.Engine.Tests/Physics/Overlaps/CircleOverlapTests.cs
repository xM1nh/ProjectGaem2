using FluentAssertions;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Tests.Physics.Overlaps
{
    public class CircleOverlapTests
    {
        [Fact]
        public void CircleToCircle_SameCenter_ReturnTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Circle(Vector2.Zero, 1);

            //Act
            var result = Collision.CircleToCircle(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCircle_Overlap_ReturnTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Circle(new Vector2(1, 0), 1);

            //Act
            var result = Collision.CircleToCircle(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCircle_Tangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Circle(new Vector2(2, 0), 1);

            //Act
            var result = Collision.CircleToCircle(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCircle_NotOverlap_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Circle(new Vector2(3, 0), 1);

            //Act
            var result = Collision.CircleToCircle(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToBox_CenterInsideOverlap_ReturnsTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(3, 2), 2);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };

            //Act
            var result = Collision.CircleToBox2D(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToBox_CenterOutsideOverlap_ReturnsTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(5, 2), 2);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };

            //Act
            var result = Collision.CircleToBox2D(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToBox_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(5, 2), 1);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };

            //Act
            var result = Collision.CircleToBox2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToBox_NotOverlap_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(6, 2), 1);
            var second = new Box2D() { Min = Vector2.Zero, Max = new Vector2(4, 4) };

            //Act
            var result = Collision.CircleToBox2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCapsule_TopBottomOverlap_ReturnTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Capsule2D(new Vector2(1, 0), new Vector2(2, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCapsule_MiddleOverlap_ReturnTrue()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(1, 4), new Vector2(1, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToCapsule_TopBottomTangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Capsule2D(new Vector2(2, 0), new Vector2(3, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCapsule_MiddleTangent_ReturnFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(2, 4), new Vector2(2, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCapsule_TopBottomNotOverlap_ReturnFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Capsule2D(new Vector2(3, 0), new Vector2(4, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToCapsule_MiddleNotOverlap_ReturnFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Capsule2D(new Vector2(3, 4), new Vector2(3, 0), 1);

            //Act
            var result = Collision.CircleToCapsule2D(first, second);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToPolygon_Overlap_ReturnsTrue()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 2);
            var second = new Polygon();
            List<Vector2> vertices = [Vector2.One, new Vector2(1, 3), new Vector2(3, 1)];
            second.SetVertices(vertices);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.CircleToPolygon(first, transform, second, transform);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CircleToPolygon_Tangent_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(Vector2.Zero, 1);
            var second = new Polygon();
            List<Vector2> vertices = [Vector2.One, new Vector2(1, 3), new Vector2(3, 1)];
            second.SetVertices(vertices);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.CircleToPolygon(first, transform, second, transform);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CircleToPolygon_NotOverlap_ReturnsFalse()
        {
            //Arrange
            var first = new Circle(new Vector2(0, 2), 1);
            var second = new Polygon();
            List<Vector2> vertices = [new Vector2(2, 2), new Vector2(2, 3), new Vector2(3, 2)];
            second.SetVertices(vertices);
            var transform = PhysicsInternalTransform.Identity;

            //Act
            var result = Collision.CircleToPolygon(first, transform, second, transform);

            //Assert
            result.Should().BeFalse();
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Utils.Math
{
    //
    // Summary:
    //     Describes a 2D-rectangle.
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct RectangleF : IEquatable<RectangleF>
    {
        private static RectangleF emptyRectangle;
        private static Matrix2 _tempMat,
            _transformMat;

        //
        // Summary:
        //     The x coordinate of the top-left corner of this Microsoft.Xna.Framework.Rectangle.
        [DataMember]
        public float X;

        //
        // Summary:
        //     The y coordinate of the top-left corner of this Microsoft.Xna.Framework.Rectangle.
        [DataMember]
        public float Y;

        //
        // Summary:
        //     The width of this Microsoft.Xna.Framework.Rectangle.
        [DataMember]
        public float Width;

        //
        // Summary:
        //     The height of this Microsoft.Xna.Framework.Rectangle.
        [DataMember]
        public float Height;

        //
        // Summary:
        //     Returns a Microsoft.Xna.Framework.Rectangle with X=0, Y=0, Width=0, Height=0.
        public static RectangleF Empty => emptyRectangle;

        //
        // Summary:
        //     Returns the x coordinate of the left edge of this Microsoft.Xna.Framework.Rectangle.
        public float Left => X;

        //
        // Summary:
        //     Returns the x coordinate of the right edge of this Microsoft.Xna.Framework.Rectangle.
        public float Right => X + Width;

        //
        // Summary:
        //     Returns the y coordinate of the top edge of this Microsoft.Xna.Framework.Rectangle.
        public float Top => Y;

        //
        // Summary:
        //     Returns the y coordinate of the bottom edge of this Microsoft.Xna.Framework.Rectangle.
        public float Bottom => Y + Height;

        //
        // Summary:
        //     Whether or not this Microsoft.Xna.Framework.Rectangle has a Microsoft.Xna.Framework.Rectangle.Width
        //     and Microsoft.Xna.Framework.Rectangle.Height of 0, and a Microsoft.Xna.Framework.Rectangle.Location
        //     of (0, 0).
        public bool IsEmpty
        {
            get
            {
                if (Width == 0 && Height == 0 && X == 0)
                {
                    return Y == 0;
                }

                return false;
            }
        }

        //
        // Summary:
        //     The top-left coordinates of this Microsoft.Xna.Framework.Rectangle.
        public Vector2 Location
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        //
        // Summary:
        //     The width-height coordinates of this Microsoft.Xna.Framework.Rectangle.
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        //
        // Summary:
        //     A Microsoft.Xna.Framework.Point located in the center of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Remarks:
        //     If Microsoft.Xna.Framework.Rectangle.Width or Microsoft.Xna.Framework.Rectangle.Height
        //     is an odd number, the center point will be rounded down.
        public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);

        internal string DebugDisplayString => X + "  " + Y + "  " + Width + "  " + Height;

        //
        // Summary:
        //     Creates a new instance of Microsoft.Xna.Framework.Rectangle struct, with the
        //     specified position, width, and height.
        //
        // Parameters:
        //   x:
        //     The x coordinate of the top-left corner of the created Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   y:
        //     The y coordinate of the top-left corner of the created Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   width:
        //     The width of the created Microsoft.Xna.Framework.Rectangle.
        //
        //   height:
        //     The height of the created Microsoft.Xna.Framework.Rectangle.
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        //
        // Summary:
        //     Creates a new instance of Microsoft.Xna.Framework.Rectangle struct, with the
        //     specified location and size.
        //
        // Parameters:
        //   location:
        //     The x and y coordinates of the top-left corner of the created Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   size:
        //     The width and height of the created Microsoft.Xna.Framework.Rectangle.
        public RectangleF(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.X;
            Height = size.Y;
        }

        //
        // Summary:
        //     Compares whether two Microsoft.Xna.Framework.Rectangle instances are equal.
        //
        // Parameters:
        //   a:
        //     Microsoft.Xna.Framework.Rectangle instance on the left of the equal sign.
        //
        //   b:
        //     Microsoft.Xna.Framework.Rectangle instance on the right of the equal sign.
        //
        // Returns:
        //     true if the instances are equal; false otherwise.
        public static bool operator ==(RectangleF a, RectangleF b)
        {
            if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
            {
                return a.Height == b.Height;
            }

            return false;
        }

        //
        // Summary:
        //     Compares whether two Microsoft.Xna.Framework.Rectangle instances are not equal.
        //
        //
        // Parameters:
        //   a:
        //     Microsoft.Xna.Framework.Rectangle instance on the left of the not equal sign.
        //
        //
        //   b:
        //     Microsoft.Xna.Framework.Rectangle instance on the right of the not equal sign.
        //
        //
        // Returns:
        //     true if the instances are not equal; false otherwise.
        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        //
        // Summary:
        //     Gets whether or not the provided coordinates lie within the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   x:
        //     The x coordinate of the point to check for containment.
        //
        //   y:
        //     The y coordinate of the point to check for containment.
        //
        // Returns:
        //     true if the provided coordinates lie inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise.
        public bool Contains(int x, int y)
        {
            if (X <= x && x < X + Width && Y <= y)
            {
                return y < Y + Height;
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the provided coordinates lie within the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   x:
        //     The x coordinate of the point to check for containment.
        //
        //   y:
        //     The y coordinate of the point to check for containment.
        //
        // Returns:
        //     true if the provided coordinates lie inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise.
        public bool Contains(float x, float y)
        {
            if ((float)X <= x && x < (float)(X + Width) && (float)Y <= y)
            {
                return y < (float)(Y + Height);
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Point lies within the
        //     bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The coordinates to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Returns:
        //     true if the provided Microsoft.Xna.Framework.Point lies inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise.
        public bool Contains(Point value)
        {
            if (X <= value.X && value.X < X + Width && Y <= value.Y)
            {
                return value.Y < Y + Height;
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Point lies within the
        //     bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The coordinates to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   result:
        //     true if the provided Microsoft.Xna.Framework.Point lies inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise. As an output parameter.
        public void Contains(ref Point value, out bool result)
        {
            result = X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Vector2 lies within
        //     the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The coordinates to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Returns:
        //     true if the provided Microsoft.Xna.Framework.Vector2 lies inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise.
        public bool Contains(Vector2 value)
        {
            if ((float)X <= value.X && value.X < (float)(X + Width) && (float)Y <= value.Y)
            {
                return value.Y < (float)(Y + Height);
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Vector2 lies within
        //     the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The coordinates to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   result:
        //     true if the provided Microsoft.Xna.Framework.Vector2 lies inside this Microsoft.Xna.Framework.Rectangle;
        //     false otherwise. As an output parameter.
        public void Contains(ref Vector2 value, out bool result)
        {
            result =
                (float)X <= value.X
                && value.X < (float)(X + Width)
                && (float)Y <= value.Y
                && value.Y < (float)(Y + Height);
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Rectangle lies within
        //     the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The Microsoft.Xna.Framework.Rectangle to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Returns:
        //     true if the provided Microsoft.Xna.Framework.Rectangle's bounds lie entirely
        //     inside this Microsoft.Xna.Framework.Rectangle; false otherwise.
        public bool Contains(RectangleF value)
        {
            if (X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y)
            {
                return value.Y + value.Height <= Y + Height;
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the provided Microsoft.Xna.Framework.Rectangle lies within
        //     the bounds of this Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   value:
        //     The Microsoft.Xna.Framework.Rectangle to check for inclusion in this Microsoft.Xna.Framework.Rectangle.
        //
        //
        //   result:
        //     true if the provided Microsoft.Xna.Framework.Rectangle's bounds lie entirely
        //     inside this Microsoft.Xna.Framework.Rectangle; false otherwise. As an output
        //     parameter.
        public void Contains(ref RectangleF value, out bool result)
        {
            result =
                X <= value.X
                && value.X + value.Width <= X + Width
                && Y <= value.Y
                && value.Y + value.Height <= Y + Height;
        }

        //
        // Summary:
        //     Compares whether current instance is equal to specified System.Object.
        //
        // Parameters:
        //   obj:
        //     The System.Object to compare.
        //
        // Returns:
        //     true if the instances are equal; false otherwise.
        public override bool Equals(object obj)
        {
            if (obj is RectangleF)
            {
                return this == (RectangleF)obj;
            }

            return false;
        }

        //
        // Summary:
        //     Compares whether current instance is equal to specified Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   other:
        //     The Microsoft.Xna.Framework.Rectangle to compare.
        //
        // Returns:
        //     true if the instances are equal; false otherwise.
        public bool Equals(RectangleF other)
        {
            return this == other;
        }

        //
        // Summary:
        //     Gets the hash code of this Microsoft.Xna.Framework.Rectangle.
        //
        // Returns:
        //     Hash code of this Microsoft.Xna.Framework.Rectangle.
        public override int GetHashCode()
        {
            return (((17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode()) * 23 + Width.GetHashCode())
                    * 23
                + Height.GetHashCode();
        }

        //
        // Summary:
        //     Adjusts the edges of this Microsoft.Xna.Framework.Rectangle by specified horizontal
        //     and vertical amounts.
        //
        // Parameters:
        //   horizontalAmount:
        //     Value to adjust the left and right edges.
        //
        //   verticalAmount:
        //     Value to adjust the top and bottom edges.
        public void Inflate(int horizontalAmount, int verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        //
        // Summary:
        //     Adjusts the edges of this Microsoft.Xna.Framework.Rectangle by specified horizontal
        //     and vertical amounts.
        //
        // Parameters:
        //   horizontalAmount:
        //     Value to adjust the left and right edges.
        //
        //   verticalAmount:
        //     Value to adjust the top and bottom edges.
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= (int)horizontalAmount;
            Y -= (int)verticalAmount;
            Width += (int)horizontalAmount * 2;
            Height += (int)verticalAmount * 2;
        }

        //
        // Summary:
        //     Gets whether or not the other Microsoft.Xna.Framework.Rectangle intersects with
        //     this rectangle.
        //
        // Parameters:
        //   value:
        //     The other rectangle for testing.
        //
        // Returns:
        //     true if other Microsoft.Xna.Framework.Rectangle intersects with this rectangle;
        //     false otherwise.
        public bool Intersects(RectangleF value)
        {
            if (value.Left < Right && Left < value.Right && value.Top < Bottom)
            {
                return Top < value.Bottom;
            }

            return false;
        }

        //
        // Summary:
        //     Gets whether or not the other Microsoft.Xna.Framework.Rectangle intersects with
        //     this rectangle.
        //
        // Parameters:
        //   value:
        //     The other rectangle for testing.
        //
        //   result:
        //     true if other Microsoft.Xna.Framework.Rectangle intersects with this rectangle;
        //     false otherwise. As an output parameter.
        public void Intersects(ref RectangleF value, out bool result)
        {
            result =
                value.Left < Right
                && Left < value.Right
                && value.Top < Bottom
                && Top < value.Bottom;
        }

        //
        // Summary:
        //     Creates a new Microsoft.Xna.Framework.Rectangle that contains overlapping region
        //     of two other rectangles.
        //
        // Parameters:
        //   value1:
        //     The first Microsoft.Xna.Framework.Rectangle.
        //
        //   value2:
        //     The second Microsoft.Xna.Framework.Rectangle.
        //
        // Returns:
        //     Overlapping region of the two rectangles.
        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            Intersect(ref value1, ref value2, out var result);
            return result;
        }

        //
        // Summary:
        //     Creates a new Microsoft.Xna.Framework.Rectangle that contains overlapping region
        //     of two other rectangles.
        //
        // Parameters:
        //   value1:
        //     The first Microsoft.Xna.Framework.Rectangle.
        //
        //   value2:
        //     The second Microsoft.Xna.Framework.Rectangle.
        //
        //   result:
        //     Overlapping region of the two rectangles as an output parameter.
        public static void Intersect(
            ref RectangleF value1,
            ref RectangleF value2,
            out RectangleF result
        )
        {
            if (value1.Intersects(value2))
            {
                float num = MathF.Min(value1.X + value1.Width, value2.X + value2.Width);
                float num2 = MathF.Max(value1.X, value2.X);
                float num3 = MathF.Max(value1.Y, value2.Y);
                float num4 = MathF.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new RectangleF(num2, num3, num - num2, num4 - num3);
            }
            else
            {
                result = new RectangleF(0, 0, 0, 0);
            }
        }

        //
        // Summary:
        //     Changes the Microsoft.Xna.Framework.Rectangle.Location of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   offsetX:
        //     The x coordinate to add to this Microsoft.Xna.Framework.Rectangle.
        //
        //   offsetY:
        //     The y coordinate to add to this Microsoft.Xna.Framework.Rectangle.
        public void Offset(int offsetX, int offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        //
        // Summary:
        //     Changes the Microsoft.Xna.Framework.Rectangle.Location of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   offsetX:
        //     The x coordinate to add to this Microsoft.Xna.Framework.Rectangle.
        //
        //   offsetY:
        //     The y coordinate to add to this Microsoft.Xna.Framework.Rectangle.
        public void Offset(float offsetX, float offsetY)
        {
            X += (int)offsetX;
            Y += (int)offsetY;
        }

        //
        // Summary:
        //     Changes the Microsoft.Xna.Framework.Rectangle.Location of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   amount:
        //     The x and y components to add to this Microsoft.Xna.Framework.Rectangle.
        public void Offset(Point amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        //
        // Summary:
        //     Changes the Microsoft.Xna.Framework.Rectangle.Location of this Microsoft.Xna.Framework.Rectangle.
        //
        //
        // Parameters:
        //   amount:
        //     The x and y components to add to this Microsoft.Xna.Framework.Rectangle.
        public void Offset(Vector2 amount)
        {
            X += (int)amount.X;
            Y += (int)amount.Y;
        }

        //
        // Summary:
        //     Returns a System.String representation of this Microsoft.Xna.Framework.Rectangle
        //     in the format: {X:[Microsoft.Xna.Framework.Rectangle.X] Y:[Microsoft.Xna.Framework.Rectangle.Y]
        //     Width:[Microsoft.Xna.Framework.Rectangle.Width] Height:[Microsoft.Xna.Framework.Rectangle.Height]}
        //
        //
        // Returns:
        //     System.String representation of this Microsoft.Xna.Framework.Rectangle.
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        //
        // Summary:
        //     Creates a new Microsoft.Xna.Framework.Rectangle that completely contains two
        //     other rectangles.
        //
        // Parameters:
        //   value1:
        //     The first Microsoft.Xna.Framework.Rectangle.
        //
        //   value2:
        //     The second Microsoft.Xna.Framework.Rectangle.
        //
        // Returns:
        //     The union of the two rectangles.
        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            float num = MathF.Min(value1.X, value2.X);
            float num2 = MathF.Min(value1.Y, value2.Y);
            return new RectangleF(
                num,
                num2,
                MathF.Max(value1.Right, value2.Right) - num,
                MathF.Max(value1.Bottom, value2.Bottom) - num2
            );
        }

        //
        // Summary:
        //     Creates a new Microsoft.Xna.Framework.Rectangle that completely contains two
        //     other rectangles.
        //
        // Parameters:
        //   value1:
        //     The first Microsoft.Xna.Framework.Rectangle.
        //
        //   value2:
        //     The second Microsoft.Xna.Framework.Rectangle.
        //
        //   result:
        //     The union of the two rectangles as an output parameter.
        public static void Union(
            ref RectangleF value1,
            ref RectangleF value2,
            out RectangleF result
        )
        {
            result.X = MathF.Min(value1.X, value2.X);
            result.Y = MathF.Min(value1.Y, value2.Y);
            result.Width = MathF.Max(value1.Right, value2.Right) - result.X;
            result.Height = MathF.Max(value1.Bottom, value2.Bottom) - result.Y;
        }

        //
        // Summary:
        //     Deconstruction method for Microsoft.Xna.Framework.Rectangle.
        //
        // Parameters:
        //   x:
        //
        //   y:
        //
        //   width:
        //
        //   height:
        public void Deconstruct(out float x, out float y, out float width, out float height)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
        }

        public void CalculateBounds(
            Vector2 position,
            Vector2 origin,
            float scale,
            float rotation,
            float width,
            float height
        )
        {
            if (rotation == 0f)
            {
                X = position.X - origin.X * scale;
                Y = position.Y - origin.Y * scale;
                Width = width * scale;
                Height = height * scale;
            }
            else
            {
                // special care for rotated bounds. we need to find our absolute min/max values and create the bounds from that
                var worldPosX = position.X;
                var worldPosY = position.Y;

                // set the reference point to world reference taking origin into account
                Matrix2.CreateTranslation(
                    -worldPosX - origin.X,
                    -worldPosY - origin.Y,
                    out _transformMat
                );
                Matrix2.CreateScale(scale, out _tempMat); // scale ->
                Matrix2.Multiply(ref _transformMat, ref _tempMat, out _transformMat);
                Matrix2.CreateRotation(rotation, out _tempMat); // rotate ->
                Matrix2.Multiply(ref _transformMat, ref _tempMat, out _transformMat);
                Matrix2.CreateTranslation(worldPosX, worldPosY, out _tempMat); // translate back
                Matrix2.Multiply(ref _transformMat, ref _tempMat, out _transformMat);

                // TODO: this is a bit silly. we can just leave the worldPos translation in the Matrix and avoid this
                // get all four corners in world space
                var topLeft = new Vector2(worldPosX, worldPosY);
                var topRight = new Vector2(worldPosX + width, worldPosY);
                var bottomLeft = new Vector2(worldPosX, worldPosY + height);
                var bottomRight = new Vector2(worldPosX + width, worldPosY + height);

                // transform the corners into our work space
                Vector2Ext.Transform(ref topLeft, ref _transformMat, out topLeft);
                Vector2Ext.Transform(ref topRight, ref _transformMat, out topRight);
                Vector2Ext.Transform(ref bottomLeft, ref _transformMat, out bottomLeft);
                Vector2Ext.Transform(ref bottomRight, ref _transformMat, out bottomRight);

                // find the min and max values so we can concoct our bounding box
                var minX = MathF.Min(
                    topLeft.X,
                    MathF.Min(bottomRight.X, MathF.Min(topRight.X, bottomLeft.X))
                );
                var maxX = MathF.Max(
                    topLeft.X,
                    MathF.Max(bottomRight.X, MathF.Max(topRight.X, bottomLeft.X))
                );
                var minY = MathF.Min(
                    topLeft.Y,
                    MathF.Min(bottomRight.Y, MathF.Min(topRight.Y, bottomLeft.Y))
                );
                var maxY = MathF.Max(
                    topLeft.Y,
                    MathF.Max(bottomRight.Y, MathF.Max(topRight.Y, bottomLeft.Y))
                );

                Location = new Vector2(minX, minY);
                Width = maxX - minX;
                Height = maxY - minY;
            }
        }
    }
}

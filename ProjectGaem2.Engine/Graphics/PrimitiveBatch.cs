using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.Graphics
{
    public class PrimitiveBatch(GraphicsDevice graphics, ContentManager content)
    {
        ShapeBatch _batch = new(graphics, content);

        public void Begin(Matrix? view = null, Matrix? projection = null) =>
            _batch.Begin(view, projection);

        public void DrawCircle(
            Vector2 center,
            float radius,
            Color c1,
            Color c2,
            float thickness = 1f
        ) => _batch.DrawCircle(center, radius, c1, c2, thickness);

        public void FillCircle(Vector2 center, float radius, Color c) =>
            DrawCircle(center, radius, c, c, 0f);

        public void BorderCircle(Vector2 center, float radius, Color c, float thickness = 1f) =>
            DrawCircle(center, radius, Color.Transparent, c, thickness);

        public void DrawRectangle(
            Vector2 xy,
            Vector2 size,
            Color c1,
            Color c2,
            float thickness = 1f,
            float rounded = 0f,
            float rotation = 0f
        ) => _batch.DrawRectangle(xy, size, c1, c2, thickness, rounded, rotation);

        public void FillRectangle(
            Vector2 xy,
            Vector2 size,
            Color c,
            float rounded = 0f,
            float rotation = 0f
        ) => DrawRectangle(xy, size, c, c, 0f, rounded, rotation);

        public void BorderRectangle(
            Vector2 xy,
            Vector2 size,
            Color c,
            float thickness = 1f,
            float rounded = 0f,
            float rotation = 0f
        ) => DrawRectangle(xy, size, Color.Transparent, c, thickness, rounded, rotation);

        public void DrawLine(
            Vector2 a,
            Vector2 b,
            float radius,
            Color c1,
            Color c2,
            float thickness = 1f
        ) => _batch.DrawLine(a, b, radius, c1, c2, thickness);

        public void FillLine(Vector2 a, Vector2 b, float radius, Color c) =>
            DrawLine(a, b, radius, c, c, 0f);

        public void BorderLine(Vector2 a, Vector2 b, float radius, Color c, float thickness = 1f) =>
            DrawLine(a, b, radius, Color.Transparent, c, thickness);

        public void DrawHexagon(
            Vector2 center,
            float radius,
            Color c1,
            Color c2,
            float thickness = 1f,
            float rounded = 0,
            float rotation = 0f
        ) => _batch.DrawHexagon(center, radius, c1, c2, thickness, rounded, rotation);

        public void FillHexagon(
            Vector2 center,
            float radius,
            Color c,
            float rounded = 0f,
            float rotation = 0f
        ) => DrawHexagon(center, radius, c, c, 0f, rounded, rotation);

        public void BorderHexagon(
            Vector2 center,
            float radius,
            Color c,
            float thickness = 1f,
            float rounded = 0f,
            float rotation = 0f
        ) => DrawHexagon(center, radius, Color.Transparent, c, thickness, rounded, rotation);

        public void DrawEquilateralTriangle(
            Vector2 center,
            float radius,
            Color c1,
            Color c2,
            float thickness = 1f,
            float rounded = 0f,
            float rotation = 0f
        ) => _batch.DrawEquilateralTriangle(center, radius, c1, c2, thickness, rounded, rotation);

        public void FillEquilateralTriangle(
            Vector2 center,
            float radius,
            Color c,
            float rounded = 0f,
            float rotation = 0f
        ) => DrawEquilateralTriangle(center, radius, c, c, 0f, rounded, rotation);

        public void BorderEquilateralTriangle(
            Vector2 center,
            float radius,
            Color c,
            float thickness = 1f,
            float rounded = 0f,
            float rotation = 0f
        ) =>
            DrawEquilateralTriangle(
                center,
                radius,
                Color.Transparent,
                c,
                thickness,
                rounded,
                rotation
            );

        public void DrawTriangle(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Color c1,
            Color c2,
            float thickness = 1f,
            float rounded = 0f
        ) => _batch.DrawTriangle(a, b, c, c1, c2, thickness, rounded);

        public void FillTriangle(Vector2 a, Vector2 b, Vector2 c, Color c1, float rounded = 0f) =>
            DrawTriangle(a, b, c, c1, c1, 0f, rounded);

        public void BorderTriangle(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Color c1,
            float thickness = 1f,
            float rounded = 0f
        ) => DrawTriangle(a, b, c, Color.Transparent, c1, thickness, rounded);

        public void DrawEllipse(
            Vector2 center,
            float radius1,
            float radius2,
            Color c1,
            Color c2,
            float thickness = 1f,
            float rotation = 0f
        ) => _batch.DrawEllipse(center, radius1, radius2, c1, c2, thickness, rotation);

        public void FillEllipse(
            Vector2 center,
            float width,
            float height,
            Color c,
            float rotation = 0f
        ) => DrawEllipse(center, width, height, c, c, 0f, rotation);

        public void BorderEllipse(
            Vector2 center,
            float width,
            float height,
            Color c,
            float thickness = 1f,
            float rotation = 0f
        ) => DrawEllipse(center, width, height, Color.Transparent, c, thickness, rotation);

        public void End() => _batch.End();
    }
}

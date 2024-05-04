using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS.Components.Renderables
{
    public class RenderableComponent : Component, IRenderable
    {
        protected bool _boundsDirty = true;
        protected RectangleF _bounds;

        public bool Visible { get; set; }
        public virtual float Width => Bounds.Width;
        public virtual float Height => Bounds.Height;

        public virtual RectangleF Bounds
        {
            get
            {
                if (_boundsDirty)
                {
                    _bounds.CalculateBounds(
                        Entity.Position,
                        Vector2.Zero,
                        Entity.Scale,
                        Entity.Rotation,
                        Width,
                        Height
                    );
                    _boundsDirty = false;
                }

                return _bounds;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public override void OnEntityTransformChanged()
        {
            _boundsDirty = true;
        }
    }
}

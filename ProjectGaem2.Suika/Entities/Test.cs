using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Entities;
using ProjectGaem2.Suika.Components;

namespace ProjectGaem2.Suika.Entities
{
    public class Test : Entity
    {
        public Test(Texture2D texture)
            : base()
        {
            AddComponent(new Dropper(texture));
            Transform.Position = new Vector2(50, 50);
        }
    }
}

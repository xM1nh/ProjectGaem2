using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Input.Virtual
{
    public abstract class VirtualInput
    {
        protected VirtualInput()
        {
            InputListener.VirtualInputs.Add(this);
        }

        public void Unregister()
        {
            InputListener.VirtualInputs.Remove(this);
        }

        public abstract void Update(GameTime gameTime);
    }

    public abstract class VirtualInputNode
    {
        public virtual void Update() { }
    }
}

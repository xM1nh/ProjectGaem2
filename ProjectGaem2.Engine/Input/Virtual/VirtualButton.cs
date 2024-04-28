using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectGaem2.Engine.Input.Virtual
{
    public class VirtualButton : VirtualInput
    {
        public List<Node> Nodes = [];

        public bool Pressed
        {
            get
            {
                foreach (var node in Nodes)
                {
                    if (node.Pressed)
                        return true;
                }
                return false;
            }
        }

        public bool Released
        {
            get
            {
                foreach (var node in Nodes)
                {
                    if (node.Released)
                        return true;
                }
                return false;
            }
        }

        public bool Held
        {
            get
            {
                foreach (var node in Nodes)
                {
                    if (node.Held)
                        return true;
                }
                return false;
            }
        }

        public bool HeldOnly
        {
            get
            {
                foreach (var node in Nodes)
                {
                    if (node.HeldOnly)
                        return true;
                }
                return false;
            }
        }

        public override void Update(GameTime gameTime) { }

        public VirtualButton AddKeyboardKey(Keys key)
        {
            Nodes.Add(new KeyboardKey(key));
            return this;
        }

        public VirtualButton AddKeyboardModifiedKey(Keys key, Keys modifier)
        {
            Nodes.Add(new KeyboardModifiedKey(key, modifier));
            return this;
        }

        public VirtualButton AddLeftMouseButton()
        {
            Nodes.Add(new MouseLeftButton());
            return this;
        }

        public VirtualButton AddMiddleMouseButton()
        {
            Nodes.Add(new MouseMiddleButton());
            return this;
        }

        public VirtualButton AddRightMouseButton()
        {
            Nodes.Add(new MouseRightButton());
            return this;
        }

        public VirtualButton AddFirstExtendedButtonMouseButton()
        {
            Nodes.Add(new MouseFirstExtendedButtonButton());
            return this;
        }

        public VirtualButton AddSecondExtendedButtonMouseButton()
        {
            Nodes.Add(new MouseSecondExtendedButtonButton());
            return this;
        }
    }

    public abstract class Node : VirtualInputNode
    {
        public abstract bool Pressed { get; }
        public abstract bool Released { get; }
        public abstract bool Held { get; }
        public abstract bool HeldOnly { get; }
    }

    public class KeyboardKey(Keys key) : Node
    {
        public Keys Key { get; } = key;
        public override bool Pressed => InputListener.Keyboard.Pressed(Key);

        public override bool Released => InputListener.Keyboard.Released(Key);

        public override bool Held => InputListener.Keyboard.Held(Key);

        public override bool HeldOnly => InputListener.Keyboard.HeldOnly(Key);
    }

    public class KeyboardModifiedKey(Keys key, Keys modifier) : Node
    {
        public Keys Key { get; } = key;
        public Keys Modifier { get; } = modifier;

        public override bool Pressed =>
            InputListener.Keyboard.Held(Modifier) && InputListener.Keyboard.Pressed(Key);

        public override bool Released => InputListener.Keyboard.Released(Key);

        public override bool Held =>
            InputListener.Keyboard.Held(Modifier) && InputListener.Keyboard.Held(Key);

        public override bool HeldOnly =>
            InputListener.Keyboard.Held(Modifier) && InputListener.Keyboard.HeldOnly(Key);
    }

    public class MouseLeftButton : Node
    {
        public override bool Pressed => InputListener.Mouse.Pressed(MouseButton.LeftButton);

        public override bool Released => InputListener.Mouse.Released(MouseButton.LeftButton);

        public override bool Held => InputListener.Mouse.Held(MouseButton.LeftButton);

        public override bool HeldOnly => InputListener.Mouse.HeldOnly(MouseButton.LeftButton);
    }

    public class MouseMiddleButton : Node
    {
        public override bool Pressed => InputListener.Mouse.Pressed(MouseButton.MiddleButton);

        public override bool Released => InputListener.Mouse.Released(MouseButton.MiddleButton);

        public override bool Held => InputListener.Mouse.Held(MouseButton.MiddleButton);

        public override bool HeldOnly => InputListener.Mouse.HeldOnly(MouseButton.MiddleButton);
    }

    public class MouseRightButton : Node
    {
        public override bool Pressed => InputListener.Mouse.Pressed(MouseButton.RightButton);

        public override bool Released => InputListener.Mouse.Released(MouseButton.RightButton);

        public override bool Held => InputListener.Mouse.Held(MouseButton.RightButton);

        public override bool HeldOnly => InputListener.Mouse.HeldOnly(MouseButton.RightButton);
    }

    public class MouseFirstExtendedButtonButton : Node
    {
        public override bool Pressed => InputListener.Mouse.Pressed(MouseButton.XButton1);

        public override bool Released => InputListener.Mouse.Released(MouseButton.XButton1);

        public override bool Held => InputListener.Mouse.Held(MouseButton.XButton1);

        public override bool HeldOnly => InputListener.Mouse.HeldOnly(MouseButton.XButton1);
    }

    public class MouseSecondExtendedButtonButton : Node
    {
        public override bool Pressed => InputListener.Mouse.Pressed(MouseButton.XButton2);

        public override bool Released => InputListener.Mouse.Released(MouseButton.XButton2);

        public override bool Held => InputListener.Mouse.Held(MouseButton.XButton2);

        public override bool HeldOnly => InputListener.Mouse.HeldOnly(MouseButton.XButton2);
    }
}

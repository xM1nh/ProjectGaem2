using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectGaem2.Engine.Input.Virtual;

namespace ProjectGaem2.Engine.Input
{
    public static class InputListener
    {
        public static KeyboardListener Keyboard { get; } = new();
        public static MouseListener Mouse { get; } = new();
        public static List<VirtualInput> VirtualInputs { get; } = [];

        public static void Update(GameTime gameTime)
        {
            Keyboard.Update();
            Mouse.Update();

            for (int i = 0; i < VirtualInputs.Count; i++)
            {
                VirtualInputs[i].Update(gameTime);
            }
        }
    }

    public class KeyboardListener
    {
        public KeyboardState PreviouseKeyboardState { get; private set; }
        public KeyboardState CurrentKeyboardState { get; private set; }

        public KeyboardListener()
        {
            CurrentKeyboardState = Keyboard.GetState();
        }

        public void Update()
        {
            PreviouseKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public bool Pressed(Keys key) =>
            PreviouseKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);

        public bool Released(Keys key) =>
            PreviouseKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);

        public bool Held(Keys key) => CurrentKeyboardState.IsKeyDown(key);

        public bool HeldOnly(Keys key) =>
            PreviouseKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyDown(key);
    }

    public class MouseListener
    {
        public MouseState PreviouseMouseState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public int ScrollDelta { get; }
        public Vector2 PointerDelta { get; }
        public Dictionary<MouseButton, Func<MouseState, ButtonState>> MouseButtons { get; } =
            new()
            {
                { MouseButton.LeftButton, s => s.LeftButton },
                { MouseButton.MiddleButton, s => s.MiddleButton },
                { MouseButton.RightButton, s => s.RightButton },
                { MouseButton.XButton1, s => s.XButton1 },
                { MouseButton.XButton2, s => s.XButton2 },
            };

        public MouseListener()
        {
            CurrentMouseState = Mouse.GetState();
            ScrollDelta = CurrentMouseState.ScrollWheelValue - PreviouseMouseState.ScrollWheelValue;
            PointerDelta = (CurrentMouseState.Position - PreviouseMouseState.Position).ToVector2();
        }

        public void Update()
        {
            PreviouseMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public bool Pressed(MouseButton button) =>
            MouseButtons[button](PreviouseMouseState) == ButtonState.Released
            && MouseButtons[button](CurrentMouseState) == ButtonState.Pressed;

        public bool Released(MouseButton button) =>
            MouseButtons[button](PreviouseMouseState) == ButtonState.Pressed
            && MouseButtons[button](CurrentMouseState) == ButtonState.Released;

        public bool Held(MouseButton button) =>
            MouseButtons[button](CurrentMouseState) == ButtonState.Pressed;

        public bool HeldOnly(MouseButton button) =>
            MouseButtons[button](PreviouseMouseState) == ButtonState.Pressed
            && MouseButtons[button](CurrentMouseState) == ButtonState.Pressed;

        public bool Scrolled() => ScrollDelta != 0;

        public bool PointerMoved() => PointerDelta != Vector2.Zero;
    }
}

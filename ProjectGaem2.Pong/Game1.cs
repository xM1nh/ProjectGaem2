using ProjectGaem2.Engine;
using ProjectGaem2.Pong.Scenes;

namespace ProjectGaem2.Pong
{
    public class Game1 : Core
    {
        protected override void LoadContent()
        {
            base.LoadContent();

            _sceneManager.Add("main", new MainScene(Content));
        }
    }
}

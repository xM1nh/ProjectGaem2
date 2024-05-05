using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.ECS
{
    public class SceneManager
    {
        readonly Dictionary<string, Scene> _scenes = [];
        Scene _activeScene;
        Scene _nextScene;

        public void Add(string sceneName, Scene scene)
        {
            if (_scenes.Count == 0)
            {
                _activeScene = scene;
            }

            _scenes.Add(sceneName, scene);
        }

        public void SwitchScene(string sceneName)
        {
            if (_activeScene is null)
            {
                _activeScene = _scenes[sceneName];
            }
            else
            {
                _nextScene = _scenes[sceneName];
            }
        }

        public void Update()
        {
            if (_nextScene is not null)
            {
                _activeScene = _nextScene;
                _nextScene = null;
            }
            _activeScene.Update();
        }

        public void FixedUpdate()
        {
            if (_nextScene is not null)
            {
                _activeScene = _nextScene;
                _nextScene = null;
            }
            _activeScene.FixedUpdate();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeScene.Draw(spriteBatch);
        }
    }
}

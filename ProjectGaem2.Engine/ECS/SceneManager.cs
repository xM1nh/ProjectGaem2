using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.Graphics;

namespace ProjectGaem2.Engine.ECS
{
    public class SceneManager
    {
        readonly Dictionary<string, Scene> _scenes = [];
        Scene _activeScene;
        Scene _nextScene;

        public void Add(string sceneName, Scene scene)
        {
            _scenes.Add(sceneName, scene);
            if (_scenes.Count == 1)
            {
                SwitchScene(sceneName);
            }
        }

        public void SwitchScene(string sceneName)
        {
            if (_activeScene is null)
            {
                _activeScene = _scenes[sceneName];
                _activeScene.Initialize();
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
                _activeScene.Initialize();
            }
            _activeScene.Update();
        }

        public void FixedUpdate()
        {
            if (_nextScene is not null)
            {
                _activeScene = _nextScene;
                _nextScene = null;
                _activeScene.Initialize();
            }
            _activeScene.FixedUpdate();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeScene.Draw(spriteBatch);
        }

        public void DebugDraw(PrimitiveBatch primitiveBatch)
        {
            _activeScene.DebugDraw(primitiveBatch);
        }
    }
}

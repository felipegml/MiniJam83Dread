using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Common.CommonData;

namespace Manager.Setup
{
    public class ViewManager : ManagerBase<ViewManager>
    {
        #region Singleton
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() => Get();
        #endregion

        #region VARIABLES

        //Private
        private SceneName currentScene;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            OpenMainMenuScene();
        }

        #endregion

        #region FUNCTIONS


        private void OpenScene(SceneName _scene)
        {
            currentScene = _scene;
            SceneManager.LoadScene(currentScene.ToString());
        }

        public void OpenMainMenuScene() => OpenScene(SceneName.MainMenuScene);

        public void OpenGameScene() => OpenScene(SceneName.GameScene);

        #endregion
    }
}
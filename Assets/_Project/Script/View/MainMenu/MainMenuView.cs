using Manager.Setup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        #region VARIABLES


        #endregion

        #region UNITY EVENTS

        void Update()
        {
            if (Input.GetKey(KeyCode.KeypadEnter))
                StartGame_Click();
        }

        #endregion

        #region FUNCTIONS

        public void StartGame_Click()
        {
            ViewManager.Get().OpenGameScene();
        }

        #endregion
    }
}
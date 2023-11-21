using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;

namespace Persona.Player
{
    public class MonsterControl : MonoBehaviour
    {

        #region FUNCTIONS

        void Start()
        {
            //Add Events
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
        }

        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            StartCoroutine(ShowTime());
        }

        private IEnumerator ShowTime()
        {
            yield return new WaitForSeconds(5.4f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        #endregion
    }
}
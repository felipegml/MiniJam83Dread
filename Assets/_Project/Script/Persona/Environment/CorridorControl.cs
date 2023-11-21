using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;
using static Common.CommonData;

namespace Environment
{
    public class CorridorControl : MonoBehaviour
    {
        #region VARIABLES

        [Header("Background")]
        public SpriteRenderer background;
        public GameObject blackout;
        public GameObject parallaxWall;

        [Header("Data")]
        public Color endColor;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //ChangeColor(endColor);
            ShowHideBlackout(false);

            //Add Events
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(GameTags.Player.ToString()))
            {
                if (parallaxWall != null)
                    parallaxWall.SetActive(true);
            }
        }

        #endregion

        #region EVENTS

        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            StartCoroutine(Endgame_Fail());
        }

        #endregion

        #region FUNCTIONS

        public void ChangeColor(Color _color)
        {
            background.color = _color;
        }

        public void ShowHideBlackout(bool _show)
        {
            blackout.SetActive(_show);
        }

        #endregion

        #region GAME STATE FUNCTIONS

        private IEnumerator Endgame_Fail()
        {
            //First blink
            yield return new WaitForSeconds(2f);
            ShowHideBlackout(true);
            yield return new WaitForSeconds(.5f);
            ShowHideBlackout(false);

            //Second blink
            yield return new WaitForSeconds(1f);
            ShowHideBlackout(true);
            yield return new WaitForSeconds(.5f);
            ChangeColor(endColor);
            ShowHideBlackout(false);

            //Second blink
            yield return new WaitForSeconds(1f);
            ShowHideBlackout(true);
            yield return new WaitForSeconds(.5f);
            ShowHideBlackout(false);

            yield return new WaitForSeconds(1.5f);
            //ShowHideBlackout(true);
        }

        #endregion
    }
}
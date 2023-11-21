using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;

namespace Components.Environment
{
    public class CorridorLight : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public SpriteRenderer sprite;
        public Vector2 blinkGap;
        public Vector2 blinkCount;

        [Header("Data")]
        public Color endColor;

        #endregion

        #region FUNCTONS

        void Start()
        {
            StartCoroutine(StartBlink());

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
            StartCoroutine(TurnOff());
        }

        private IEnumerator StartBlink()
        {
            float _delay = Random.Range(blinkGap.x, blinkGap.y);
            yield return new WaitForSeconds(_delay);

            float _blinks = Random.Range(blinkCount.x, blinkCount.y);

            for(int i = 0; i < _blinks; i++)
            {
                sprite.DOFade(0, .1f);
                yield return new WaitForSeconds(.1f);
                sprite.DOFade(1, .1f);
            }

            StartCoroutine(StartBlink());
        }

        public IEnumerator TurnOff()
        {
            yield return new WaitForSeconds(4f);
            gameObject.SetActive(false);
        }

        #endregion
    }
}
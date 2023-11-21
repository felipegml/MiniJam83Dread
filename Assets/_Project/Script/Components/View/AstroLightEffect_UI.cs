using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.Components
{
    public class AstroLightEffect_UI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public Image light;
        public Vector2 alphaGap = new Vector2();
        public Vector2 timeGap = new Vector2();

        #endregion

        #region FUNCTIONS

        void Start()
        {
            BlinkEffect();
        }

        private void BlinkEffect()
        {
            float _time = Random.Range(timeGap.x, timeGap.y);
            float _fade = Random.Range(alphaGap.x, alphaGap.y);

            light.DOFade(_fade, _time)
                 .SetEase(Ease.InOutBounce)
                 .OnComplete(() =>
                 {
                     BlinkEffect();
                 });
        }

        #endregion
    }
}
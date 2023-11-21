using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace View.Components
{
    public class BackgroundParallax_UI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public float time = 1;
        public float endValue = 0;

        //Private
        private float originalPosX;

        #endregion

        #region FUNCTIONS

        void Start()
        {
            originalPosX = transform.localPosition.x;
            BackgroundParallaxMove(true);
        }

        private void BackgroundParallaxMove(bool _right)
        {
            float _moveX = _right ? endValue : originalPosX;

            transform.DOLocalMoveX(_moveX, time)
                     .SetEase(Ease.Linear)
                     .OnComplete(() =>
                     {
                     //End
                     //transform.localPosition = new Vector3(originalPosX, transform.localPosition.y, 0);
                     BackgroundParallaxMove(!_right);
                     });
        }

        #endregion
    }
}
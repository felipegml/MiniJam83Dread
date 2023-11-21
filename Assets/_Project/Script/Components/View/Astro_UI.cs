using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.Components
{
    public class Astro_UI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public Vector2 moveXGap = new Vector2();
        public Vector2 moveYGap = new Vector2();

        #endregion

        #region FUNCTIONS

        void Start()
        {
            DoAstroLoopMove(true);
        }

        private void DoAstroLoopMove(bool _right)
        {
            float _moveX = _right ? Random.Range(moveXGap.x, 0) : Random.Range(0, moveXGap.y);
            float _moveY = Random.Range(moveYGap.x, moveYGap.y);
            transform.DOLocalMoveY(_moveY, 2f);
            transform.DOLocalMoveX(_moveX, 2f)
                     .SetEase(Ease.OutSine)
                     .OnComplete(() =>
                     {
                         DoAstroLoopMove(!_right);
                     });
        }

        #endregion
    }
}
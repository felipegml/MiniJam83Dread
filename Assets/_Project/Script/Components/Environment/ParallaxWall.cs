using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Environment
{
    public class ParallaxWall : MonoBehaviour
    {
        #region VARIABLES

        [Header("Data")]
        public Vector2 distance;
        public float time;

        #endregion

        #region FUNCTIONS

        // Start is called before the first frame update
        void Start()
        {
            transform.localPosition = new Vector3(distance.y, 0 , 0);
            transform.DOLocalMoveX(distance.x, time);
        }

        #endregion
    }
}
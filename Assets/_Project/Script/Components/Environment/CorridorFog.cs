using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Environment
{
    public class CorridorFog : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public float speed;

        #endregion

        #region FUNCTIONS

        void Update()
        {
            transform.localPosition = new Vector3(transform.localPosition.x + speed, 0 , 0);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;

namespace Components.Environment
{
    public class CorridorSpinLight : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public float speed;

        [Header("Data")]
        public Color endColor;

        #endregion

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
            StartCoroutine(ChangeColor());
        }

        void Update()
        {
            transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + speed);
        }

        public IEnumerator ChangeColor()
        {
            yield return new WaitForSeconds(4f);
            gameObject.GetComponent<SpriteRenderer>().color = endColor;
        }

        #endregion
    }
}
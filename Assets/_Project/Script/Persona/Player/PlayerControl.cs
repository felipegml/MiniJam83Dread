using DG.Tweening;
using Environment;
using Persona.Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;
using static Common.CommonData;

namespace Persona.Player
{
    public class PlayerControl : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public Rigidbody2D body;
        public Animator animator;
        public GameObject shadow;
        public AudioSource stepSound;

        [Header("Data")]
        public float speed;

        //Private
        private bool lastDoor = false;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //Add Events
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
            DoorControl.EnterDoor_Event += DoorControl_EnterDoor_Event;
            DoorControl.UnlockDoor_Event += DoorControl_UnlockDoor_Event;

            //Setup
            lastDoor = false;
            StartCoroutine(StartMove());
        }

        void OnDestroy()
        {
            //Remove Events
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
            DoorControl.EnterDoor_Event -= DoorControl_EnterDoor_Event;
            DoorControl.UnlockDoor_Event -= DoorControl_UnlockDoor_Event;
        }

        #endregion

        #region EVENTS

        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            StartCoroutine(StartDeath());
        }

        private void DoorControl_EnterDoor_Event(object[] obj = null)
        {
            lastDoor = (bool)obj[3];
            StopMove();
        }

        private void DoorControl_UnlockDoor_Event(object[] obj = null)
        {
            if (!lastDoor)
                StartCoroutine(StartMove());
            else
                StartCoroutine(StartEscape());
        }

        #endregion

        #region FUNCTIONS

        private IEnumerator StartMove()
        {
            animator.SetInteger(PlayerValues.PLAYER_ANIMATOR_TRIGGER, PlayerValues.RUN);
            stepSound.Play();
            yield return new WaitForSeconds(0.1f);
            body.velocity = new Vector2(speed, 0f);
        }

        private void StopMove()
        {
            body.velocity = Vector2.zero;
            animator.SetInteger(PlayerValues.PLAYER_ANIMATOR_TRIGGER, PlayerValues.IDLE);
            stepSound.Stop();
        }

        #endregion

        #region STATE ANIMATIONS

        public IEnumerator StartDeath()
        {
            yield return new WaitForSeconds(2.5f);
            transform.localScale = new Vector3(-1, 1, 1);
            yield return new WaitForSeconds(2.5f);
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(transform.localPosition .x, -2.55f, 0);
            animator.SetInteger(PlayerValues.PLAYER_ANIMATOR_TRIGGER, PlayerValues.DEAD);
        }

        public IEnumerator StartEscape()
        {
            yield return new WaitForSeconds(2f);
            shadow.SetActive(false);
            animator.SetInteger(PlayerValues.PLAYER_ANIMATOR_TRIGGER, PlayerValues.ESCAPE);
            transform.DOMoveY(0, 1f);
            transform.DOScale(new Vector3(.2f, .2f, .2f), 10f);
        }

        #endregion
    }
}
using DG.Tweening;
using Persona.Puzzle;
using Persona.Puzzle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.GameView;
using static Common.CommonData;

namespace Environment
{
    public class DoorControl : MonoBehaviour
    {
        #region VARIABLES

        [Header("Data")]
        public bool firstDoor = false;
        public bool lastDoor = false;
        public Color endColor;

        [Header("Setup")]
        public GameObject wall;
        public GameObject doorTop;
        public GameObject doorBottom;
        public float doorMoveY = 0;
        public AudioSource audio;

        //Private
        private bool isTrigged = false;
        private int index;
        private PuzzleData_SO data;

        private bool locked = false;

        //Event
        public static event CustomEvent EnterDoor_Event;
        public static event CustomEvent UnlockDoor_Event;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //Add Event
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
            PuzzleControl.Unlock_Event += PuzzleBase_Unlock_Event;
        }

        void OnDestroy()
        {
            //Remove Event
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
            PuzzleControl.Unlock_Event -= PuzzleBase_Unlock_Event;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GameTags.Player.ToString()) && !isTrigged)
                EnterDoor();
        }

        #endregion

        #region EVENTS

        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            locked = true;
            StartCoroutine(ChangeColor());
        }

        private void PuzzleBase_Unlock_Event(object[] obj = null)
        {
            if(index == (int)obj[0])
            {
                OpenDoor();
                //UnlockDoor_Event?.Invoke(new object[] { index, firstDoor, lastDoor });
            }  
        }

        #endregion

        #region SETUP

        public void Setup(int _index, PuzzleData_SO _data, bool _firstDoor, bool _lastDoor)
        {
            index = _index;
            data = _data;
            firstDoor = _firstDoor;
            lastDoor = _lastDoor;
        }

        private void EnterDoor()
        {
            print("EntreDoor");
            isTrigged = true;
            EnterDoor_Event?.Invoke(new object[] { index, data, firstDoor, lastDoor, locked });
        }

        private IEnumerator ChangeColor()
        {
            yield return new WaitForSeconds(4f);
            wall.GetComponent<SpriteRenderer>().color = endColor;
            doorTop.GetComponent<SpriteRenderer>().color = endColor;
            doorBottom.GetComponent<SpriteRenderer>().color = endColor;
        }

        #endregion

        #region ANIMATION SETUP

        public void OpenDoor()
        {
            doorTop.transform.DOLocalMoveY(doorTop.transform.localPosition.y + doorMoveY, PlayerValues.DOOR_OPEN_TIME).SetDelay(1f);
            doorBottom.transform.DOLocalMoveY(doorBottom.transform.localPosition.y - doorMoveY, PlayerValues.DOOR_OPEN_TIME)
                      .SetDelay(1f)
                      .OnComplete(() =>
                      {
                          UnlockDoor_Event?.Invoke(new object[] { index, firstDoor, lastDoor });
                      });

            StartCoroutine(PlaySound());
        }

        public IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(1f);

            if(audio != null)
                audio.Play();
        }

        #endregion
    }
}
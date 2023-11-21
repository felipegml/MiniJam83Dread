using DG.Tweening;
using Environment;
using Persona.Puzzle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.GameView;
using static Common.CommonData;

namespace Persona.Puzzle
{
    public class DiceController : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public GameObject vCam;
        public Transform diceContainer;

        [Header("SFX")]
        public AudioSource rotateDiceSFX;
        public AudioSource selectDice;

        [Header("REF")]
        public Vector3 HIDE_POS;
        public Vector3 SHOW_POS;
        public List<DiceLayout> diceLayoutList = new List<DiceLayout>();

        //Private
        private bool enable = false;
        private PuzzleData_SO data;
        private int currentDice = 0;
        private List<GameObject> diceList = new List<GameObject>();

        //Event
        public static event CustomEvent SelectDice_Event;

        #endregion

        #region CLASSES

        [Serializable]
        public class DiceLayout 
        {
            public List<Vector3> dicePos = new List<Vector3>();
        }

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //Add Events
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
            PuzzleControl.SetupComplete_Event += PuzzleControl_SetupComplete_Event;
            PuzzleControl.Unlock_Event += PuzzleBase_Unlock_Event;
            PuzzleControl.PreUnlock_Event += PuzzleControl_PreUnlock_Event;
            DiceObject.SelectDice_Event += DiceObject_SelectDice_Event;
            DiceObject.RotateDice_Event += DiceObject_RotateDice_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
            PuzzleControl.SetupComplete_Event -= PuzzleControl_SetupComplete_Event;
            PuzzleControl.Unlock_Event -= PuzzleBase_Unlock_Event;
            PuzzleControl.PreUnlock_Event -= PuzzleControl_PreUnlock_Event;
            DiceObject.SelectDice_Event -= DiceObject_SelectDice_Event;
            DiceObject.RotateDice_Event -= DiceObject_RotateDice_Event;
        }

        void Update()
        {
            if(enable)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    SendDiceCommand(DiceDirection.UP);
                if (Input.GetKeyDown(KeyCode.S))
                    SendDiceCommand(DiceDirection.DOWN);
                if (Input.GetKeyDown(KeyCode.A))
                    SendDiceCommand(DiceDirection.LEFT);
                if (Input.GetKeyDown(KeyCode.D))
                    SendDiceCommand(DiceDirection.RIGHT);
                if (Input.GetKeyDown(KeyCode.Q))
                    SendDiceCommand(DiceDirection.CLOCKWISE);
                if (Input.GetKeyDown(KeyCode.E))
                    SendDiceCommand(DiceDirection.COUNTER_CLOCKWISE);

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SelectDice(-1);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    SelectDice(1);
            }

            UpdateControllerPos();
        }

        #endregion

        #region EVENTS

        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            ShowHideDice(false);
        }

        private void PuzzleControl_SetupComplete_Event(object[] obj = null)
        {
            Setup((PuzzleData_SO)obj[0]);
        }

        private void PuzzleBase_Unlock_Event(object[] obj = null)
        {
            ShowHideDice(false);
        }

        private void PuzzleControl_PreUnlock_Event(object[] obj = null)
        {
            enable = false;
        }

        private void DiceObject_SelectDice_Event(object[] obj = null)
        {
            currentDice = (int)obj[0];
            PlaySelectDice();
            SelectDice_Event?.Invoke(new object[] { currentDice });
        }

        private void DiceObject_RotateDice_Event(object[] obj = null)
        {
            PlayRotateDice();
        }

        #endregion

        #region SETUP

        public void Setup(PuzzleData_SO _data)
        {
            data = _data;
            StartCoroutine(AddDices());
        }

        private void UpdateControllerPos()
        {
            transform.position = new Vector3(vCam.transform.position.x,
                                             vCam.transform.position.y,
                                             transform.position.z);
        }

        private IEnumerator AddDices()
        {
            currentDice = 0;
            diceList.Clear();
            foreach (Transform child in diceContainer)
                GameObject.Destroy(child.gameObject);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            DiceLayout _layout = diceLayoutList[data.diceSequence.Count - 1];

            for (int i = 0; i < data.diceSequence.Count; i++)
            {
                GameObject _dice = Instantiate<GameObject>(data.diceSequence[i], diceContainer);
                _dice.transform.localPosition = _layout.dicePos[i];
                _dice.name = i.ToString();
                _dice.GetComponentInChildren<DiceObject>().Setup(i, 
                                                                 data.numberSequence[i],
                                                                 data.directionSequence[i]);
                diceList.Add(_dice);
            }

            diceList.First().GetComponentInChildren<DiceObject>().SelectDice(true);
            ShowHideDice(true);
        }

        private void ShowHideDice(bool _show)
        {
            //UpdateControllerPos();

            if (!_show)
            {
                enable = false;

                for (int i = 0; i < diceList.Count; i++)
                    diceList[i].GetComponentInChildren<DiceObject>().SetEnable(false);

                diceContainer.transform.DOLocalMove(HIDE_POS, PuzzleValues.DICE_SHOW_TIME);
            }
            else
            {
                diceContainer.transform.localPosition = HIDE_POS;
                diceContainer.transform.DOLocalMove(SHOW_POS, PuzzleValues.DICE_SHOW_TIME)
                             .OnComplete(() =>
                             {
                                 enable = true;

                                 for (int i = 0; i < diceList.Count; i++)
                                     diceList[i].GetComponentInChildren<DiceObject>().SetEnable(true);
                             });
            }
        }

        private void SelectDice(int _index)
        {
            currentDice += _index;

            if (currentDice < 0)
                currentDice = diceList.Count - 1;
            else if (currentDice > diceList.Count - 1)
                currentDice = 0;

            PlaySelectDice();

            SelectDice_Event?.Invoke(new object[] { currentDice });
        }

        private void SendDiceCommand(DiceDirection _direction)
        {
            diceList[currentDice].GetComponentInChildren<DiceObject>().RotateDice(_direction);
        }

        #endregion

        #region SFX FUNCTIONS

        public void PlayRotateDice()
        {
            if (enable)
            {
                if (rotateDiceSFX != null && rotateDiceSFX.clip != null)
                    rotateDiceSFX.Play();
            }
        }

        public void PlaySelectDice()
        {
            if (enable)
            {
                if (selectDice != null && selectDice.clip != null)
                    selectDice.Play();
            }
        }

        #endregion
    }
}
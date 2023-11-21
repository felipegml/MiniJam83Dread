using Cinemachine;
using DG.Tweening;
using Environment;
using Manager.Setup;
using Persona.Puzzle;
using Persona.Puzzle.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Common.CommonData;

namespace View.GameView
{
    public class GameMenuView : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public CinemachineVirtualCamera vCam;
        public GameObject firstDoorSound;
        public GameObject timeoutSound;
        public GameObject breathSound;
        public GameObject breathSound2;
        public GameObject monsterWhisper;
        public GameObject endgameSound;
        public GameObject deathSound;

        [Header("UI")]
        public GameObject timerContainer;
        public TextMeshProUGUI timerTxt;
        public GameObject pausePanel;
        public TextMeshProUGUI messageTxt;
        public GameObject warningMessage;
        public GameObject breachMessage;

        [Header("Panel")]
        public Image blackout;
        public GameObject gameoverUI;
        public GameObject escapedUI;

        //Private
        private bool started = false;
        private float currentTime = 0;

        //Event
        public static event CustomEvent Timeout_Event;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            Time.timeScale = 1;
            currentTime = PlayerValues.GAME_TIME;
            UpdateTimer();

            StartCoroutine(StartMessages());

            //Add Events
            DoorControl.EnterDoor_Event += DoorControl_EnterDoor_Event;
            PuzzleControl.Unlock_Event += PuzzleControl_Unlock_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            DoorControl.EnterDoor_Event -= DoorControl_EnterDoor_Event;
            PuzzleControl.Unlock_Event -= PuzzleControl_Unlock_Event;
        }

        void Update()
        {
            if(started)
            {
                if (Input.GetKeyDown(KeyCode.O))
                    currentTime = 0;

                currentTime -= Time.deltaTime;
                if (currentTime < 0)
                    currentTime = 0;

                UpdateTimer();
                VerifyTimeout();
            }
        }

        private void DoorControl_EnterDoor_Event(object[] obj = null)
        {
            PuzzleData_SO _data = (PuzzleData_SO)obj[1];
            currentTime = _data.time;

            //if(!(bool)obj[2])
                ShowHideTimer(true);
        }

        private void PuzzleControl_Unlock_Event(object[] obj = null)
        {
            ShowHideTimer(false);
            StartCoroutine(ShowMessage((string)obj[1]));

            if (((PuzzleData_SO)obj[2]).first)
                firstDoorSound.SetActive(true);

            if (((PuzzleData_SO)obj[2]).last)
                StartCoroutine(StartEscape());
        }

        public void PauseButton_Click()
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }

        public void UnPauseButton_Click()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

        public void ExitButton_Click()
        {
            //UnPauseButton_Click();
            ViewManager.Get().OpenMainMenuScene();
        }

        #endregion

        #region TIMER FUNCTIONS

        public void ShowHideTimer(bool _show)
        {
            started = _show;
            timerContainer.SetActive(_show);
        }

        public void UpdateTimer()
        {
            timerTxt.text = currentTime.ToString("F3");
        }

        public void VerifyTimeout()
        {
            if(currentTime <= 0)
            {
                started = false;
                timeoutSound.SetActive(true);
                ShowHideTimer(false);
                StartCoroutine(StartDeath());

                Timeout_Event?.Invoke();
            }
        }

        #endregion

        #region MESSAGE FUNCTION

        public IEnumerator StartMessages()
        {
            //UpdateMessage(MessageValues.BEGIN_MESSAGE_01);
            yield return new WaitForSeconds(1f);
            warningMessage.SetActive(true);
            yield return new WaitForSeconds(4f);
            breachMessage.SetActive(true);
            //UpdateMessage(MessageValues.BEGIN_MESSAGE_02);

            yield return new WaitForSeconds(2f);
            warningMessage.SetActive(false);
            breachMessage.SetActive(false);
        }

        public IEnumerator ShowMessage(string _message)
        {
            yield return new WaitForSeconds(2f);
            UpdateMessage(_message);
        }

        public void UpdateMessage(string _message)
        {
            messageTxt.DOFade(0, 0);
            messageTxt.text = _message;
            messageTxt.DOFade(1, 1);
            messageTxt.DOFade(0, 1).SetDelay(MessageValues.MESSAGE_TIME);
        }

        #endregion

        #region CAMERA CONTROL

        public IEnumerator StartDeath()
        {
            deathSound.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            breathSound.SetActive(false);
            breathSound2.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            vCam.m_Follow = null;
            DOTween.To(() => vCam.m_Lens.OrthographicSize, x => vCam.m_Lens.OrthographicSize = x, 9, 0);
            monsterWhisper.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            breathSound2.SetActive(false);
            gameoverUI.SetActive(true);
        }

        public IEnumerator StartEscape()
        {
            yield return new WaitForSeconds(2.5f);
            endgameSound.SetActive(true);
            blackout.DOFade(1, 5f).SetDelay(5f);
            DOTween.To(() => breathSound.GetComponent<AudioSource>().volume , x => breathSound.GetComponent<AudioSource>().volume = x, 0, 10f);
            DOTween.To(() => vCam.m_Lens.OrthographicSize, x => vCam.m_Lens.OrthographicSize = x, 3, 15f)
                   .OnComplete(() =>
                   {
                       escapedUI.SetActive(true);
                   });
        }

        #endregion
    }
}
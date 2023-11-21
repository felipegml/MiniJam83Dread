using Persona.Puzzle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View.GameView;
using static Common.CommonData;

namespace Persona.Puzzle
{
    public class PuzzleControl : MonoBehaviour
    {
        #region VARIABLES

        [Header("UI")]
        public GameObject buttonContainer;
        public Transform dicePlayerContainer;
        public Transform dicePuzzleContainer;
        public Image projectionImagePrefab;
        public List<Sprite> diceProjection = new List<Sprite>();

        [Header("Warning")]
        public GameObject puzzleUI;
        public GameObject accessgranted;
        public GameObject accessDenied;

        //Private
        private int index;
        private PuzzleData_SO data;
        private List<Image> playerProjection = new List<Image>();
        private Dictionary<int, int> combinationNumber = new Dictionary<int, int>();
        private Dictionary<int, DiceDirection> combinationDirection = new Dictionary<int, DiceDirection>();

        //Event
        public static event CustomEvent SetupComplete_Event;
        public static event CustomEvent PreUnlock_Event;
        public static event CustomEvent Unlock_Event;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //Add Events
            GameMenuView.Timeout_Event += GameMenuView_Timeout_Event;
            DiceObject.RotateDice_Event += DiceObject_RotateDice_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            GameMenuView.Timeout_Event -= GameMenuView_Timeout_Event;
            DiceObject.RotateDice_Event -= DiceObject_RotateDice_Event;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                StartCoroutine(UnlockDebug_Click());
        }

        //Events
        private void GameMenuView_Timeout_Event(object[] obj = null)
        {
            TimeoutFail();
        }

        private void DiceObject_RotateDice_Event(object[] obj = null)
        {
            UpdatePlayerProjection((int)obj[0], (int)obj[1], (DiceDirection)obj[2]);
        }

        #endregion

        #region FUNCTIONS

        public void Setup(int _index, PuzzleData_SO _data)
        {
            index = _index;
            data = _data;
            SetupPuzzleProjection();
            CreatePlayerProjection();

            SetupComplete_Event?.Invoke(new object[] { data });
        }

        private void SetupPuzzleProjection()
        {
            foreach (Transform child in dicePuzzleContainer)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < data.numberSequence.Count; i++)
            {
                Image _img = Instantiate<Image>(projectionImagePrefab, dicePuzzleContainer);
                _img.sprite = diceProjection[data.numberSequence[i] - 1];
                float _angle = PuzzleValues.PROJECTION_ROTATION[data.directionSequence[i]];
                _img.transform.localRotation = Quaternion.Euler(0, 0, _angle);
            }
        }

        private void CreatePlayerProjection()
        {
            playerProjection.Clear();
            combinationNumber.Clear();
            combinationDirection.Clear();

            foreach (Transform child in dicePlayerContainer)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < data.numberSequence.Count; i++)
            {
                Image _img = Instantiate<Image>(projectionImagePrefab, dicePlayerContainer);
                playerProjection.Add(_img);
            }
        }

        private void UpdatePlayerProjection(int _index, int _number, DiceDirection _direction)
        {
            playerProjection[_index].sprite = diceProjection[_number - 1];
            float _angle = PuzzleValues.PROJECTION_ROTATION[_direction];
            playerProjection[_index].transform.localRotation = Quaternion.Euler(0, 0, _angle);

            VerifyCombination(_index, _number, _direction);
        }

        private void VerifyCombination(int _index, int _number, DiceDirection _direction)
        {
            //Update values
            if (!combinationNumber.ContainsKey(_index) && !combinationDirection.ContainsKey(_index))
            {
                combinationNumber.Add(_index, _number);
                combinationDirection.Add(_index, _direction);
            }
            else
            {
                combinationNumber[_index] = _number;
                combinationDirection[_index] = _direction;
            }

            //Verify
            for (int i = 0; i < data.numberSequence.Count; i++)
            {
                List<int> _numbers = new List<int>(combinationNumber.Values);
                List<DiceDirection> _directions = new List<DiceDirection>(combinationDirection.Values);

                if (_numbers.Contains(data.numberSequence[i]))
                {
                    int _temp = _numbers.IndexOf(data.numberSequence[i]);

                    if(combinationDirection[_temp] != data.directionSequence[i])
                    {
                        print("<color=red>WRONG!</color>");
                        //buttonContainer.SetActive(false);
                        return;
                    }
                }
                else
                {
                    print("<color=red>WRONG!</color>");
                    //buttonContainer.SetActive(false);
                    return;
                }

            }

            print("<color=green>RIGHT!</color>");
            //buttonContainer.SetActive(true);
            StartCoroutine(UnlockDebug_Click());
        }

        public IEnumerator UnlockDebug_Click()
        {
            PreUnlock_Event?.Invoke();
            yield return new WaitForSeconds(.5f);
            accessgranted.SetActive(true);
            puzzleUI.SetActive(false);
            Unlock_Event?.Invoke(new object[] { index, data.message, data });
        }

        public void TimeoutFail()
        {
            accessDenied.SetActive(true);
            puzzleUI.SetActive(false);
        }

        #endregion
    }
}
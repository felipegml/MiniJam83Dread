using Environment;
using Persona.Puzzle;
using Persona.Puzzle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.GameView
{
    public class PuzzleView : MonoBehaviour
    {
        #region VARIABLES

        [Header("UI")]
        public GameObject puzzleContainer;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            StartCoroutine(ShowHidePuzzle(false, 0));

            //Add Events
            DoorControl.EnterDoor_Event += DoorControl_EnterDoor_Event;
            PuzzleControl.Unlock_Event += PuzzleBase_Unlock_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            DoorControl.EnterDoor_Event -= DoorControl_EnterDoor_Event;
            PuzzleControl.Unlock_Event -= PuzzleBase_Unlock_Event;
        }

        #endregion

        #region EVENTS

        private void DoorControl_EnterDoor_Event(object[] obj = null)
        {
            PuzzleData_SO _data = (PuzzleData_SO)obj[1];

            if (_data != null)
                SetupPuzzle((int)obj[0], _data);

            StartCoroutine(ShowHidePuzzle(true, 0));
        }

        private void PuzzleBase_Unlock_Event(object[] obj = null)
        {
            StartCoroutine(ShowHidePuzzle(false, 2f));
        }

        #endregion

        #region FUNCTIONS

        private void SetupPuzzle(int _index, PuzzleData_SO _data)
        {
            foreach (Transform child in puzzleContainer.transform)
                GameObject.Destroy(child.gameObject);

            GameObject _puzzle = Instantiate<GameObject>(_data.puzzlePrefab, puzzleContainer.transform);
            _puzzle.name = _data.name;
            _puzzle.GetComponent<PuzzleControl>().Setup(_index, _data);
        }

        public IEnumerator ShowHidePuzzle(bool _show, float _time)
        {
            yield return new WaitForSeconds(_time);
            puzzleContainer.SetActive(_show);
        }

        #endregion
    }
}
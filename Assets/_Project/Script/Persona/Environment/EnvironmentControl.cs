using Persona.Puzzle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class EnvironmentControl : MonoBehaviour
    {
        #region VARIABLES

        [Header("Data")]
        public List<PuzzleData_SO> puzzleList = new List<PuzzleData_SO>();

        [Header("Corridor Setup")]
        public float corridorStartX = 0;
        public float corridorGap = 0;
        public Transform doorContainer;
        public Transform corridorContainer;
        public List<GameObject> corridorList = new List<GameObject>();

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            SetupCorridor();
            SetupDoors();
        }

        #endregion

        #region FUNCTIONS

        public void SetupCorridor()
        {
            foreach (Transform child in corridorContainer)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < corridorList.Count; i++)
            {
                Vector3 _pos = new Vector3(i * corridorGap, 0, 0);
                GameObject _corridor = Instantiate<GameObject>(corridorList[i],
                                                               _pos,
                                                               Quaternion.identity,
                                                               corridorContainer);
                _corridor.name = i.ToString();
            }
        }

        public void SetupDoors()
        {
            foreach (Transform child in doorContainer)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < puzzleList.Count; i++)
            {
                Vector3 _pos = new Vector3(corridorStartX + i * corridorGap, 0, 0);

                if (puzzleList[i] != null)
                {
                    GameObject _door = Instantiate<GameObject>(puzzleList[i].doorPrefab,
                                                               _pos,
                                                               Quaternion.identity,
                                                               doorContainer);
                    _door.name = i.ToString();
                    _door.GetComponentInChildren<DoorControl>().Setup(i, puzzleList[i], puzzleList[i].first, puzzleList[i].last);
                }
            }
        }

        #endregion
    }
}
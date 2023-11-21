using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Common.CommonData;

namespace Persona.Puzzle.Data
{
    [CreateAssetMenu(fileName = "New Puzzle", menuName = "Puzzle")]
    public class PuzzleData_SO : ScriptableObject
    {
        [Header("Data")]
        public float time;
        public bool first = false;
        public bool last = false;
        [TextArea]
        public string message;
        public PuzzleType puzzleType;
        public GameObject doorPrefab;
        public GameObject puzzlePrefab;
         

        [Header("PROJECTION SETUP")]
        public List<int> numberSequence;
        public List<DiceDirection> directionSequence;
        public List<GameObject> diceSequence;
    }
}
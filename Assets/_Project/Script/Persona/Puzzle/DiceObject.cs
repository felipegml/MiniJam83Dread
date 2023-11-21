using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Common.CommonData;

namespace Persona.Puzzle
{
    public class DiceObject : MonoBehaviour
    {
        #region VARIABLES

        [Header("Setup")]
        public Transform mainContainer;
        public Transform diceContainer;
        public Transform dice;
        public GameObject diceOutline;
        public List<Vector3> sideRotation = new List<Vector3>();
        public List<GameObject> sideTriggers = new List<GameObject>();

        //Private
        private bool isEnable = false;
        private int index;
        private int number = 0;
        private DiceDirection direction;
        private int currentTriggerIndex;

        //Event
        public static event CustomEvent RotateDice_Event;
        public static event CustomEvent SelectDice_Event;

        #endregion

        #region UNITY EVENTS

        void Start()
        {
            //Add Events
            DiceController.SelectDice_Event += DiceController_SelectDice_Event;
        }

        void OnDestroy()
        {
            //Remove Events
            DiceController.SelectDice_Event -= DiceController_SelectDice_Event;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.CubeTrigger.ToString()))
            {
                currentTriggerIndex = int.Parse(other.name);
                VerifySide(currentTriggerIndex);
            }
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
                SelectDice_Click();
        }
        #endregion

        #region EVENTS

        private void DiceController_SelectDice_Event(object[] obj = null)
        {
            SelectDice(index == (int)obj[0]);
        }

        public void SelectDice_Click()
        {
            SelectDice_Event?.Invoke(new object [] { index });
        }

        #endregion

        #region FUNCTIONS

        public void Setup(int _index, int _number, DiceDirection _direction)
        {
            index = _index;
            number = _number;
            direction = _direction;
            SetupPosition();
        }

        public void SelectDice(bool _select)
        {
            diceOutline.SetActive(_select);
        }

        public void SetEnable(bool _enable) => isEnable = _enable;

        private void SetupPosition()
        {
            /*
            List<int> _temp = new List<int>(Values.DICE_NUMBERS);
            _temp.RemoveAt(number-1);

            int _index = Random.Range(0, _temp.Count);
            diceContainer.transform.DOLocalRotate(sideRotation[_temp[_index]-1], 0);
            */

            SendDiceInfo(PuzzleValues.DICE_NUMBERS.First(), DiceDirection.UP);
        }

        public void RotateDice(DiceDirection _direciton)
        {
            if(isEnable)
            {
                isEnable = false;

                diceContainer.transform.DOLocalRotate(PuzzleValues.DIRECTION_ROTATION[_direciton], PuzzleValues.DICE_ROTATION_TIME)
                             .OnComplete(() =>
                             {
                                 dice.SetParent(mainContainer);
                                 diceContainer.transform.localRotation = Quaternion.Euler(0,0,0);
                                 dice.SetParent(diceContainer.transform);

                                 isEnable = true;

                                 if(_direciton == DiceDirection.CLOCKWISE || 
                                    _direciton == DiceDirection.COUNTER_CLOCKWISE)
                                     VerifySide(currentTriggerIndex);
                             });
            }
        }

        public void VerifySide(int _index)
        {
            GameObject _side = sideTriggers[_index - 1];
            Vector2 _normal = new Vector2(Mathf.RoundToInt(_side.transform.up.normalized.x),
                                          Mathf.RoundToInt(_side.transform.up.normalized.y));
            DiceDirection _direction = PuzzleValues.FACE_DIRECTION[_normal];

            //print(string.Format("SIDE: {0} {1}", _index, _direction));
            SendDiceInfo(PuzzleValues.DICE_NUMBERS[_index-1], _direction);
        }

        private void SendDiceInfo(int _number, DiceDirection _direction)
        {
            RotateDice_Event?.Invoke(new object[] { index, _number, _direction });
        }

        #endregion
    }
}
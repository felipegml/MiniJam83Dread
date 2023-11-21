using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class CommonData
    {
        #region EVENTS\TAGS

        //Custon event
        public delegate void CustomEvent(object[] obj = null);

        public enum SceneName
        {
            MainMenuScene,
            GameScene
        }

        public enum GameTags
        {
            Player,
            Door,
            CubeTrigger
        }

        #endregion

        #region VALUES

        public enum PuzzleType
        {
            PROJECTION
        }

        public enum DiceDirection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            CLOCKWISE,
            COUNTER_CLOCKWISE
        }


        public static class PuzzleValues
        {
            public static float DICE_SHOW_TIME = 0;
            public static float DICE_ROTATION_TIME = .2f;
            public static List<int> DICE_NUMBERS = new List<int>() { 1, 2, 3, 4, 5, 6 };

            public static Dictionary<DiceDirection, Vector3> DIRECTION_ROTATION =
                new Dictionary<DiceDirection, Vector3>()
                {
                    { DiceDirection.UP, new Vector3(-90, 0, 0) },
                    { DiceDirection.DOWN, new Vector3(90, 0, 0) },
                    { DiceDirection.LEFT, new Vector3(0, -90, 0) },
                    { DiceDirection.RIGHT, new Vector3(0, 90, 0) },
                    { DiceDirection.CLOCKWISE, new Vector3(0, 0, 90) },
                    { DiceDirection.COUNTER_CLOCKWISE, new Vector3(0, 0, -90) }
                };

            public static Dictionary<Vector2, DiceDirection> FACE_DIRECTION =
                new Dictionary<Vector2, DiceDirection>()
                {
                    { new Vector2(0, 1), DiceDirection.UP  },
                    { new Vector2(0, -1), DiceDirection.DOWN },
                    { new Vector2(1, 0), DiceDirection.RIGHT },
                    { new Vector2(-1, 0), DiceDirection.LEFT }
                };

            public static Dictionary<DiceDirection, float> PROJECTION_ROTATION =
                new Dictionary<DiceDirection, float>()
                {
                    { DiceDirection.UP, 0 },
                    { DiceDirection.DOWN, -180 },
                    { DiceDirection.LEFT, 90 },
                    { DiceDirection.RIGHT, -90 }
                };
        }

        public static class MessageValues
        {
            public static float MESSAGE_TIME = 2f;
            public static string BEGIN_MESSAGE_01 = "Deu merda, fudeu!";
            public static string BEGIN_MESSAGE_02 = "Corre maluco!!!";
        }

        #endregion

        #region PLAYER VALUES

        public static class PlayerValues
        {
            public static string PLAYER_ANIMATOR_TRIGGER = "STATE";
            public static int IDLE = 0;
            public static int RUN = 1;
            public static int DEAD = 2;
            public static int ESCAPE = 3;

            public static float DOOR_OPEN_TIME = 1f;

            public static float GAME_TIME = 30f;
        }

        #endregion
    }
}
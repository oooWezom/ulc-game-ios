using UnityEngine;
using System.Collections;

public class Constants
{
    public static bool IS_ANDROID_INITIATED = false;

    public static int USER_ID = -1;
    public static int SELF_INDEX = -1;

    public static int GAME_ID = 0;

    public static int LEFT_PLAYER_ID = -1;
    public static int LEFT_PLAYER_INDEX = -1;

    public static string BASE_URL; //initing in MessageRouter(), OnMessage() method
}
using UnityEngine;
using System.Collections;
using Assets;

public class SpinTheDisksMessageRouter : MonoBehaviour, IMessageRouter
{
    public SpinTheDisksSceneController spinTheDisksSceneController;
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject gameActivity;

    void Start()
    {
        ObtainActivity();
    }


    void Update()
    {
    }

    void OnDestroy()
    {
        Debug.Log("SpinTheDisksMessageRouter OnDestroy");
        ReleaseActivity();
    }

	/* iOS changes */
    public void SendAndroidMessage(UnityMessage message)
    {
		string s = JsonUtility.ToJson (message); // For ios

//        if (gameActivity != null)
//        {
//            Debug.Log("SpinTheDisksMessageRouter onUnityMessage: " + JsonUtility.ToJson(message));
//            gameActivity.Call("onUnityMessage", JsonUtility.ToJson(message));
//        }
    }

    public void ObtainActivity()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        gameActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Log("SpinTheDisksMessageRouter onUnityReady");
        gameActivity.Call("onUnityReady");
    }

    public void ReleaseActivity()
    {
        gameActivity = null;
        unityPlayer = null;
    }

    public void OnMessage(string message)
    {
        Debug.Log("SpinTheDisksMessageRouter OnMessage: " + message);
        Message messageType = JsonUtility.FromJson<Message>(message);
        if (Constants.IS_ANDROID_INITIATED == false && messageType.type != 0)
            return;
        switch (messageType.type)
        {
            case 1000: // initial game state
                Debug.Log("1000: initial game state");
                SpinTheDiskGameStateMessage spinTheDiskGameStateMessage =
                    JsonUtility.FromJson<SpinTheDiskGameStateMessage>(message);
                spinTheDisksSceneController.InitGame(spinTheDiskGameStateMessage.data);
                Debug.Log("Init from WS: " + message);
                break;
            case 1002:
                Debug.Log("1002: roundStartMessage");
                Debug.Log("RoundStart: UserID = " + Constants.USER_ID);
                Debug.Log("Constants.SELF_INDEX = " + Constants.SELF_INDEX);
                RoundStartMessage<SpinTheDisksPlayerData> roundStartMessage =
                    JsonUtility.FromJson<RoundStartMessage<SpinTheDisksPlayerData>>(message);
                if (roundStartMessage == null)
                {
                    Debug.Log("1002 roundStartMessage == NULL");
                }

                Debug.Log("Round start: " + roundStartMessage);
                spinTheDisksSceneController.RoundStart(roundStartMessage.p1, roundStartMessage.p2, true);
                break;
            case 1003:
                Debug.Log("1003: moveDiskMessage");
                MoveDiskMessage moveDiskMessage = JsonUtility.FromJson<MoveDiskMessage>(message);
                spinTheDisksSceneController.Move(moveDiskMessage.id, moveDiskMessage.disk, moveDiskMessage.angle);
                Debug.Log("Move message: angle" + moveDiskMessage.angle + " disk : " + moveDiskMessage.disk);
                break;
            case 1005: // round result
                Debug.Log("1005: round result");
                spinTheDisksSceneController.InitReadyState(false, false);
                SendAndroidMessage(new UnityMessage(1005, message)); //send callback to activity, notify round end
                break;
        }
    }
}
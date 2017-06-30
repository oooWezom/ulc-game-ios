using UnityEngine;
using System.Collections;
using Assets;

public class RockSpockMessageRouter : MonoBehaviour, IMessageRouter
{
    public RockSpockSceneController sceneController;
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject gameActivity;

    void Start()
    {
        ObtainActivity();
    }
  
    void OnDestroy()
    {
        Debug.Log("RockSpockMessageRouter OnDestroy");
        ReleaseActivity();
    }

	/* iOS changes */
    public void SendAndroidMessage(UnityMessage message)
    {
		string s = JsonUtility.ToJson (message); // For ios

//        if (gameActivity != null)
//        {
//            Debug.Log("RockSpockMessageRouter onUnityMessage: " + JsonUtility.ToJson(message));
//            gameActivity.Call("onUnityMessage", JsonUtility.ToJson(message));
//        }
    }

    public void ObtainActivity()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        gameActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Log("RockSpockMessageRouter onUnityReady");
        gameActivity.Call("onUnityReady");
    }

    public void ReleaseActivity()
    {
        gameActivity = null;
        unityPlayer = null;
    }

    public void OnMessage(string message)
    {
        Debug.Log("RockSpockMessageRouter OnMessage: " + message);
        Message messageType = JsonUtility.FromJson<Message>(message);
        if (Constants.IS_ANDROID_INITIATED == false && messageType.type != 0)
            return;
        switch (messageType.type)
        {
            case 1000: // initial game state
                Debug.Log("1000: initial game state");
                RockSpockGameStateMessage spinTheDiskGameStateMessage =
                    JsonUtility.FromJson<RockSpockGameStateMessage>(message);
                sceneController.InitGame(spinTheDiskGameStateMessage.data);
                break;

            case 1002: //roundStartMessage
                Debug.Log("1002: roundStartMessage");
                sceneController.ShowGame(true);
                break;

            case 1003: //moveMessage
                Debug.Log("1003: moveMessage");
                MoveRockSpockMessage moveDiskMessage = JsonUtility.FromJson<MoveRockSpockMessage>(message);
                sceneController.Move(moveDiskMessage.id, moveDiskMessage.moveId);
                Debug.Log("Move message: moveId " + moveDiskMessage.moveId);
                break;

            case 1005: // round result
                Debug.Log("1005: round result");
                RockSpockRoundResultMessage roundResult = JsonUtility.FromJson<RockSpockRoundResultMessage>(message);
                Debug.Log("1005: moves " + roundResult.moves[0] + " " + roundResult.moves[1]);
                Debug.Log("1005: winner_id " + roundResult.winner_id);
                Debug.Log("1005: final " + roundResult.final);
                Debug.Log("1005: round result");
                sceneController.OnRoundResult(roundResult);
                break;
        }
    }
}
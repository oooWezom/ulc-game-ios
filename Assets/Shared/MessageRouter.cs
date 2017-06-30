using System.Collections;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Shared.Messages;

public class MessageRouter : MonoBehaviour, IMessageRouter
{
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject gameActivity;

    public void OnMessage(string message)
    {
        Debug.Log("MessageRouter OnMessage: " + message);
        Message messageType = JsonUtility.FromJson<Message>(message);
        if (Constants.IS_ANDROID_INITIATED == false && messageType.type != 0)
            return;
        switch (messageType.type)
        {
            case 0: // init from android
                Debug.Log("0 - init from android");
                GameInitMessage gameInitMessage = JsonUtility.FromJson<GameInitMessage>(message);
                Constants.USER_ID = gameInitMessage.user_id;
                Constants.GAME_ID = gameInitMessage.game_id;
                Constants.LEFT_PLAYER_ID = gameInitMessage.left_player_id;
                Constants.IS_ANDROID_INITIATED = true;
                Constants.BASE_URL = gameInitMessage.base_url;
				Debug.Log("BASE URL: " + gameInitMessage.base_url);
                Debug.Log("Init from android: " + gameInitMessage);
                if (gameInitMessage.user_id == -1)
                {
                    // if we are watching
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = true;
                    Debug.Log("We are watching");
                }
                else
                {
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    Screen.autorotateToPortrait = true;
                    Debug.Log("We are playing");
                }
                break;
            case 1: // screen orientation change request
                Debug.Log("1 - screen orientation change request");
                ScreenOrientationMessage orientationMessage = JsonUtility.FromJson<ScreenOrientationMessage>(message);
                switch (orientationMessage.orientation)
                {
                    case 0: // Landscape
                        Screen.orientation = ScreenOrientation.Landscape;
                        Debug.Log("screen orientation Landscape");
                        Screen.autorotateToLandscapeLeft = true;
                        Screen.autorotateToLandscapeRight = true;
                        Screen.autorotateToPortrait = false;
                        break;
                    case 1: // Portrait
                        Screen.orientation = ScreenOrientation.Portrait;
                        Debug.Log("screen orientation Portrait");
                        Screen.autorotateToLandscapeLeft = false;
                        Screen.autorotateToLandscapeRight = false;
                        Screen.autorotateToPortrait = true;
                        Screen.autorotateToPortraitUpsideDown = true;
                        break;
                    case 2: //autorotate
                        Debug.Log("screen orientation autorotation");
                        Screen.orientation = ScreenOrientation.AutoRotation;
                        Screen.autorotateToLandscapeLeft = true;
                        Screen.autorotateToLandscapeRight = true;
                        Screen.autorotateToPortrait = true;
                        Screen.autorotateToPortraitUpsideDown = true;
                        break;
                }
                break;

			case 2: //switch scene
				//SwitchSceneMessage sceneMessage = JsonUtility.FromJson<SwitchSceneMessage>(message);
				//SwitchScene(sceneMessage);
				break;
        }
    }

	/* iOS changes */
	// called from iOS
	private void CleanSceneState() {
		Debug.Log("MessageRouter CleanSceneState: ");
		Constants.LEFT_PLAYER_ID = -1;
		Constants.LEFT_PLAYER_INDEX = -1;
		Constants.USER_ID = -1;
		Constants.SELF_INDEX = -1;
		Constants.GAME_ID = 0;
	}

	/* iOS changes */
	private void SwitchScene(string scene)
	{
		Debug.Log("MessageRouter SwitchScene: " + scene);
		switch (scene)
		{
		case "7": //spin the disks
			SceneManager.LoadScene("SpinTheDisk");
			break;
		case "10": //rock scissors papper
			SceneManager.LoadScene("RockSpock");
			break;
		}
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameActivity != null)
        {
            Debug.Log("MessageRouter onUnityBackPressed");
            gameActivity.Call("onUnityBackPressed");
        }
    }

    void Start()
    {
        Screen.fullScreen = false; // work with plugin to show status bar
        ObtainActivity();
    }

    void OnDestroy()
    {
        Debug.Log("MessageRouter OnDestroy");
        ReleaseActivity();
    }

	/* iOS changes */
    public void SendAndroidMessage(UnityMessage message)
    {
		string s = JsonUtility.ToJson (message); // For ios

//        if (gameActivity != null)
//        {
//            Debug.Log("MessageRouter onUnityMessage: " + JsonUtility.ToJson(message));
//            gameActivity.Call("onUnityMessage", JsonUtility.ToJson(message));
//        }
    }

    public void ObtainActivity()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        gameActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Log("MessageRouter onUnityReady");
        gameActivity.Call("onUnityReady");
    }

    public void ReleaseActivity()
    {
        gameActivity = null;
        unityPlayer = null;
    }
}
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesRouter : MonoBehaviour
{
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject gameActivity;


    void Start()
    {
        Screen.fullScreen = false; // work with plugin to show status bar
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        gameActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Log("ScenesRouter start()");
        Debug.Log("ScenesRouter gameActivity.Call onUnitySceneSwitchReady()");
        gameActivity.Call("onUnityInitialSceneReady");

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    void onSceneSwitch(String sceneId)
    {
        Debug.Log("onSceneSwitch(), scene id - " + sceneId);

        switch (sceneId)
        {
            case "7": //spin the disks
                SceneManager.LoadScene("SpinTheDisk");
                break;
			case "10": //rock scissors papper
				SceneManager.LoadScene ("RockSpock");
                break;
        }
    }

    void OnDestroy()
    {
        Debug.Log("ScenesRouter OnDestroy");
        gameActivity = null;
        unityPlayer = null;
    }
}
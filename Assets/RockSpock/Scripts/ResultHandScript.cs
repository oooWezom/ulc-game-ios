using UnityEngine;
using System.Collections;

public class ResultHandScript : MonoBehaviour
{
    public RockSpockSceneController sceneController;

    public void OnAnimationEnd()
    {
        if (sceneController != null)
        {
            sceneController.OnRoundResultCallback();
        }
    }
}
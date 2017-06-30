using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

   
    public MessageRouter messageRouter;
    public Animator countdownAnimation;

    public void ShowUI() {
        gameObject.SetActive(true);
    }

    public void HideUI() {
        gameObject.SetActive(false);
    }

    public void ShowGame(bool animate)
    {
        Debug.Log("UIController ShowGame");
        if (animate) {
            countdownAnimation.gameObject.SetActive(true);
            countdownAnimation.SetTrigger("Count");
        } else {
            HideUI();
        }
    }

    public void OnReadyClick() {
        if (Constants.SELF_INDEX == -1)
            return;
        messageRouter.SendAndroidMessage(new UnityMessage(0, ""));
    }
}

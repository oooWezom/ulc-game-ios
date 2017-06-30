using UnityEngine;
using System.Collections;

public class CountEvents : MonoBehaviour {

    public UIController uiController;

    public void CountedToThree() {
        uiController.HideUI();
        gameObject.SetActive(false);
    }
}

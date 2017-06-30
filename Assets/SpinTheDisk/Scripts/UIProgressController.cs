using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressController : MonoBehaviour {

    public Text progressText;

    private int currentProgress = 0;

    public void UpdateProgress(int progress) {
        currentProgress = progress;
        progressText.text = progress + "/5";
    }
}

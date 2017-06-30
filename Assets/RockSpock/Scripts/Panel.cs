using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public const int MOVE_ROCK = 1;
    public const int MOVE_SCISSORS = 2;
    public const int MOVE_PAPER = 3;

    public Button rockButton;
    public Button scissorsButton;
    public Button papperButton;

    public RockSpockMessageRouter messageRouter;

    public void Move(int moveId)
    {
        Debug.Log("Panel Move: " + moveId);
        switch (moveId)
        {
            case MOVE_ROCK:
                setButtonTint(papperButton, true);
                setButtonTint(scissorsButton, true);
                setButtonsActive(false, false, false);
                enableChoseAnimation(true, rockButton);
                break;
            case MOVE_SCISSORS:
                setButtonTint(rockButton, true);
                setButtonTint(papperButton, true);
                setButtonsActive(false, false, false);
                enableChoseAnimation(true, scissorsButton);
                break;
            case MOVE_PAPER:
                setButtonTint(rockButton, true);
                setButtonTint(scissorsButton, true);
                setButtonsActive(false, false, false);
                enableChoseAnimation(true, papperButton);
                break;
        }
    }

    public void OnRockClick()
    {
        sendMove(MOVE_ROCK);
        Debug.Log("OnRockClick");
        setButtonTint(papperButton, true);
        setButtonTint(scissorsButton, true);
        setButtonsActive(false, false, false);
        enableChoseAnimation(true, rockButton);
    }

    public void OnPaperClick()
    {
        sendMove(MOVE_PAPER);
        Debug.Log("OnPaperClick");
        setButtonTint(rockButton, true);
        setButtonTint(scissorsButton, true);
        setButtonsActive(false, false, false);
        enableChoseAnimation(true, papperButton);
    }

    public void OnScissorsClick()
    {
        sendMove(MOVE_SCISSORS);
        setButtonTint(rockButton, true);
        setButtonTint(papperButton, true);
        setButtonsActive(false, false, false);
        enableChoseAnimation(true, scissorsButton);
        Debug.Log("OnScissorsClick");
    }

    private void enableChoseAnimation(bool isEnable, Button button)
    {
        Animator animator = button.GetComponent<Animator>();
        animator.SetBool("animate", isEnable);
    }

    private void setButtonsActive(bool rock, bool papper, bool scissors)
    {
        rockButton.interactable = rock;
        papperButton.interactable = papper;
        scissorsButton.interactable = scissors;
    }

    private void setButtonTint(Button button, bool isTinted)
    {
        button.image.color = isTinted ? Color.black : Color.white;
    }

    private void sendMove(int move)
    {
        messageRouter.SendAndroidMessage(new UnityMessage(1, JsonUtility.ToJson(new RockSpockMessage(move))));
    }

    public void ResetState()
    {
        Debug.Log("RockSpockSceneController ResetPanel");
        setButtonsActive(true, true, true);
        setButtonTint(rockButton, false);
        setButtonTint(papperButton, false);
        setButtonTint(scissorsButton, false);
        enableChoseAnimation(false, rockButton);
        enableChoseAnimation(false, papperButton);
        enableChoseAnimation(false, scissorsButton);
    }
}
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RockSpockSceneController : MonoBehaviour
{
    private const String PAPPER_TRIGGER = "papper";
    private const String SCISSORS_TRIGGER = "scissors";
    private const String ROCK_TRIGGER = "rock";

    public RockSpockMessageRouter messageRouter;
    public Camera leftCamera;
    public Camera rightCamera;
    public UIController uiController;

    public Panel leftPanel;
    public Panel rightPanel;

    public GameObject leftText;
    public GameObject rightText;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private RockSpockPlayerData leftPlayer;
    private RockSpockPlayerData rightPlayer;

    private RockSpockRoundResultMessage roundResult;

    private String loseText = "loose";
    private String winText = "win";
    private String drawText = "draw";

    private bool moveCamToCenterAlloved;
    private float cameraSpeed = 9F;

	/* iOS changes */
    public void Start()
    {
        //for testing purpose
        //         PlayAnimation(rightHandAnimator, Panel.MOVE_PAPER);
        //         PlayAnimation(leftHandAnimator, Panel.MOVE_SCISSORS);
        //  moveCamToInitPositionAlloved = true;

//        this.roundResult = new RockSpockRoundResultMessage();
//        roundResult.winner_id = 0;
//        int[] array = { 1,1 };
//        roundResult.moves = array;
//        roundResult.final = false;
//        Constants.USER_ID = 138;
//        OnRoundResultCallback();
    }

    public void Update()
    {
        if (moveCamToCenterAlloved)
        {
            MoveCameraToCenter();
        }
    }


    public void InitGame(RockSpockGameState gameState)
    {
        Debug.Log("RockSpockSceneController.InitGame() p1:" + gameState.players_data[0].id);
        if (Constants.USER_ID != -1)
        {
            // if we are playing
            if (gameState.players_data[0].id == Constants.USER_ID)
            {
                Constants.SELF_INDEX = 0; //i'm p1
            }
            if (gameState.players_data[1].id == Constants.USER_ID)
            {
                Constants.SELF_INDEX = 1; //i'm p2
            }
        }
        else
        {
            if (gameState.players_data[0].id == Constants.LEFT_PLAYER_ID)
            {
                Constants.LEFT_PLAYER_INDEX = 0;
            }
            if (gameState.players_data[1].id == Constants.LEFT_PLAYER_ID)
            {
                Constants.LEFT_PLAYER_INDEX = 1;
            }
        }

        switch (gameState.state)
        {
            case 1: //ready
                if (Constants.SELF_INDEX != -1)
                {
                    // i'm playing
                    int opponentIndex = Constants.SELF_INDEX == 0 ? 1 : 0;
                    // if my index is 0, than opponent is 1, inverse otherwise
                    InitReadyState(gameState.players_data[Constants.SELF_INDEX].ready,
                        gameState.players_data[opponentIndex].ready);
                }
                else
                {
                    int rightPlayerIndex = Constants.LEFT_PLAYER_INDEX == 0 ? 1 : 0;
                    InitReadyState(gameState.players_data[Constants.LEFT_PLAYER_INDEX].ready,
                        gameState.players_data[rightPlayerIndex].ready);
                }
                break;
            case 2: //playing
                RoundStart(gameState.players_data[0], gameState.players_data[1], false);
                break;
            case 3: //end
                break;
        }
    }

    public void InitReadyState(bool leftReady, bool rightReady)
    {
        uiController.ShowUI();
    }

    public void RoundStart(RockSpockPlayerData p1, RockSpockPlayerData p2, bool animate)
    {
        Debug.Log("RockSpockSceneController RoundStart: p1 = " + p1.id);
        Debug.Log("RockSpockSceneController RoundStart: p2 = " + p2.id);
        Debug.Log("RockSpockSceneController RoundStart: UserID = " + Constants.USER_ID);
        Debug.Log("RockSpockSceneController Constants.SELF_INDEX = " + Constants.SELF_INDEX);
        uiController.ShowGame(animate);

        switch (Constants.SELF_INDEX)
        {
            case -1: //not playing, just watching
                if (Constants.LEFT_PLAYER_INDEX == 0)
                {
                    Debug.Log("RockSpockSceneController not playing, just watching");
                    leftPlayer = p1;
                    rightPlayer = p2;
                }
                else
                {
                    leftPlayer = p2;
                    rightPlayer = p1;
                }
                switchToTwoPlayerMode();
                break;
            case 0: //i'm p1
                SwitchToOnePlayerMode();
                leftPlayer = p1;
                rightPlayer = p2;
                break;
            case 1: //i'm p2
                SwitchToOnePlayerMode();
                leftPlayer = p2;
                rightPlayer = p1;
                break;
            default: //magic!
                switchToTwoPlayerMode();
                leftPlayer = p1;
                rightPlayer = p2;
                break;
        }
    }

    public void ShowGame(bool isAnimate)
    {
        switch (Constants.SELF_INDEX)
        {
            case -1: //not playing, just watching

                switchToTwoPlayerMode();
                break;
            case 0: //i'm p1
                SwitchToOnePlayerMode();

                break;
            case 1: //i'm p2
                SwitchToOnePlayerMode();
                break;
            default: //magic!
                switchToTwoPlayerMode();
                break;
        }

        uiController.ShowGame(isAnimate);
    }

    private void SwitchToOnePlayerMode()
    {
        Debug.Log("RockSpockSceneController SwitchToOnePlayerMode");
        rightCamera.enabled = false; //we need to see only our camera
        leftCamera.rect = new Rect(0, 0, 1, 1); // make camera fullscreen
        leftCamera.orthographicSize = 5f;
    }

    private void switchToTwoPlayerMode()
    {
        Debug.Log("RockSpockSceneController switchToTwoPlayerMode");
        rightCamera.enabled = true; // enable second camera

        leftCamera.rect = new Rect(0, 0, 0.5f, 1); // half screen
        rightCamera.rect = new Rect(0.5F, 0, 0.5f, 1); // half screen

        leftCamera.cullingMask = ~(1 << 9); //RightSide layer exclude
        rightCamera.cullingMask = ~(1 << 8); //LeftSide layer exclude

        leftCamera.orthographicSize = 5f;
        rightCamera.orthographicSize = 5f;

        leftCamera.GetComponent<EventSystem>().enabled = false; //disable camera events
    }

    public void Move(int id, int moveId)
    {
        if (Constants.SELF_INDEX != -1)
        {
            if (id == Constants.USER_ID)
            {
                leftPanel.Move(moveId);
            }
            else
            {
                rightPanel.Move(moveId);
            }
        }
        else
        {
            if (id == Constants.LEFT_PLAYER_ID)
            {
                Debug.Log("Left move. id = " + id);
                leftPanel.Move(moveId);
            }
            else
            {
                Debug.Log("Right move. id = " + id);
                rightPanel.Move(moveId);
            }
            Debug.Log("RockSpockSceneController Move. LeftPlayerId: " + leftPlayer.id);
            Debug.Log("RockSpockSceneController Move. RightPlayerId: " + rightPlayer.id);
        }
    }

    public void OnRoundResultCallback() //triggers when animation ends
    {
        Debug.Log("RockSpockSceneController OnRoundResultCallback() winner id:" + roundResult.winner_id);
        Debug.Log("RockSpockSceneController Constants.USER_ID == " + Constants.USER_ID);
        if (Constants.USER_ID != -1)
        {
            Debug.Log("Constants.USER_ID != -1");

            // i'm playing
            //setting text loose-win

            if (Constants.USER_ID == roundResult.winner_id) //left win
            {
                Debug.Log("RockSpockSceneController OnRoundResultCallback() winner LEFT");

                leftText.GetComponent<Text>().text = winText;
                rightText.GetComponent<Text>().text = loseText;
            }
            else
            {
                if (roundResult.winner_id == 0) //draw
                {
                    Debug.Log("RockSpockSceneController OnRoundResultCallback() winner DRAW");
                    leftText.GetComponent<Text>().text = drawText;
                    rightText.GetComponent<Text>().text = drawText;
                }
                else //right win
                {
                    Debug.Log("RockSpockSceneController OnRoundResultCallback() winner RIGHT");
                    leftText.GetComponent<Text>().text = loseText;
                    rightText.GetComponent<Text>().text = winText;
                }
            }
        }
        else //we watchers
        {
            Debug.Log("Constants.USER_ID == -1");
            if (Constants.LEFT_PLAYER_ID == roundResult.winner_id)
            {
                Debug.Log("RockSpockSceneController OnRoundResultCallback() winner LEFT_PLAYER_ID" +
                          Constants.LEFT_PLAYER_ID);
                leftText.GetComponent<Text>().text = winText;
                rightText.GetComponent<Text>().text = loseText;
            }

            else
            {
                if (roundResult.winner_id == 0) //draw
                {
                    Debug.Log("RockSpockSceneController OnRoundResultCallback() winner DRAW");
                    leftText.GetComponent<Text>().text = drawText;
                    rightText.GetComponent<Text>().text = drawText;
                }
                else
                {
                    Debug.Log("RockSpockSceneController OnRoundResultCallback() winner RIGHT_PLAYER_ID");
                    leftText.GetComponent<Text>().text = loseText;
                    rightText.GetComponent<Text>().text = winText;
                }
            }
        }
        StartCoroutine("PrepareToNewRound");
    }

    IEnumerator PrepareToNewRound()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("RockSpockSceneController PrepareToNewRound");

        //show messageBoard in Android
        messageRouter.SendAndroidMessage(new UnityMessage(1005, JsonUtility.ToJson(roundResult)));
        ResetScene();
        StopCoroutine("PrepareToNewRound");
    }

    private void PlayAnimation(Animator animator, int moveId)
    {
        switch (moveId)
        {
            case Panel.MOVE_ROCK:
                Debug.Log("RockSpockSceneController PlayAnimation ROCK_TRIGGER");
                animator.SetTrigger(ROCK_TRIGGER);
                break;
            case Panel.MOVE_PAPER:
                Debug.Log("RockSpockSceneController PlayAnimation PAPPER_TRIGGER");
                animator.SetTrigger(PAPPER_TRIGGER);
                break;
            case Panel.MOVE_SCISSORS:
                Debug.Log("RockSpockSceneController PlayAnimation SCISSORS_TRIGGER");
                animator.SetTrigger(SCISSORS_TRIGGER);
                break;
        }
    }

    public void OnRoundResult(RockSpockRoundResultMessage result)
    {
        Debug.Log("RockSpockSceneController OnRoundResult");
        roundResult = result;

        if (Constants.SELF_INDEX != -1)
        {
            // i'm playing
            int opponentIndex = Constants.SELF_INDEX == 0 ? 1 : 0;
            // if my index is 0, than opponent is 1, inverse otherwise
            var leftMove = roundResult.moves[Constants.SELF_INDEX];
            var rightMove = roundResult.moves[opponentIndex];
            PlayAnimation(leftHandAnimator, leftMove); //set sprites to hands
            PlayAnimation(rightHandAnimator, rightMove);

            moveCamToCenterAlloved = true;
        }
        else //I`m watcher
        {
            int rightPlayerIndex = Constants.LEFT_PLAYER_INDEX == 0 ? 1 : 0;
            var leftMove = roundResult.moves[Constants.LEFT_PLAYER_INDEX];
            var rightMove = roundResult.moves[rightPlayerIndex];

            leftPanel.Move(leftMove);
            rightPanel.Move(rightMove);

            PlayAnimation(leftHandAnimator, leftMove); //set sprites to hands
            PlayAnimation(rightHandAnimator, rightMove);
        }
    }

    private void MoveCameraToCenter()
    {
        Debug.Log("RockSpockSceneController MoveCameraToCenter");
        leftCamera.transform.position = Vector3.MoveTowards(leftCamera.transform.position, new Vector3(0, 0, -10F),
            cameraSpeed * Time.deltaTime);
        if (leftCamera.transform.position == new Vector3(0, 0, -10F))
        {
            Debug.Log("RockSpockSceneController moveCamToCenterAlloved = false");
            moveCamToCenterAlloved = false;
        }
    }

    private void MoveCameraToInitPosition()
    {
        Debug.Log("RockSpockSceneController MoveCameraToInitPosition");
        leftCamera.transform.position = new Vector3(-10, 0, -10F);
    }

    public void ResetScene()
    {
        Debug.Log("RockSpockSceneController ResetScene");
        leftPanel.ResetState();
        rightPanel.ResetState();

        leftText.GetComponent<Text>().text = "";
        rightText.GetComponent<Text>().text = "";

        leftHandAnimator.SetTrigger("default");
        rightHandAnimator.SetTrigger("default");

        MoveCameraToInitPosition();
    }
}
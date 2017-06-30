using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinTheDisksSceneController : MonoBehaviour
{
    public static int AUTO_FINALIZATION_ANGLE = 10;
    public const float INITIAL_DRAG_DELAY_TIME = 1.0f;

    public MessageRouter messageRouter;
    public ImageLoader leftImageLoader;
    public ImageLoader rightImageLoader;
    public DiskMover leftDiskMover;
    public DiskMover rightDiskMover;
    public Camera leftCamera;
    public Camera rightCamera;
    public UIController uiController;


    private SpinTheDisksPlayerData leftPlayer;
    private SpinTheDisksPlayerData rightPlayer;

    public void InitGame(SpinTheDisksGameState gameState)
    {
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
        SpinTheDisksSceneController.AUTO_FINALIZATION_ANGLE = gameState.auto_finalization;
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

    public void RoundStart(SpinTheDisksPlayerData p1, SpinTheDisksPlayerData p2, bool animate)
    {
        Debug.Log("RoundStart: UserID = " + Constants.USER_ID);
        Debug.Log("Constants.SELF_INDEX = " + Constants.SELF_INDEX);
        //        uiController.ShowGame(animate);
        uiController.ShowGame(animate);
        switch (Constants.SELF_INDEX)
        {
            case -1: //not playing, just watching
                if (Constants.LEFT_PLAYER_INDEX == 0)
                {
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

        leftImageLoader.LoadImage(Constants.BASE_URL + leftPlayer.category + "/" + leftPlayer.file);
        rightImageLoader.LoadImage(Constants.BASE_URL + rightPlayer.category + "/" + rightPlayer.file);
        for (int i = 0; i < 5; i++)
        {
            leftDiskMover.SetDisk(i, leftPlayer.disks[i]);
            rightDiskMover.SetDisk(i, rightPlayer.disks[i]);
        }
    }

    public void Move(int id, int disk, int angle)
    {
        if (Constants.SELF_INDEX != -1)
        {
            if (id == Constants.USER_ID)
            {
                leftDiskMover.MoveDisk(disk, angle);
            }
            else
            {
                rightDiskMover.MoveDisk(disk, angle);
            }
        }
        else
        {
            if (id == Constants.LEFT_PLAYER_ID)
            {
                Debug.Log("Left move. id = " + id);
                leftDiskMover.MoveDisk(disk, angle);
            }
            else
            {
                Debug.Log("Right move. id = " + id);
                rightDiskMover.MoveDisk(disk, angle);
            }
            Debug.Log("Move. LeftPlayerId: " + leftPlayer.id);
            Debug.Log("Move. RightPlayerId: " + rightPlayer.id);
        }
    }

    private void SwitchToOnePlayerMode()
    {
        rightCamera.enabled = false; //we need to see only our camera
        leftCamera.rect = new Rect(0, 0, 1, 1); // make camera fullscreen
        leftCamera.orthographicSize = 5f;
    }

    private void switchToTwoPlayerMode()
    {
        rightCamera.enabled = true; // enable second camera
        leftCamera.rect = new Rect(0, 0, 0.5f, 1); // half screen
        leftCamera.orthographicSize = 5f;
    }

    public void InitReadyState(bool leftReady, bool rightReady)
    {
        uiController.ShowUI();
    }

    void Start()
    {
        Screen.fullScreen = false;
    }
}
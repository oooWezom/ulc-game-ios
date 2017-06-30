using System;
using UnityEngine;


public class DiskMover : MonoBehaviour
{
    private const int defaultValue = -1;
    public MessageRouter messageRouter;
    public Material originalMaterial;
    public Material highlightedMaterial;
    public UIProgressController progressController;

    private Vector3 destination = new Vector3();
    private Vector3 initial = new Vector3();
    private GameObject diskObject;

    private float time = 1f;
    private float lastSendTime = 0f;
    private float delaySendTime = 0f;
    private int lastSendedAngle = -1;
    private bool isMoving = false;


    void Update()
    {
        if (time < 1f)
        {
            isMoving = true;
            time += Time.deltaTime*2f;
            float resultAngle = Mathf.LerpAngle(initial.y, destination.y, time);
            diskObject.transform.localEulerAngles = new Vector3(0, resultAngle, 0);
        }
        else
        {
            if (isMoving)
            {
                UpdateProgress();
                isMoving = false;
            }
        }
    }

    void Start()
    {
        lastSendTime = Time.time;
    }

    public void MoveDisk(int disk, int angle)
    {
        diskObject = GetDisk(disk);
        initial = diskObject.transform.localEulerAngles;
        destination.Set(0f, (float) angle, 0f);
        time = 0f;
        UpdateProgress();
    }

    public void SetDisk(int disk, int angle)
    {
        diskObject = GetDisk(disk);
        diskObject.transform.localEulerAngles = new Vector3(0, angle, 0);
        UpdateProgress();
    }


    public void DragDisk(GameObject disk, float angle)
    {
        diskObject = disk;
        diskObject.transform.localEulerAngles = new Vector3(0, angle, 0);

        if (Time.time > delaySendTime) //initial delay
        {
            if ((Time.time - lastSendTime) >= 1f) //once per second
            {
                Debug.Log("DragDisk(" + angle + ");");
                SendData();
                lastSendTime = Time.time;
            }
        }
    }

    public void DragDiskInstant(GameObject disk, float angle)
    {
        Debug.Log("DragDiskInstant angle:" + angle);
        diskObject = disk;
        diskObject.transform.localEulerAngles = new Vector3(0, angle, 0);
        SendData();
    }

    public void setInitialSendDelayTime()
    {
        delaySendTime = Time.time + SpinTheDisksSceneController.INITIAL_DRAG_DELAY_TIME;
    }

    public bool CheckAutoFinalization(GameObject disk)
    {
        float currentAngle = GetDiskAngle(disk);
        bool isAutoFinalize = IsAutoFinalizeAngle(currentAngle);
        if (isAutoFinalize)
        {
            MoveDisk(Convert.ToInt32(disk.name), 0);
            SendData(0); // complete image
            lastSendedAngle = defaultValue; //autofinalization resets last sended angle in case of next turn will autofinalize another disk
        }
        UpdateProgress();
        Debug.Log("CheckAutoFinalization = isAutoFinalize");
        return isAutoFinalize;
    }

    private GameObject GetDisk(int disk)
    {
        return gameObject.transform.Find(disk.ToString()).gameObject;
    }

    private void SendData()
    {
        // lastSendedAngle
        int diskAngle = Convert.ToInt32(diskObject.transform.localEulerAngles.y);
        if (lastSendedAngle != diskAngle) //we not on same angle
        {
            DiskMoveMessage moveMessage = new DiskMoveMessage(diskAngle, Convert.ToInt32(diskObject.name));
            messageRouter.SendAndroidMessage(new UnityMessage(1, JsonUtility.ToJson(moveMessage)));
            Debug.Log("SendData() angle:" + Convert.ToInt32(diskObject.transform.localEulerAngles.y));
            lastSendedAngle = diskAngle;
        }
    }

    private void SendData(int angle)
    {
        int diskAngle = angle;
        if (lastSendedAngle != diskAngle) //we not on same angle
        {
            Debug.Log("SendData(" + angle + ")");
            DiskMoveMessage moveMessage = new DiskMoveMessage(angle, Convert.ToInt32(diskObject.name));
            messageRouter.SendAndroidMessage(new UnityMessage(1, JsonUtility.ToJson(moveMessage)));
            lastSendedAngle = diskAngle;
        }
    }

    public void ActivateDisk(GameObject disk)
    {
        MeshRenderer meshRenderer = disk.GetComponent<MeshRenderer>();
        meshRenderer.material = highlightedMaterial;
    }

    public void DeActivateDisk(GameObject disk)
    {
        MeshRenderer meshRenderer = disk.GetComponent<MeshRenderer>();
        meshRenderer.material = originalMaterial;
    }

    private bool IsAutoFinalizeAngle(float currentAngle)
    {
        Debug.Log("IsAutoFinalizeAngle, angle: " + currentAngle);
        bool result = currentAngle >= (360 - SpinTheDisksSceneController.AUTO_FINALIZATION_ANGLE) ||
                      currentAngle <= SpinTheDisksSceneController.AUTO_FINALIZATION_ANGLE;
        Debug.Log("IsAutoFinalizeAngle = " + result);
        return result;
    }

    private float GetDiskAngle(GameObject disk)
    {
        return Utils.To360Angle(disk.transform.localEulerAngles.y);
    }

    private void UpdateProgress()
    {
        int completeCount = 0;
        for (int i = 0; i < 5; i++)
        {
            if (IsAutoFinalizeAngle(GetDiskAngle(GetDisk(i))))
            {
                completeCount++;
            }
        }
        progressController.UpdateProgress(completeCount);
    }
}
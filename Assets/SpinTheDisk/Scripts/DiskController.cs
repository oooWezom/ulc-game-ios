using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiskController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public DiskMover diskMover;
    public float startAngle = 0f;
    public float lastAngle = 0;
    private const int defaultValue = -1;

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!checkIsPlayer())
            return;
        lastAngle = startAngle + calculateAngle(eventData.pointerCurrentRaycast.worldPosition);
        Debug.Log("OnDrag: " + (startAngle + calculateAngle(eventData.pointerCurrentRaycast.worldPosition)));
        Debug.Log("lastAngle: " + lastAngle);
        diskMover.DragDisk(gameObject, lastAngle);
    }

    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!checkIsPlayer())
            return;
        diskMover.DeActivateDisk(gameObject);

        if (!diskMover.CheckAutoFinalization(gameObject))
        {
            Debug.Log("OnEndDrag:AutoFinalization = false");
            diskMover.DragDiskInstant(gameObject, lastAngle);
        }
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag " +
                  (transform.localEulerAngles.y - calculateAngle(eventData.pointerCurrentRaycast.worldPosition)));
        if (!checkIsPlayer())
            return;
        lastAngle = defaultValue;
        diskMover.setInitialSendDelayTime();
        startAngle = transform.localEulerAngles.y - calculateAngle(eventData.pointerCurrentRaycast.worldPosition);
        diskMover.ActivateDisk(gameObject);
    }

    private bool checkIsPlayer()
    {
        return Constants.SELF_INDEX > -1;
    }

    private float calculateAngle(Vector3 position)
    {
        var dir = position - transform.position;

        return Mathf.Repeat(Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg, 360f);
    }
}
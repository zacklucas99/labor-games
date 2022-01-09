using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NotifierObject : MonoBehaviour
{
    // Start is called before the first frame update

    public LayerMask EnemyLayer;

    public float notificationRad;
    public float interactRadius;
    public Color notificationRadColor;
    public Color interactRadColor;
    public bool drawGizmos;

    public bool canPickUp;
    public bool canCleanUp;

    public GameObject moveToPoint;

    public bool turnedOn = true;
    public Color turnedOffColor = Color.grey;
    public GameObject interactObject;
    public GameObject destroyObject;

    public bool notifyInView = true;
    public bool isBone = false;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        if (turnedOn)
        {
            Handles.color = notificationRadColor;
        }
        else
        {
            Handles.color = turnedOffColor;
        }
        Handles.DrawWireDisc(transform.position, Vector3.up, notificationRad);

        if (turnedOn)
        {
            Handles.color = interactRadColor;
        }
        else
        {
            Handles.color = turnedOffColor;
        }
        Handles.DrawWireDisc(transform.position, Vector3.up, interactRadius);
    }
    void Update()
    {

        var enemys = Physics.OverlapSphere(transform.position, notificationRad);
        foreach(var enemy in enemys)
        {
            if (turnedOn && !notifyInView)
            {
                enemy.GetComponent<NotificationReceiver>()?.ReceiveNotification(this);
            }
        }
    }

    public void Notify(OfficerController officer)
    {
        officer.GetComponent<NotificationReceiver>()?.ReceiveNotification(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NotifierObject : MonoBehaviour
{
    // Start is called before the first frame update

    public LayerMask EnemyLayer;

    public float notificationRad;
    public Color notificationRadColor;
    public bool drawGizmos;

    public bool canPickUp;

    public GameObject moveToPoint;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }

        Handles.color = notificationRadColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, notificationRad);
    }
    void Update()
    {

        var enemys = Physics.OverlapSphere(transform.position, notificationRad);
        foreach(var enemy in enemys)
        {
            enemy.GetComponent<NotificationReceiver>()?.ReceiveNotification(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationReceiver : MonoBehaviour
{
    [System.Serializable]
    public class NotificationEvent : UnityEvent<NotifierObject>
    {
    }

    public NotificationEvent notificationEvent;

    public void ReceiveNotification(NotifierObject obj) {
        notificationEvent.Invoke(obj);
    }
}

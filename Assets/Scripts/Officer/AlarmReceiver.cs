using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AlarmReceivedEvent : UnityEvent<Vector3>
{
}

public class AlarmReceiver : MonoBehaviour
{
    public AlarmReceivedEvent alarmReceivedEvent;

    public void AlarmReceived(Vector3 playerPos)
    {
        alarmReceivedEvent.Invoke(playerPos);
    }
}

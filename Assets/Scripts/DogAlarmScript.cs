using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DogAlarmScript : MonoBehaviour
{
    public float cameraNotifyRad;
    public bool drawGizmos;
    public Color gizmoColor;
    public LayerMask officerLayer;

    public void Bark(GameObject playerObj) {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        
        var officersInRange = Physics.OverlapSphere(transform.position, cameraNotifyRad, officerLayer);

        foreach (var officer in officersInRange)
        {
            if(officer.gameObject == this.gameObject)
            {
                continue;
            }
            officer.gameObject.GetComponent<AlarmReceiver>().AlarmReceived(playerObj.transform.position);
        }
    }

    private void OnDrawGizmos()
    {

        Handles.color = gizmoColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, cameraNotifyRad);
    }
}

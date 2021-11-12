using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SoundObject : MonoBehaviour
{
    // Start is called before the first frame update

    public bool turnedOn = true;
    public bool canTurnSoundOff = true;
    public float soundRad;
    public LayerMask enemyLayer;

    public Color radColor;
    public Color turnedOffRadColor = Color.gray;
    public bool drawGizmos;

    public float volume = 0;

    public const float VolumeLossFactor = 1;
    public const float wallVolumeLoss = 30;

    public LayerMask environLayer;

    public Vector3 topOffset = Vector3.up;

    public void Start()
    {
        if(environLayer == 0)
        {
            environLayer = LayerMask.GetMask("Ground");
        }
    }

    void Update()
    {
        if (turnedOn)
        {
            var enemies = Physics.OverlapSphere(transform.position, soundRad, enemyLayer);
            foreach(var enemy in enemies)
            {

                enemy.GetComponent<OfficerController>().ReceiveSound(this, CalculateVolumeAtPlayer(enemy.gameObject));
            }
        }
    }


    public float CalculateVolumeAtPlayer(GameObject enemy)
    {
        // Trying to come up with some kind of formular to calculate sound volume
        var numWalls = Physics.RaycastAll(new Ray(transform.position, (enemy.transform.position+ topOffset) - transform.position),
            (transform.position- enemy.transform.position).magnitude, environLayer);
        Debug.Log("numWalls:" + numWalls.Length);
        return volume - (enemy.transform.position - transform.position).magnitude * VolumeLossFactor - numWalls.Length * wallVolumeLoss;
    }

    public void SetTurnedOn(bool val) {
        turnedOn = val;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Handles.color = turnedOn?radColor:turnedOffRadColor;
        Handles.DrawWireDisc(transform.position, Vector2.up, soundRad);
    }
}

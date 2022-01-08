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
    public bool canPickUp;
    public bool canCleanUp;
    public bool canDestroy = true;
    public bool isBone;
    public LayerMask enemyLayer;

    public Color radColor;
    public Color turnedOffRadColor = Color.gray;
    public bool drawGizmos;

    public float volume = 0;

    public const float VolumeLossFactor = 1;
    public const float wallVolumeLoss = 6;

    public LayerMask environLayer;

    public Vector3 topOffset = Vector3.up;

    public int numLines = 5;
    public float stepSize = 1f;

    public float interactDist;
    public Color interactDistColor;

    public bool isLocked{ get; set; }



    public void Awake()
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
            var enemies = Physics.OverlapSphere(transform.position, volume, enemyLayer);
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
        Handles.color = interactDistColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, interactDist);

        Handles.color = turnedOn?radColor:turnedOffRadColor;
        Handles.DrawWireDisc(transform.position, Vector2.up, volume);

        // Trying to draw lines to simulate the sound behaviour
        for(int i = 0; i< numLines; i++)
        {
            float dist = 0;
            float angle = 360f / ((float)numLines) * i;
            var rot = Quaternion.AngleAxis(angle, transform.up);
            for (float j = 0; j <= volume; j+=stepSize)
            {
                dist = j;
                var vol = volume;
                var numWalls = Physics.RaycastAll(new Ray(transform.position, rot * transform.forward), j, environLayer);
                vol -= j+numWalls.Length * wallVolumeLoss; // Calculating the current volume
                if(vol < 0)
                {
                    break;
                }
            }

            Handles.DrawLine(transform.position, transform.position + (rot * transform.forward) * Mathf.Max(0, dist));
        }

    }
}

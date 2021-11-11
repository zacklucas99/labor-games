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

    void Update()
    {
        if (turnedOn)
        {
            var enemies = Physics.OverlapSphere(transform.position, soundRad, enemyLayer);
            foreach(var enemy in enemies)
            {

                enemy.GetComponent<OfficerController>().ReceiveSound(this);
            }
        }
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

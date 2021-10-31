using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerApproximationEvent : UnityEvent<GameObject>
{
}
public class ApproximationRecognition : MonoBehaviour
{

    public LayerMask playerMask;
    public Color approximationColor = Color.blue;
    public float playerRad = 2;
    public bool gizmosVisible = true;

    public PlayerApproximationEvent playerApproximationEvent = new PlayerApproximationEvent();
    public UnityEvent approxmiationEndedEvent = new UnityEvent();

    private void OnDrawGizmos()
    {
        if (!gizmosVisible)
        {
            return;
        }
        Handles.color = approximationColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, playerRad);
    }

    public void Update()
    {
        var players = Physics.OverlapSphere(transform.position, playerRad, playerMask);
        foreach(var player in players)
        {
            playerApproximationEvent.Invoke(player.gameObject);
        }
        if(players.Length == 0)
        {
            approxmiationEndedEvent.Invoke();
        }
    }

}

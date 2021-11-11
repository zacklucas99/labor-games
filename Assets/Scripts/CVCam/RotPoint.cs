using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public Color rotPointColor;
    public bool drawGizmosEnabled = true;
    private float rot = 0.2f;

    private void OnDrawGizmos()
    {
        if (!drawGizmosEnabled)
        {
            return;
        }

        Gizmos.color = rotPointColor;
        Gizmos.DrawWireSphere(transform.position, rot);
    }
}

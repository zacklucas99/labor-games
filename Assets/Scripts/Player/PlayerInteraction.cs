using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform cam;

    public float maxDistance = 7f; 
    public float collisionRadius = 0.4f;
    private Transform interactionObj;
    public LayerMask interactionMask;
    public LayerMask collisionMask;
    private Vector3 hitPosition;

    private Shader shaderNoOutline;
    private Shader shaderOutline;

    void Start()
    {
        interactionObj = null;
        shaderNoOutline = Shader.Find("Standard");
        shaderOutline = Shader.Find("Unlit/Outline");
    }

    // Update is called once per frame
    void Update()
    {
        DetectInteraction(); //outline when facing an interaction obj

        InteractWithObject(); //press interact with outlined object
    }

    private void DetectInteraction()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, collisionMask)) //if raycast hits interaction obj
        {
            hitPosition = hit.point;
            Collider[] hitColliders = Physics.OverlapSphere(hitPosition, collisionRadius, interactionMask);

            if (hitColliders.Length > 0)
            {
                if (interactionObj != null && interactionObj != hitColliders[0].transform) //if facing new interaction obj
                {
                    interactionObj.GetComponent<Renderer>().material.shader = shaderNoOutline; //no outline on old object
                }

                interactionObj = hitColliders[0].transform;
                interactionObj.GetComponent<Renderer>().material.shader = shaderOutline; //outline on object that you are currently facing
            }
            else
            {
                ResetOutline(); //no outlines if interaction obj is not in overlap sphere anymore
            }
        }
        else
        {
            ResetOutline(); //no outlines if raycast does not have a hit point anymore
        }
    }

    private void ResetOutline()
    {
        if (interactionObj != null)
        {
            interactionObj.GetComponent<Renderer>().material.shader = shaderNoOutline;
            interactionObj = null;
        }
    }

    private void InteractWithObject()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactionObj != null) //interact with object by pressing E
        {
            if (interactionObj.gameObject.layer == 10) //if throwing obj
            {
                if (GetComponent<PlayerInventory>().AddObject(interactionObj.gameObject))
                {
                    Destroy(interactionObj.parent.gameObject);
                }
            }
            else
            {
                if (interactionObj.GetComponent<PaintingBorder>()) //if painting (border)
                {
                    interactionObj.GetComponent<PaintingBorder>().ChangeCanvasTexture(); //texture change on canvas
                }
                else
                {
                    interactionObj.GetComponent<Renderer>().material.SetColor("_Color", Color.red); //color active interaction obj
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, collisionRadius);
    }
}

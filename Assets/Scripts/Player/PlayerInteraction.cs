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

    private Vector3 oldPos;

    void Start()
    {
        interactionObj = null;
        shaderNoOutline = Shader.Find("Standard");
        shaderOutline = Shader.Find("Unlit/Outline");
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.GetComponent<ThirdPersonMovement>().isPainting && !transform.GetComponent<ThirdPersonMovement>().isSplatooning) 
        {
            CancelInvoke();
            DetectInteraction(); //outline when facing an interaction obj
        }

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
                interactionObj.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 1.03f);
                interactionObj.GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(1f, 172f/255f, 0));
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
            else if (interactionObj.gameObject.tag == "HidingPlace")
            {
                if (!transform.GetComponent<ThirdPersonMovement>().blockedMovement)
                {
                    gameObject.SetActive(false);
                    transform.position = interactionObj.GetChild(0).transform.position;
                    transform.rotation = interactionObj.GetChild(0).transform.rotation;
                    transform.GetComponent<ThirdPersonMovement>().isHiding = true;
                    transform.GetComponent<ThirdPersonMovement>().blockedMovement = true;
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                    transform.position = interactionObj.GetChild(1).transform.position;
                    transform.rotation = interactionObj.GetChild(1).transform.rotation;
                    transform.GetComponent<ThirdPersonMovement>().isHiding = false;
                    transform.GetComponent<ThirdPersonMovement>().blockedMovement = false;
                    gameObject.SetActive(true);
                }


            }
            else if (interactionObj.gameObject.tag == "CamButton")
            {
                Debug.Log("cam button interaction");
                interactionObj.GetComponentInParent<CamButton>().press();
            }
            else
            {
                if (interactionObj.GetComponent<PaintingBorder>()) //if painting (border)
                {
                    if (!interactionObj.GetComponent<PaintingBorder>().mustached)
                    {
                        transform.GetComponent<ThirdPersonMovement>().isPainting = true;
                        Invoke(nameof(FinishPainting), transform.GetComponent<ThirdPersonMovement>().GetPaintingClipLength());
                    }
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

    private void FinishPainting()
    {
        transform.GetComponent<ThirdPersonMovement>().isPainting = false;
        interactionObj.GetComponent<PaintingBorder>().ChangeCanvasTexture(); //texture change on canvas
    }
}

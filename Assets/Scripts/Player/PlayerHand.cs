using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform cam;
    public LayerMask collisionMask;
    private Shader shaderOutline;
    public GameObject coinPrefab;
    public float throwForce = 1f;
    public int lineSegment = 10; //smoothness of line
    private LineRenderer lineRenderer;
    public bool highlightCoin = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineSegment;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            lineRenderer.enabled = true;
            Ray camRay = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit, 100f, collisionMask))
            {
                Vector3 vo = CalculateVelocity(hit.point, transform.position, 1f);
                Visualize(vo * throwForce);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Debug.Log("Tossing coin");
                    GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    coin.GetComponent<Rigidbody>().AddForce(vo * throwForce, ForceMode.Impulse);
                    //coin.GetComponent<Rigidbody>().velocity = vo;

                    //Debug settings to make coin more visible
                    if (highlightCoin)
                    {
                        coin.GetComponentInChildren<Renderer>().material.shader = shaderOutline;
                        coin.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineWidth", 2.0f);
                    }
                }
            }

        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;
        Vector3 result = transform.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + transform.position.y;
        result.y = sY;
        return result;
    }

    void Visualize(Vector3 vo)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment);
            lineRenderer.SetPosition(i, pos);
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHand : MonoBehaviour
{
    public Transform cam;
    public LayerMask collisionMask;
    private Shader shaderOutline;
    public GameObject coinPrefab;
    public float throwForce = 1f;
    public int lineSegment = 10; //smoothness of line
    private LineRenderer throwingLine;
    public bool highlightCoin = false;

    public LineRenderer hitCircle;
    [Range(3, 256)]
    private int circleSegment = 128;
    [Range(0.1f, 10f)]
    private float radius = 0.2f;

    public float throwCoolDown = 0.6f;
    private bool activeCoolDown = false;

    public CinemachineFreeLook vcam;
    public float fov = 40f;
    private float fovInit;
    public float fovTarget = 30f;
    private float fovTargetInit;

    public Transform camPos;
    public float camOffset = 0f;
    private float camOffsetInit;
    public float camOffsetTarget = 0.5f;
    private float camOffsetTargetInit;

    void Start()
    {
        shaderOutline = Shader.Find("Unlit/Outline");

        throwingLine = GetComponent<LineRenderer>();
        throwingLine.positionCount = lineSegment;

        hitCircle.positionCount = circleSegment+1;

        fovInit = fov;
        fovTargetInit = fovTarget;
        fovTarget = fov;

        camOffsetInit = camOffset;
        camOffsetTargetInit = camOffsetTarget;
        camOffsetTarget = camOffset;
}

    void Update()
    {
        float fovDelta = fovTarget - fov;
        fovDelta *= Time.deltaTime*5;
        fov += fovDelta;
        vcam.m_Lens.FieldOfView = fov;

        float camOffsetDelta = camOffsetTarget - camOffset;
        camOffsetDelta *= Time.deltaTime;
        camOffset += camOffsetDelta;
        camPos.localPosition = new Vector3(camOffset, 0, 0);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            fovTarget = fovTargetInit;
            camOffsetTarget = camOffsetTargetInit;
            hitCircle.enabled = true;
            throwingLine.enabled = true;
            Ray camRay = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit, 100f, collisionMask))
            {
                Vector3 vo = CalculateVelocity(hit.point, transform.position, 1f);
                Visualize(vo * throwForce);
                CreateCirclePoints(hit.point, GetHitFace(hit));

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!activeCoolDown)
                    {
                        Debug.Log("Tossing coin");
                        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                        coin.GetComponent<Rigidbody>().AddForce(vo * throwForce, ForceMode.Impulse);

                        //Debug settings to make coin more visible
                        if (highlightCoin)
                        {
                            coin.GetComponentInChildren<Renderer>().material.shader = shaderOutline;
                            coin.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineWidth", 2.0f);
                        }

                        activeCoolDown = true;
                        Invoke(nameof(DeactivateCoolDown), throwCoolDown);
                    }
                    
                }
            }
        }
        else
        {
            fovTarget = fovInit;
            camOffsetTarget = camOffsetInit;
            camPos.localPosition = new Vector3(0, 0, 0);
            throwingLine.enabled = false;
            hitCircle.enabled = false;
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
            throwingLine.SetPosition(i, pos);
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

    void CreateCirclePoints(Vector3 pos, HitFace face)
    {
        float deltaTheta = (float)(2.0 * Mathf.PI) / circleSegment;
        float theta = 0f;

        if(face == HitFace.North || face == HitFace.South)
        {
            for (int i = 0; i < circleSegment + 1; i++)
            {
                float x = radius * Mathf.Cos(theta) + pos.x;
                float y = radius * Mathf.Sin(theta) + pos.y;
                float z = pos.z;
                if (face == HitFace.North)
                {
                    z += 0.01f;
                } else
                {
                    z -= 0.01f;
                }
                hitCircle.SetPosition(i, new Vector3(x, y, z));
                theta += deltaTheta;
            }
        }
        else if (face == HitFace.East || face == HitFace.West)
        {
            for (int i = 0; i < circleSegment + 1; i++)
            {
                float x = pos.x;
                float y = radius * Mathf.Cos(theta) + pos.y;
                float z = radius * Mathf.Sin(theta) + pos.z;
                if (face == HitFace.East)
                {
                    x += 0.01f;
                }
                else
                {
                    x -= 0.01f;
                }
                hitCircle.SetPosition(i, new Vector3(x, y, z));
                theta += deltaTheta;
            }
        }
        else if (face == HitFace.Up || face == HitFace.Down)
        {
            for (int i = 0; i < circleSegment + 1; i++)
            {
                float x = radius * Mathf.Cos(theta) + pos.x;
                float y = pos.y;
                float z = radius * Mathf.Sin(theta) + pos.z;
                if (face == HitFace.Up)
                {
                    y += 0.01f;
                }
                else
                {
                    y -= 0.01f;
                }
                hitCircle.SetPosition(i, new Vector3(x, y, z));
                theta += deltaTheta;
            }
        }
        else
        {
            hitCircle.enabled = false;
        }
    }

    public enum HitFace
    {
        None,
        Up,
        Down,
        East,
        West,
        North,
        South
    }

    public HitFace GetHitFace(RaycastHit hit)
    {
        Vector3 incomingVec = hit.normal - Vector3.up;

        if (incomingVec == new Vector3(0, -1, -1))
            return HitFace.South;

        if (incomingVec == new Vector3(0, -1, 1))
            return HitFace.North;

        if (incomingVec == new Vector3(0, 0, 0))
            return HitFace.Up;

        if (incomingVec == new Vector3(1, 1, 1))
            return HitFace.Down;

        if (incomingVec == new Vector3(-1, -1, 0))
            return HitFace.West;

        if (incomingVec == new Vector3(1, -1, 0))
            return HitFace.East;

        return HitFace.None;
    }

    private void DeactivateCoolDown()
    {
        activeCoolDown = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class FoundPlayerEvent : UnityEvent<GameObject>
{
}

public class FieldOfView : MonoBehaviour
{
    // Start is called before the first frame update

    private MeshFilter meshFilter;

    public int meshResolution;
    public int refinementSteps;
    public float maxThreshold;

    [Range(0, 360)]
    public float viewAngle;

    public float viewDist;

    public LayerMask actionMask;
    public LayerMask environment;


    private bool foundPlayers = false;

    public float offset = 1;
    public Color gizmosColor;
    public bool drawGizmos = true;

    public OfficerController officer;

    public FoundPlayerEvent PlayerFoundEvent = new FoundPlayerEvent();
    public UnityEvent PlayerLostEvent = new UnityEvent();

    public bool logging;




    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        GenerateMesh();
    }

    private void Update()
    {
        CheckCollision();
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Vision";
        meshFilter.mesh = mesh;

        float angleSteps = viewAngle / meshResolution;

        ViewHitInfo[] hitPoints = new ViewHitInfo[meshResolution + 1];
        RaycastHit hitInfo;
        for (int i = 0; i < meshResolution + 1; i++)
        {
            float currentAngle = -viewAngle / 2 + angleSteps * i;
            Vector3 currentVec = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;
            Debug.DrawRay(transform.position, currentVec * viewDist, Color.cyan);
            if (Physics.Raycast(transform.position+ Vector3.up * offset, currentVec, out hitInfo, viewDist, environment))
            {
                hitPoints[i] = new ViewHitInfo { HitPos = hitInfo.point, Angle = currentAngle, HitWall = true };
            }
            else
            {
                hitPoints[i] = new ViewHitInfo { HitPos = transform.position + currentVec * viewDist + Vector3.up * offset , Angle = currentAngle, HitWall = false };
            }
            hitPoints[i].HitPos = transform.InverseTransformPoint(hitPoints[i].HitPos);
        }
        
        //Todo: Refine Position
        RefinePosition(hitPoints);

        Vector3[] vertices = new Vector3[hitPoints.Length + 1];
        vertices[0] = new Vector3() + Vector3.up*offset;
        for (int i = 0; i < hitPoints.Length; i++)
        {
            vertices[i + 1] = hitPoints[i].HitPos;
        }
        mesh.vertices = vertices;

        List<int> trianglePoints = new List<int>();
        for (int i = 2; i < mesh.vertices.Length; i++)
        {
            trianglePoints.Add(0);
            trianglePoints.Add(i - 1);
            trianglePoints.Add(i);
        }
        mesh.triangles = trianglePoints.ToArray();
    }

    public void RefinePosition(ViewHitInfo[] infos)
    {
        if (infos.Length < 1)
        {
            return;
        }
        for (int i = 2; i < infos.Length - 1; i++)
        {
            if (infos[i].HitWall != infos[i - 1].HitWall)
            {
                if (infos[i].HitWall == true ||(infos[i].HitPos - infos[i-1].HitPos).magnitude > maxThreshold)
                {
                    infos[i] = ExecuteRefinement(infos[i], infos[i - 1]);
                }

                if (infos[i - 1].HitWall == true)
                {
                    infos[i - 1] = ExecuteRefinement(infos[i - 1], infos[i]);
                }

            }
        }
    }

    public ViewHitInfo ExecuteRefinement(ViewHitInfo wallHit, ViewHitInfo notWallHit)
    {
        ViewHitInfo viewHitInfo = wallHit;
        for (int i = 0; i < refinementSteps; i++)
        {
            float middleAngle = (wallHit.Angle + notWallHit.Angle) / 2;

            Vector3 currentVec = Quaternion.AngleAxis(middleAngle, Vector3.up) * transform.forward;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up * offset, currentVec, out hitInfo, viewDist, environment) && !((wallHit.HitPos - notWallHit.HitPos).magnitude > maxThreshold))
            {
                viewHitInfo = new ViewHitInfo { Angle = middleAngle, HitPos = transform.InverseTransformPoint(hitInfo.point), HitWall = true };
                wallHit = viewHitInfo;
            }
            else
            {
                notWallHit = viewHitInfo;
            }
        }

        return viewHitInfo;
    }




    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = gizmosColor;
        Handles.DrawWireDisc(transform.position // position
                              , transform.up                       // normal
                              , viewDist);
        Vector3 vecMin = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up) * transform.forward;
        Handles.DrawLine(transform.position, transform.position + vecMin * viewDist);

        Vector3 vecMax = Quaternion.AngleAxis(viewAngle / 2, Vector3.up) * transform.forward;
        Handles.DrawLine(transform.position, transform.position + vecMax * viewDist);
        if (EditorApplication.isPlaying)
        {
            CheckCollisionUI();
        }
    }

    private void CheckCollisionUI()
    {
        Handles.color = Color.red;

        foreach (var collider in Physics.OverlapSphere(transform.position, viewDist, actionMask))
        {
            var dist = collider.transform.position - transform.position;
            dist = new Vector3(dist.x, 0, dist.z);
            if (Mathf.Abs(Vector3.Angle(dist, transform.forward)) <= viewAngle / 2)
            {
                if (!Physics.Raycast(new Ray(transform.position + new Vector3(0, 1, 0),dist),viewDist, environment))
                {
                    return;
                }
                Handles.DrawLine(transform.position, collider.transform.position);
            }

        }
    }

    private void CheckCollision()
    {
        Handles.color = Color.red;
        bool foundPlayerThisRound = false;

        foreach (var collider in Physics.OverlapSphere(transform.position, viewDist, actionMask))
        {
            var dist = collider.transform.position - transform.position;
            dist = new Vector3(dist.x, 0, dist.z);
            if (Mathf.Abs(Vector3.Angle(dist, transform.forward)) <= viewAngle / 2)
            {
                RaycastHit info;
                Ray ray = new Ray(transform.position, collider.transform.position - transform.position);

                if (Physics.Raycast(ray, out info, environment))
                {
                    if (logging)
                    {
                        Debug.Log(info.collider.gameObject);
                    }

                    if(collider.GetComponent<ThirdPersonMovement>() == null && collider.GetComponent<NotifierObject>() == null)
                    {
                        continue;
                    }
                }
                GameObject gameObject = collider.gameObject;

                if (gameObject.GetComponent<ThirdPersonMovement>())
                {
                    PlayerFoundEvent.Invoke(gameObject);
                }

                else if (gameObject.GetComponent<NotifierObject>() && gameObject.GetComponent<NotifierObject>().notifyInView &&
                    gameObject.GetComponent<NotifierObject>().turnedOn)
                {
                    gameObject.GetComponent<NotifierObject>().Notify(officer);
                }
                foundPlayers = true;
                foundPlayerThisRound = true;

            }

        }
        if (foundPlayers && !foundPlayerThisRound)
        {
            foundPlayers = false;
            PlayerLostEvent.Invoke();
        }
    }

    public void Disable()
    {
        // Disabling the view, when camera turned off
        GetComponent<MeshRenderer>().enabled = false;
        this.enabled = false;
    }
}

public struct ViewHitInfo
{
    public bool HitWall { get; set; }
    public float Angle { get; set; }

    public Vector3 HitPos { get; set; }
}

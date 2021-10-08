using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class OfficerController : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private RoutePoint[] points;
    public int pointIndex;
    public RouteVisualization route;
    [Range(0,360)]
    public float viewAngle;

    public float viewDist;

    private bool destinationSet = false;
    public int waitTime = 5;

    public LayerMask actionMask;

    private MeshFilter meshFilter;

    public int meshResolution;
    public int refinementSteps;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        agent = GetComponent<NavMeshAgent>();
        if (route)
        {
            points = route.GetPoints();
        }
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint();
        }

    }

    IEnumerator GotoNextPoint()
    {
        if (points[(pointIndex) % points.Length].isStoppable)
        {
            yield return new WaitForSeconds(waitTime);
        }
        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
        destinationSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !destinationSet && route!= null && route.GetPoints().Length > 1)
        {
            destinationSet = true;
            StartCoroutine(GotoNextPoint());
        }

        GenerateMesh();

    }


    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position // position
                              , transform.up                       // normal
                              , viewDist);
        Vector3 vecMin = Quaternion.AngleAxis(-viewAngle/2, Vector3.up) * transform.forward;
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

        foreach (var collider in Physics.OverlapSphere(transform.position,viewDist, actionMask)) {
            var dist = collider.transform.position - transform.position;
            dist = new Vector3(dist.x, 0, dist.z);
            if (Mathf.Abs(Vector3.Angle(dist, transform.forward)) <= viewAngle/2)
            {
                if (!Physics.Raycast(new Ray(transform.position + new Vector3(0, 1, 0), dist)))
                {
                    return;
                }
                Handles.DrawLine(transform.position, collider.transform.position);
            }
            
        }
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Vision";
        meshFilter.mesh = mesh;

        float angleSteps = viewAngle / meshResolution;

        ViewHitInfo[] hitPoints = new ViewHitInfo[meshResolution+1];
        RaycastHit hitInfo;
        for (int i = 0; i< meshResolution+1; i++)
        {
            float currentAngle = -viewAngle / 2 + angleSteps * i;
            Vector3 currentVec= Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;
            if(Physics.Raycast(transform.position, currentVec,out hitInfo, viewDist))
            {
                hitPoints[i] = new ViewHitInfo { HitPos = hitInfo.point, Angle = currentAngle, HitWall = true };
            }
            else
            {
                hitPoints[i] = new ViewHitInfo { HitPos = transform.position + currentVec * viewDist, Angle = currentAngle, HitWall = false };
            }
            hitPoints[i].HitPos=transform.InverseTransformPoint(hitPoints[i].HitPos);
        }

        //Todo: Refine Position
        RefinePosition(hitPoints);

        Vector3[] vertices = new Vector3[hitPoints.Length + 1];
        vertices[0] = new Vector3() +Vector3.up;
        for (int i = 0; i< hitPoints.Length; i++)
        {
            vertices[i + 1] = hitPoints[i].HitPos+ Vector3.up;
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
        for (int i = 1; i < infos.Length; i++)
        {
            if(infos[i].HitWall != infos[i - 1].HitWall)
            {
                if(infos[i].HitWall == true)
                {
                    infos[i] = ExecuteRefinement(infos[i], infos[i-1]);
                }

                if (infos[i-1].HitWall == true)
                {
                    infos[i-1] = ExecuteRefinement(infos[i - 1], infos[i]);
                }

            }
        }
    }

    public ViewHitInfo ExecuteRefinement(ViewHitInfo wallHit, ViewHitInfo notWallHit)
    {
        ViewHitInfo viewHitInfo = wallHit;
        for (int i = 0; i< refinementSteps; i++)
        {
            float middleAngle = (wallHit.Angle + notWallHit.Angle) / 2;

            Vector3 currentVec = Quaternion.AngleAxis(middleAngle, Vector3.up) * transform.forward;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, currentVec, out hitInfo, viewDist))
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

}

public struct ViewHitInfo
{
    public bool HitWall { get; set; }
    public float Angle { get; set; }

    public Vector3 HitPos { get; set; }
}

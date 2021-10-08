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

        Vector3[] hitPoints = new Vector3[meshResolution];
        for(int i = 0; i< meshResolution; i++)
        {
            Vector3 currentVec= Quaternion.AngleAxis(-viewAngle / 2 + angleSteps * i, Vector3.up) * transform.forward;
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, currentVec,out hitInfo, viewDist))
            {
                hitPoints[i] = hitInfo.point;
            }
            else
            {
                hitPoints[i] = transform.position + currentVec * viewDist;
            }
            hitPoints[i] = transform.InverseTransformPoint(hitPoints[i]);
        }

        Vector3[] vertices = new Vector3[hitPoints.Length + 1];
        vertices[0] = new Vector3() +Vector3.up;
        for (int i = 0; i< hitPoints.Length; i++)
        {
            vertices[i + 1] = hitPoints[i]+ Vector3.up;
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
}

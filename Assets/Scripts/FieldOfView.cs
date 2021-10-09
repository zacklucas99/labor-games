using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        GenerateMesh();
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
            if (Physics.Raycast(transform.position, currentVec, out hitInfo, viewDist))
            {
                hitPoints[i] = new ViewHitInfo { HitPos = hitInfo.point, Angle = currentAngle, HitWall = true };
            }
            else
            {
                hitPoints[i] = new ViewHitInfo { HitPos = transform.position + currentVec * viewDist, Angle = currentAngle, HitWall = false };
            }
            hitPoints[i].HitPos = transform.InverseTransformPoint(hitPoints[i].HitPos);
        }

        //Todo: Refine Position
        RefinePosition(hitPoints);

        Vector3[] vertices = new Vector3[hitPoints.Length + 1];
        vertices[0] = new Vector3() + Vector3.up;
        for (int i = 0; i < hitPoints.Length; i++)
        {
            vertices[i + 1] = hitPoints[i].HitPos + Vector3.up;
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
            if (Physics.Raycast(transform.position, currentVec, out hitInfo, viewDist) && !((wallHit.HitPos - notWallHit.HitPos).magnitude > maxThreshold))
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
                if (!Physics.Raycast(new Ray(transform.position + new Vector3(0, 1, 0), dist)))
                {
                    return;
                }
                Handles.DrawLine(transform.position, collider.transform.position);
            }

        }
    }
}

public struct ViewHitInfo
{
    public bool HitWall { get; set; }
    public float Angle { get; set; }

    public Vector3 HitPos { get; set; }
}

using UnityEngine;
using UnityEngine.AI;

public class OfficerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera cam;
    public NavMeshAgent agent;
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log("Move:"+hit.point);
                agent.SetDestination(hit.point);
            }
        }
        
    }
}

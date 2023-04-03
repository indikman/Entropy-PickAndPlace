using Unity.VisualScripting;
using UnityEngine;

public class LiftObject : MonoBehaviour
{
    [SerializeField] private float curveHeight;
    [SerializeField] private Material lineMat;
    [SerializeField] private Gradient lineColor;
    [SerializeField] private int lineFrequency=20;
    [SerializeField] private Transform gunPoint;

    private Camera cam;
    private SpringJoint joint;
    private LineRenderer line;
    private Rigidbody pointRB;
    private Rigidbody objectRB;
    private RaycastHit hit;
    private GameObject pointer;
    private Vector3 localHitpoint;
    private bool isConnected;

    Vector3 pointA, pointB, pointC, pointAB, pointBC;
    Vector2 midPoint;

    void Start()
    {
        cam= Camera.main;
        isConnected = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            midPoint = new Vector2(Screen.width/2, Screen.height/2);

            if(!isConnected && Physics.Raycast(cam.ScreenPointToRay(midPoint), out hit))
            {
                if (!hit.transform.CompareTag("liftable"))
                    return;

                isConnected = true;
                localHitpoint = hit.transform.InverseTransformPoint(hit.point);

                pointer = new GameObject("pointer");
                pointer.transform.position = hit.point;
                pointer.transform.parent = cam.transform;

                pointRB = pointer.AddComponent<Rigidbody>();
                pointRB.isKinematic = true;
                objectRB = hit.transform.GetComponent<Rigidbody>();
                
                joint = objectRB.AddComponent<SpringJoint>();
                joint.spring = 500;
                joint.damper = 1;
                joint.tolerance = 0.001f;
                joint.connectedBody = pointRB;

                line = gameObject.AddComponent<LineRenderer>();
                line.material = lineMat;
                line.startWidth = 0.01f;
                line.endWidth = 0.005f;
                line.colorGradient = lineColor;
                line.positionCount = lineFrequency;
            }
            else
            {
                //clear line, rotate and drop object
                isConnected = false;
                Destroy(pointer.gameObject);
                Destroy(line);
            }
        }

        if(Input.GetMouseButtonUp(0) && isConnected)
        {
            isConnected = false;

            if(!pointer)
                Destroy(pointer.gameObject);
            if(!line)
                Destroy(line);
        }

        if (Input.GetMouseButton(0) && isConnected && pointer!= null)
        {
            if (line != null)
            {
                pointA = gunPoint.transform.position;
                pointC = hit.transform.TransformPoint(localHitpoint);
                pointB = pointA + (pointC - pointA) / 2 + Vector3.up * curveHeight * objectRB.mass;

                for (int i = 0; i < lineFrequency; i++)
                {
                    pointAB = Vector3.Lerp(pointA, pointB, i / (float)lineFrequency);
                    pointBC = Vector3.Lerp(pointB, pointC, i / (float)lineFrequency);

                    line.SetPosition(i, Vector3.Lerp(pointAB, pointBC, i / (float)lineFrequency));
                }
            }
        }
    }
}

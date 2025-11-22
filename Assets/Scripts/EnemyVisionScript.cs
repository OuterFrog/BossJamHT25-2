using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionScript : MonoBehaviour
{


    
    public float viewAnagleFOV;
    public float viewRange;
    public float meshResolution;

    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // Update is called once per frame
    void Update()
    {
        DrawFieldOfView();
        //Debug.DrawLine(transform.position, dirFromAngle(transform.eulerAngles.y + 30) *5 + transform.position, Color.yellow);

        if (GameManager.singleton == null)
        {
            Debug.Log("no gamemanager ref found");
        }
        if (GameManager.singleton.GetPlayerObj() != null) {
            lookFor(GameManager.singleton.GetPlayerObj());
        }
        //else Debug.Log("no playerref");
    }


    void lookFor(GameObject player)
    {
        RaycastHit hit;

        Vector3 direction = player.transform.position - transform.position;

        Debug.DrawRay(transform.position, direction, Color.red);

        //Debug.Log(Vector3.Dot(direction.normalized, transform.forward.normalized));

        if (Vector3.Magnitude(direction) < viewRange && Physics.Raycast( transform.position, direction, out hit, viewRange))
        {
            if (Vector3.Dot(direction.normalized, transform.forward.normalized) > viewAnagleFOV / 180)
            {

                Debug.Log("i can see you");

            }
            else { Debug.Log("Line of sight"); }
            
        }
        else { Debug.Log("i can t see you"); }
    }


    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAnagleFOV * meshResolution);
        float stepAngleSize = viewAnagleFOV / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAnagleFOV / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public Vector3 dirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = dirFromAngle(globalAngle);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRange))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir *  viewRange,viewRange, globalAngle);
        }
    }
    
 }

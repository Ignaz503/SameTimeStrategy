using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewVisualization : MonoBehaviour {

    [SerializeField] float viewRadius;
    public float ViewRadius { get { return viewRadius; } }
    [Range(0,360)]
    [SerializeField] float viewAngle;
    public float ViewAngle { get { return viewAngle; } }

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float meshResolution;
    public float MeshResolution { get { return meshResolution; } }
    [SerializeField] int edgeResolveInteration;
    public int EdgeResolveInteration { get { return edgeResolveInteration; } }
    [SerializeField] float edgeDistanceThreshold;
    public float EdgeDistanceThreshold { get { return edgeDistanceThreshold; } }

    [SerializeField] MeshFilter viewMeshFilter;
    Mesh viewMesh;

    [HideInInspector] public List<Transform> visibleTargets;
    
    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "FOVMesh";
        viewMeshFilter.mesh = viewMesh;
        visibleTargets = new List<Transform>();
        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool AngleIsGlobal=true)
    {
        if (!AngleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void FindVisibletargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRad = Physics.OverlapSphere(transform.position, ViewRadius,targetMask);

        for (int i = 0; i < targetsInViewRad.Length; i++)
        {
            Transform target = targetsInViewRad[i].transform;

            if (target == transform)
                continue;

            Vector3 dirToTarget = (target.position - transform.position).normalized;
             
            if(Vector3.Angle(target.forward,dirToTarget) < ViewAngle/2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    //no obstale can see target
                    visibleTargets.Add(target);
                }
            }
        }

    }
      
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibletargets();
        }
    }

    void DrawFieldOfView()
    {
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float setAngleSize = viewAngle / rayCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo lastCastInfo = new ViewCastInfo();
        for (int i = 0; i < rayCount; i++)
        {
            float curAngle = transform.eulerAngles.y - viewAngle / 2 + setAngleSize * i;
            ViewCastInfo newCast = ViewCast(curAngle);

            if(i> 0)
            {
                bool edgeDistThresholdExceeded = Mathf.Abs(lastCastInfo.distance - newCast.distance) > edgeDistanceThreshold;
                if (lastCastInfo.hit != newCast.hit || (lastCastInfo.hit && newCast.hit && edgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(lastCastInfo, newCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }// end if last hit != new hit
            }// end if i > 0

            viewPoints.Add(newCast.point);
            lastCastInfo = newCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount -1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            
            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i+1;
                triangles[i * 3 + 2] = i+2;
            }
        }// end for 

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;

    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle);
        RaycastHit hit;
        if(Physics.Raycast(transform.position,dir,out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius,viewRadius, globalAngle);
    }

    EdgeInfo FindEdge(ViewCastInfo min, ViewCastInfo max)
    {
        float minAngle = min.angle;
        float maxAngle = max.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveInteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2.0f;
            ViewCastInfo newCast = ViewCast(angle);
            bool edgeDistThresholdExceeded = Mathf.Abs(min.distance - newCast.distance) > edgeDistanceThreshold;
            if (newCast.hit == min.hit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }


    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}

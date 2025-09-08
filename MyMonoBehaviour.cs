using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    public bool IsObjectsAhead(float detectionRadius, float detectionDistance, LayerMask obstacleLayers)
    {
        return Physics.SphereCast(transform.position, detectionRadius, transform.forward, out RaycastHit hitInfo, detectionDistance, obstacleLayers);
    }
    public bool IsObjectsAhead(Vector3 origin, float detectionRadius, Vector3 direction, float detectionDistance, LayerMask obstacleLayers)
    {
        return Physics.SphereCast(origin, detectionRadius, direction, out RaycastHit hitInfo, detectionDistance, obstacleLayers);
    }
    public static Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MyMonoBehaviour
{
    public string editorName; // Editor only!

    [Header("Patrol:")]
    public List<Transform> targets;
    private Transform target;
    public float moveSpeed = 5f;
    public bool rotate = true;
    public float rotationSpeed = 200f;
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private readonly float detectionDistanceForTargets = 0.1f;
    public float pauseTime = 1;
    public bool isWaiting;
    //public Vector3 surfaceNormal = new(0, 1, 0);

    [Header("Obstacle Detection:")]
    public bool detect = true;
    public Vector3 detectionOrigin;
    public float detectionDistance = 2f;
    public float detectionRadius = 0.5f;
    public LayerMask obstacleLayers;
    
    [Header("Gizmos:")]
    public bool showGizmos = true;
    public Color gizmosColor = Color.red;

    private void Start()
    {
        if (target == null) target = targets[1];
    }

    private void Update()
    {
        DeffaultPatrol();
    }

    private void DeffaultPatrol()
    {
        // move:
        moveDirection = (target.position - transform.position).normalized;

        if (!isWaiting)
        {
            if (detect && IsObjectsAhead(transform.position + detectionOrigin, detectionRadius, transform.forward, detectionDistance, obstacleLayers))
                StartCoroutine(SwitchTargetAfterPause());
            else
                transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }

        // look:
        if (rotate)
        {
            lookDirection = moveDirection;
            lookDirection.y = 0f; // keep up
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // reach point:
        if (!isWaiting && Vector3.Distance(transform.position, target.position) < detectionDistanceForTargets)
            StartCoroutine(SwitchTargetAfterPause());
    }
    IEnumerator SwitchTargetAfterPause()
    {
        isWaiting = true;
        NextPoint();
        yield return new WaitForSeconds(pauseTime);
        isWaiting = false;
    }
    void NextPoint()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (target == targets[i])
            {
                if (i + 1 == targets.Count)
                    target = targets[0];
                else target = targets[i + 1];
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            // IsObjectsAhead()
            Gizmos.color = gizmosColor;
            Vector3 start = transform.position + detectionOrigin;
            Vector3 end = start + transform.forward * detectionDistance;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(end, detectionRadius);
        }
    }
}
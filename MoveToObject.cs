using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToObject : MyMonoBehaviour
{
    public string editorName; // Editor only!

    [Header("Move to Object:")]
    public Transform target;
    public float speed = 5;
    public float closeDistance = 5;
    public float distance; // to show only
    private Vector3 direction;
    public Vector3 axis = new Vector3(1, 0, 1); // insert 0 on the axis you want the object not move on

    [Space(10)]
    public float detectionDistance = 2f;
    public float detectionRadius = 0.5f;
    public LayerMask obstacleLayers;

    [Header("Add Ones:")]
    public bool lateUpdate = false;

    [Header("Gizmos:")]
    public bool showGizmos = true;
    public Color gizmosColor = Color.red;

    private void Start()
    {
        if (target == null)
        {
            target = FindAnyObjectByType<PlayerGeneral>().transform;
            //Debug.Log(gameObject.name + ": target has been asign otomaticly to player");
        }
    }

    void Update()
    {
        if (!lateUpdate) GeneralMethod();
    }

    private void LateUpdate()
    {
        if (lateUpdate) GeneralMethod();
    }

    private void GeneralMethod()
    {
        Vector3 targetPosition = Multiply(target.position, axis);
        /*float*/
        distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > closeDistance)
        {
            direction = (target.position - transform.position).normalized;
            direction = Multiply(direction, axis);

            if (!IsObjectsAhead(detectionDistance, detectionRadius, obstacleLayers))
            {
                transform.position += direction * speed * Time.deltaTime;
                //Debug.Log(gameObject.name + ": !IsObjectsAhead()");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            // IsObjectsAhead()
            Gizmos.color = gizmosColor;
            Vector3 start = transform.position;
            Vector3 end = start + transform.forward * detectionDistance;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(end, detectionRadius);
        }
    }
}

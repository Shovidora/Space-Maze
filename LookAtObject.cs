using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MyMonoBehaviour
{
    public string editorName; // Editor only!

    [Header("Look at Object:")]
    public Transform trToRotate;
    public Transform trToLookAt;
    private Vector3 direction;
    public Vector3 axis = new(1, 1, 1); // insert 0 on the axis you want the object to rotate around
    public Vector3 targetPositionAdd = Vector3.zero;
    public float sensetivity = 5;

    void Start()
    {
        if (trToRotate == null) trToRotate = transform;
        if (trToLookAt == null) trToLookAt = FindAnyObjectByType<PlayerGeneral>().centerToShootAt;
    }

    void Update()
    {
        direction = trToLookAt.position - trToRotate.position;
        if (direction != Vector3.zero)
        {
            direction = Multiply(direction, axis);
            direction += targetPositionAdd;

            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            trToRotate.rotation = Quaternion.Slerp(trToRotate.rotation, desiredRotation, sensetivity * Time.deltaTime);
        }
    }
}
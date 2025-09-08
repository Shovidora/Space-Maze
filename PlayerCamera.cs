using UnityEngine;
using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public Transform targetPos;
    public Transform targetRot;
    public Vector3 offsetPos;
    public float smoothSpeed = 20f;
    public float rotationSmoothSpeed = 20f;

    [Header("Camera offest with Up/Down look:")]
    public Vector3 offestPosStandard;
    public Vector3 offsetPosLookUp;
    public Vector3 offsetPosLookDown;
    private PlayerMovement2 pm2;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerGeneral>().transform;
        pm2 = player.GetComponent<PlayerMovement2>();

        transform.position = targetPos.position + targetPos.rotation * offsetPos;
    }

    void LateUpdate()
    {
        // Camera offest with Up/Down look:
        if (pm2.xAxisRotationPercentage > 0)
            offsetPos = Vector3.Lerp(offestPosStandard, offsetPosLookUp, pm2.xAxisRotationPercentage);
        else offsetPos = Vector3.Lerp(offestPosStandard, offsetPosLookDown, -/*-*/pm2.xAxisRotationPercentage);

        Vector3 desiredPosition = targetPos.position + targetPos.rotation * offsetPos;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        Quaternion desiredRotation = Quaternion.LookRotation(targetRot.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}
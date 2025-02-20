using UnityEngine;
using UnityEngine.InputSystem;

public class LockTarget : MonoBehaviour, ICharacterComponent
{

    [SerializeField] private Camera m_Camera;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float detectionAngle;

    public character ParentCharacter { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnLock(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius,detectionMask);
        if (detectedObjects.Length > 0) return;
        float nearestAngle = detectionAngle;
        float nearestDistance = detectionRadius;
        int closestObject = 0;

        Vector3 cameraFoward = m_Camera.transform.forward;

        for (int i = 0; i < detectedObjects.Length; i++)
        {
            Collider obj = detectedObjects[i];
            Vector3 objViewDirection = obj.transform.position - m_Camera.transform.position;
            float dot = Vector3.Dot(cameraFoward, objViewDirection.normalized);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (angle > detectionAngle) continue;
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance < nearestDistance && angle < nearestAngle)
                closestObject = i;
            nearestDistance = Mathf.Min(distance, nearestDistance);
            nearestAngle = Mathf.Min(angle, nearestAngle);
        }

        ParentCharacter.LockTarget = detectedObjects[closestObject].transform;
    }
}

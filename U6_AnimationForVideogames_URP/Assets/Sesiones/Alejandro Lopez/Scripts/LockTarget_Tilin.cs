using UnityEngine;
using UnityEngine.InputSystem;

public class LockTarget_Tilin : MonoBehaviour, ICharacterComp
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float detectionAngle;

    public Character_ALR Character { get; set; }

    public void Lock(InputAction.CallbackContext ctx)
    {
        if (ctx.started) return;

        if(Character.LockTarget != null){
            Character.LockTarget = null;
            return;
        }

        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        if(detectedObjects.Length == 0) return;

        float nearestAngle = detectionAngle;
        float nearestDistance = detectionRadius;
        int cloestesObject = 0;

        Vector3 cameraForward = mainCam.transform.forward;

        for(int i = 0; i < detectedObjects.Length; i++){
            Collider obj = detectedObjects[i];
            Vector3 objViewDir = obj.transform.position - mainCam.transform.position;
            float dot = Vector3.Dot(cameraForward, objViewDir.normalized);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if(angle > detectionAngle) continue;

            float distance = Vector3.Distance(obj.transform.position, transform.position);

            if(distance < nearestDistance && angle < nearestAngle){
                cloestesObject = i;
            }

            nearestDistance = Mathf.Min(nearestDistance, distance);
            nearestAngle = Mathf.Min(angle, nearestAngle);
        }

        Character.LockTarget = detectedObjects[cloestesObject].transform;
    }

    private void Update()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}

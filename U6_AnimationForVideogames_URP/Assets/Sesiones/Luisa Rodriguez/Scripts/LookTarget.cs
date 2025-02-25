using UnityEngine;

public class LookTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void ApplyRotation()
    {
        if(target == null) return;
        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection = Vector3.Project(lookDirection, Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = rotation;
    }
}

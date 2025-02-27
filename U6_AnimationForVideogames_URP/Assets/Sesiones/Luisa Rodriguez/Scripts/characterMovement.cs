using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rotationThresHold;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    private Quaternion targetRotation;

    private void SolveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealFoward = camera.transform.forward;
        float angleInterpolator = Mathf.Abs(f: Vector3.Dot(lhs: cameraRealFoward, rhs: floorNormal));

        Vector3 cameraFoward = Vector3.Lerp(cameraRealFoward, camera.transform.up, angleInterpolator).normalized;
        Vector3 characterFoward = Vector3.ProjectOnPlane(vector: cameraFoward, floorNormal).normalized;
        Debug.DrawLine(transform.position, transform.position + cameraFoward * 2, Color.magenta, duration: 5);

        Quaternion lookRotation = Quaternion.LookRotation(characterFoward, upwards: floorNormal);
    }

    private void ApplyCharacterRotation()
    {
        float motionMagnitude = Mathf.Sqrt(speedX.targetValue * speedX.targetValue + speedY.targetValue * speedY.targetValue);
        float rotationSpeed = Mathf.SmoothStep(0, .1f, motionMagnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
    }

    private void ApplyCharacterRotationFromAim()
    {
        Vector3 aimFoward = Vector3.ProjectOnPlane(aimTarget.forward,transform.up).normalized;
        Vector3 characterFoward = transform.forward;
        float angleCos = Vector3.Dot(characterFoward, aimFoward);
         float rotationSpeed = Mathf.SmoothStep(0f, 1f, Mathf.Acos(angleCos) * Mathf.Rad2Deg / rotationThresHold);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
    }

     public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        speedX.targetValue = inputValue.x;
        speedY.targetValue = inputValue.y;
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();
        animator.SetFloat(speedXHash, speedX.currentValue);
        animator.SetFloat(speedYHash, speedY.currentValue);
        SolveCharacterRotation();
        ApplyCharacterRotation();
    }

    public character ParentCharacter { get; set; }
}

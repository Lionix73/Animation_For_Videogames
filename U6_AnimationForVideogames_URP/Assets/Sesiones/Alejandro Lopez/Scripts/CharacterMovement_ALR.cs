using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterMovement_ALR : MonoBehaviour, ICharacterComp
{
    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rotationThreshold;

    [Header("Dampeners")]
    [SerializeField] private float angularSpeed = 180;
    [SerializeField] private FloatDamper speedX;
    [SerializeField] private FloatDamper speedY;

    private int speedXHash;
    private int speedYHash;

    [Header("Animation Settings")]
    private Quaternion lookRotation;
    private Animator animator;

    public Character_ALR Character { get; set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        speedX.TargetValue = movement.x;
        speedY.TargetValue = movement.y;
    }

    private void SoftCharacterRotation(){

        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = mainCamera.transform.forward;
        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));
        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, mainCamera.transform.up, angleInterpolator).normalized;
        Vector3 characterForward = Vector3.ProjectOnPlane(cameraForward, floorNormal).normalized;

        Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.magenta,5);

        lookRotation = Quaternion.LookRotation(characterForward, floorNormal);

    }

    private void ApplyCharacterRotation()
    {
        float motionMagnitude = Mathf.Sqrt(speedX.CurrentValue * speedX.CurrentValue + speedY.CurrentValue * speedY.CurrentValue);
        float rotationSpeed = Mathf.SmoothStep(0, 0.1f, motionMagnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, angularSpeed * rotationSpeed);
    }

    private void ApplyCharacterRotationAiming(){
        Vector3 aimForward = Vector3.ProjectOnPlane(aimTarget.forward, transform.up).normalized;
        Vector3 characterForward = transform.forward;

        float angleCos = Vector3.Dot(aimForward, characterForward);

        float rotationSpeed = Mathf.SmoothStep(0f, 1f,Mathf.Acos(angleCos)  * Mathf.Rad2Deg / rotationThreshold);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, angularSpeed * rotationSpeed);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();

        animator.SetFloat(speedXHash, speedX.CurrentValue);
        animator.SetFloat(speedYHash, speedY.CurrentValue);

        SoftCharacterRotation();

        if(!Character.IsAiming){
            ApplyCharacterRotation();
        }
        else{
            ApplyCharacterRotationAiming();
        }
    }
}

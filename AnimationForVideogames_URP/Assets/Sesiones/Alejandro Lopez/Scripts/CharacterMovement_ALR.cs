using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterMovement_ALR : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private FloatDamper speedX;
    [SerializeField] private FloatDamper speedY;

    private Quaternion lookRotation;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        speedX.TargetValue = movement.x;
        speedY.TargetValue = movement.y;

        SoftCharacterRotation();
    }

    private void SoftCharacterRotation(){

        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = mainCamera.transform.forward;
        float angleInterpolate = Mathf.Abs(Vector3.Dot(floorNormal, cameraRealForward));
        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, mainCamera.transform.up, angleInterpolate).normalized;
        Vector3 characterForward = Vector3.ProjectOnPlane(cameraForward, floorNormal).normalized;

        Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.magenta, 10f);

        lookRotation = Quaternion.LookRotation(characterForward, floorNormal);
        
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

        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 180 * Time.deltaTime);

        transform.rotation = rotation;
    }
}

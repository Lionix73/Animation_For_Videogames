using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;
    [SerializeField] private float angularSpeed;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    private Quaternion targetRotation;

    private void solveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealFoward = camera.transform.forward;
        float angleInterpolator = Mathf.Abs(f: Vector3.Dot(lhs: cameraRealFoward, rhs: floorNormal));

        Vector3 cameraFoward = Vector3.Lerp(cameraRealFoward, camera.transform.up, angleInterpolator).normalized;
        Vector3 characterFoward = Vector3.ProjectOnPlane(vector: cameraFoward, floorNormal).normalized;
        Debug.DrawLine(transform.position, transform.position + cameraFoward * 2, Color.magenta, duration: 5);

        Quaternion lookRotation = Quaternion.LookRotation(characterFoward, upwards: floorNormal);
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 InputValue = ctx.ReadValue<Vector2>();
        speedX.targetValue = InputValue.x;
        speedY.targetValue = InputValue.y;

        if (InputValue.magnitude > .1f)
        {
            solveCharacterRotation();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash(name:"SpeedX");
        speedYHash = Animator.StringToHash(name:"SpeedY");
    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();

        animator.SetFloat(speedXHash, speedX.currentValue);
        animator.SetFloat(speedYHash, speedY.currentValue);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * Time.deltaTime);

    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook_ALR : MonoBehaviour, ICharacterComp
{
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float horizontalRotationSpeed = 1.0f;
    [SerializeField] private float verticalRotationSpeed = 1.0f;
    [SerializeField] private Vector2 rotationLimits = new Vector2(-90, 90);

    [Header("Dampeners")]
    [SerializeField] private FloatDamper horizontalDampener;
    [SerializeField] private FloatDamper verticalDampener;

    float verticalRotation;

    public Character_ALR Character { get; set; }

    public void OnLook(InputAction.CallbackContext ctx){
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        inputValue = inputValue / new Vector2(Screen.width, Screen.height);

        horizontalDampener.TargetValue = inputValue.x;
        verticalDampener.TargetValue = inputValue.y;
    }

    private void ApplyLookRotation()
    {
        if(target == null) {
            throw new NullReferenceException("Target is null");
        }

        if(Character.LockTarget != null){
            target = Character.LockTarget;

            Vector3 lookDir = (Character.LockTarget.position - target.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            transform.rotation = rotation;
            return;
        }

        target.RotateAround(target.position, transform.up, horizontalDampener.CurrentValue * horizontalRotationSpeed * Time.deltaTime);
        //Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDampener.CurrentValue * horizontalRotationSpeed * Time.deltaTime, transform.up);
        //target.rotation *= horizontalRotation;
        verticalRotation += verticalDampener.CurrentValue * verticalRotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, rotationLimits.x, rotationLimits.y);

        Vector3 euler = target.localEulerAngles;
        euler.x = verticalRotation;
        target.localEulerAngles = euler;
    }

    private void Update()
    {
        horizontalDampener.Update();
        verticalDampener.Update();

        ApplyLookRotation();
    }
}

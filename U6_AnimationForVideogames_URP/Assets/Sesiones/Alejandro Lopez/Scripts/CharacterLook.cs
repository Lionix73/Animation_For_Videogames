using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [SerializeField] private float horizontalRotationSpeed = 1.0f;
    [SerializeField] private float verticalRotationSpeed = 1.0f;

    [Header("Dampeners")]
    [SerializeField] private FloatDamper horizontalDampener;
    [SerializeField] private FloatDamper verticalDampener;

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

        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDampener.CurrentValue * horizontalRotationSpeed, transform.up);
        transform.rotation *= horizontalRotation;

    }

    private void Update()
    {
        horizontalDampener.Update();
        verticalDampener.Update();

        ApplyLookRotation();
    }
}

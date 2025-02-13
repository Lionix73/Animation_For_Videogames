using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterLoook : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private FloatDampener horizontalDampener;
    [SerializeField] private FloatDampener verticalDampener;

    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float verticalRotationSpeed;

    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();

        inputValue = inputValue / new Vector2(Screen.width, Screen.height);
        horizontalDampener.targetValue = inputValue.x;
        verticalDampener.targetValue = inputValue.y;

    }

    private void ApplylookRotation()
    {
        if (target == null)
        {
            throw new NullReferenceException("Look target is null, assign it in the inspector");
        }

        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDampener.currentValue * horizontalRotationSpeed, transform.up);
        transform.rotation = horizontalRotation;
    }

    private void Update()
    {
        horizontalDampener.Update();
        verticalDampener.Update();
        ApplylookRotation();
    }
}

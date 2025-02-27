using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterLoook : MonoBehaviour, ICharacterComponent
{

   [SerializeField] private Transform target;

    [SerializeField] private FloatDampener horizontalDampener;
    [SerializeField] private FloatDampener verticalDampener;

    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private Vector2 verticalRotationLimits;

    private float verticalRotation;

    [field:SerializeField]public character ParentCharacter { get; set; }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        inputValue = inputValue / new Vector2(Screen.width, Screen.height);
        horizontalDampener.targetValue = inputValue.x;
        verticalDampener.targetValue = inputValue.y;
    }

    private void ApplyLookRotation()
    {
        
        if (target == null)
        {
            throw new NullReferenceException("Look target is null, assign it in inspector");
        }
        
        if (ParentCharacter.LockTarget != null)
        {
            Vector3 lookDirection = (ParentCharacter.LockTarget.position - target.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            target.rotation = rotation;
            return;
        }

        #warning Reset rotation while locked
        target.RotateAround(target.position, transform.up, horizontalDampener.currentValue * horizontalRotationSpeed * 360 * Time.deltaTime);
        verticalRotation += verticalDampener.currentValue * verticalRotationSpeed * 360 * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalRotationLimits.x, verticalRotationLimits.y);
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

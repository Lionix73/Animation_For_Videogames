using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class TilinAim : MonoBehaviour, ICharacterComp
{
    [SerializeField] private CinemachineCamera aimingCamera;
    [SerializeField] private AimConstraint aimConstraint;
    [SerializeField] private FloatDamper aimDampener;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        if(!ctx.started && !ctx.canceled) return;
        aimingCamera?.gameObject.SetActive(ctx.started);
        Character.IsAiming = ctx.started;
        aimConstraint.enabled = ctx.started;
        aimDampener.TargetValue = ctx.started ? 1 : 0;

        /*
        if(ctx.started){
            //Aim
            aimingCamera?.gameObject.SetActive(true);
            Character.IsAiming = true;
            aimConstraint.enabled = true;
            aimDampener.TargetValue = 1;
            aimConstraint.weight = 1;
            anim.SetLayerWeight(1, 1);
        }

        if(ctx.canceled){
            //Stop aiming
            aimingCamera?.gameObject.SetActive(false);
            Character.IsAiming = false;
            aimConstraint.enabled = false;
            aimDampener.TargetValue = 0;
            aimConstraint.weight = 0;
            anim.SetLayerWeight(1, 0);
        }
        */
    }

    public void Update()
    {
        aimDampener.Update();
        aimConstraint.weight = aimDampener.CurrentValue;
        anim.SetLayerWeight(1, aimDampener.CurrentValue);
    }

    public Character_ALR Character { get; set; }
}

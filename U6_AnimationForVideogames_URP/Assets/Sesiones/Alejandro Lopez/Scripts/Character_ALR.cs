using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Character_ALR : MonoBehaviour
{
    private Transform lockTarget;
    private bool isAiming;

    public Transform LockTarget { get => lockTarget; set => lockTarget = value; }
    public bool IsAiming { get => isAiming; set => isAiming = value; }

    private void RegisterComp(){
        foreach (ICharacterComp comp in GetComponentsInChildren<ICharacterComp>())
        {
            comp.Character = this;
        }
    }

    private void Awake()
    {
        RegisterComp();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

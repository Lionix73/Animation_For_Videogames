using UnityEngine;

public class character : MonoBehaviour
{
    Transform lockTarget;
    [SerializeField]private bool isAiming;

    public Transform LockTarget
    {
        get => lockTarget;
        set => lockTarget = value;
    }

    public bool IsAiming
    {
        get => isAiming;
        set => isAiming = value;
    }

    private void RegisterComponents()
    {
        foreach (ICharacterComponent characterComponent in GetComponentsInChildren<ICharacterComponent>())
        {
            characterComponent.ParentCharacter = this;
        }
    }

    private void Awake()
    {
        RegisterComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }
}

using UnityEngine;

public class character : MonoBehaviour
{
    [SerializeField] Transform lockTarget;

    public Transform LockTarget
    {
        get => lockTarget;
        set => lockTarget = value;
    }

    private void RegisterComponent()
    {
        foreach(ICharacterComponent characterComponent in GetComponentsInChildren<ICharacterComponent>())
        {
            characterComponent.ParentCharacter = this;
        }
    }

    private void Awake()
    {
        RegisterComponent();
    }
}

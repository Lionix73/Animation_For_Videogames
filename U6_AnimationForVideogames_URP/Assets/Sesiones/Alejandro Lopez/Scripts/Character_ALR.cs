using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Character_ALR : MonoBehaviour
{
    [SerializeField] private Transform lockTarget;
    public Transform LockTarget { get => lockTarget; set => lockTarget = value; }

    private void RegisterComp(){
        foreach (ICharacterComp comp in GetComponentsInChildren<ICharacterComp>())
        {
            comp.Character = this;
        }
    }

    private void Awake()
    {
        RegisterComp();
    }
}

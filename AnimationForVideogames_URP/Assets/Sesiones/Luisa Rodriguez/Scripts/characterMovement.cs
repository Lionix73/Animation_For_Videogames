using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash(name:"SpeedX");
        speedYHash = Animator.StringToHash(name:"SpeedY");
    }

    private void Update()
    {
        if (animator == null) return;
    
        animator.SetFloat(name:"SpeedX", speedX);
        animator.SetFloat(name:"SpeedY", speedY);
    }
}

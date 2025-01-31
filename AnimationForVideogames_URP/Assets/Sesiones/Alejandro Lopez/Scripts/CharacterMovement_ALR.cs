using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterMovement_ALR : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }

    private void Update()
    {
        animator.SetFloat(speedXHash, speedX);
        animator.SetFloat(speedYHash, speedY);
    }
}

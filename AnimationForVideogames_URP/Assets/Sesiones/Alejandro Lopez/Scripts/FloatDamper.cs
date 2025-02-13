using System;
using UnityEngine;

[Serializable]
public struct FloatDamper
{
    public float CurrentValue {get; private set;}
    public float TargetValue { get; set; }
    private float currentVelocity;
    [SerializeField] private float smoothTime;

    public void Update()
    {
        CurrentValue = Mathf.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, smoothTime);
    }
}

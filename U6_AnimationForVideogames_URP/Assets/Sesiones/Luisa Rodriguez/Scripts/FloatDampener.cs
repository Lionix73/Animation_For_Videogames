using System;
using UnityEngine;

[Serializable]
public struct FloatDampener
{
    [SerializeField] private float smoothTime;

    private float currentVelocity;

    public float targetValue { get; set; }
    public float currentValue {get; private set;}

    public void Update()
    {
        currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref currentVelocity, smoothTime);
    }

}

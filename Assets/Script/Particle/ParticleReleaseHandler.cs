using System;
using UnityEngine;

public class ParticleReleaseHandler : MonoBehaviour
{
    public event Action OnParticleRelease;

    private float lifeTime = 0.75f;
    private float flowTime = 0f;

    public float LifeTime { get; set; }

    private void OnEnable()
    {
        flowTime = 0f;
    }

    private void Update()
    {
        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            OnParticleRelease?.Invoke();
            OnParticleRelease = null;
            flowTime = 0f;
        }
    }
}
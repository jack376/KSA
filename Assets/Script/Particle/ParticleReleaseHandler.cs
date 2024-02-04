using System;
using UnityEngine;

public class ParticleReleaseHandler : MonoBehaviour
{
    public event Action OnParticleRelease;

    private readonly float lifeTime = 0.75f;
    private float flowTime = 0f;

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
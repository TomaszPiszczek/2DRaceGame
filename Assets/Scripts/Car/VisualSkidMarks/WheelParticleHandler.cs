using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    private float particleEmissionRate = 0;

    private TopDownCarController topDownCarController;
    private ParticleSystem particleSystemSmoke;
    private ParticleSystem.EmissionModule particleSystemEmissionModule;
    private ParticleSystem.MainModule particleSystemMainModule;

    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponent<ParticleSystem>();
        
        if (particleSystemSmoke != null)
        {
            particleSystemMainModule = particleSystemSmoke.main;
            particleSystemEmissionModule = particleSystemSmoke.emission;
            particleSystemEmissionModule.rateOverTime = 0;
        }
        else
        {
            Debug.LogError("ParticleSystem not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (particleSystemSmoke == null || topDownCarController == null)
            return;

        // Gradually reduce particle emission
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        // Set particle color based on surface type
        switch (topDownCarController.GetSurface())
        {
            case Surface.SurfaceTypes.Road:
                particleSystemMainModule.startColor = new Color(0.83f, 0.83f, 0.83f);
                break;

            case Surface.SurfaceTypes.Sand:
                particleSystemMainModule.startColor = new Color(0.64f, 0.42f, 0.24f);
                break;

            case Surface.SurfaceTypes.Grass:
                particleSystemMainModule.startColor = new Color(0.15f, 0.4f, 0.13f);
                break;

            case Surface.SurfaceTypes.Oil:
                particleSystemMainModule.startColor = new Color(0.2f, 0.2f, 0.2f);
                break;
        }

        // Check if tires are screeching and adjust particle emission
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
        {
            if (isBreaking)
                particleEmissionRate = 30;
            else
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}
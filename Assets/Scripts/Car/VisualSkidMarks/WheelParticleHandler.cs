using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
 float particleEmissionRate = 0;

 TopDownCarController topDownCarController;
 ParticleSystem particleSystemSmoke;

 ParticleSystem.EmissionModule particleSystemEmissionModule;
 ParticleSystem.MainModule particleSystemMainModule;

 private void Awake() {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponentInParent<ParticleSystem>();
        particleSystemMainModule = particleSystemSmoke.main;

        particleSystemEmissionModule = particleSystemSmoke.emission;
        particleSystemEmissionModule.rateOverTime = 0;
    
   }

   void Update()
   {
    particleEmissionRate = Mathf.Lerp(particleEmissionRate,0,Time.deltaTime *5);
    particleSystemEmissionModule.rateOverTime = particleEmissionRate;

    switch(topDownCarController.GetSurface())
    {
        case Surface.SurfaceTypes.Road:
            particleSystemMainModule.startColor = new Color(0.83f,0.83f,0.83f);
            break;

        case Surface.SurfaceTypes.Sand:
            particleSystemMainModule.startColor = new Color(0.64f,0.42f,0.24f);
            break;
        case Surface.SurfaceTypes.Grass:
            particleSystemMainModule.startColor = new Color(0.15f,0.4f,0.13f);
            break;
         case Surface.SurfaceTypes.Oil:
            particleSystemMainModule.startColor = new Color(0.2f,0.2f,0.2f);
            break;


    }


    if(topDownCarController.IsTireScreeching(out float lateralVelovity,out bool isBreaking))
    {
        if(isBreaking)
            particleEmissionRate =30;
        else
            particleEmissionRate = Mathf.Abs(lateralVelovity)*2;
    }


   }
}

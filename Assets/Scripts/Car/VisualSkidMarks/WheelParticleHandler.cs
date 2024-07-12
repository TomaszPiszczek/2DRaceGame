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

 private void Awake() {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponentInParent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission;
        particleSystemEmissionModule.rateOverTime = 0;
    
   }

   void Update()
   {
    particleEmissionRate = Mathf.Lerp(particleEmissionRate,0,Time.deltaTime *5);
    particleSystemEmissionModule.rateOverTime = particleEmissionRate;


    if(topDownCarController.IsTireScreecing(out float lateralVelovity,out bool isBreaking))
    {
        if(isBreaking)
            particleEmissionRate =30;
        else
            particleEmissionRate = Mathf.Abs(lateralVelovity)*2;
    }


   }
}

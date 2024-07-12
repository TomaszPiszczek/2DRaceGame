using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderHandler : MonoBehaviour
{
   TopDownCarController TopDownCarController;
   TrailRenderer trailRenderer;
   
    private void Awake() {
        TopDownCarController = GetComponentInParent<TopDownCarController>();

        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    
   }

   private void Update() {

    if(TopDownCarController.IsTireScreecing(out float lateralVelovity,out bool isBreaking))
    {
        trailRenderer.emitting = true;
    }
    else trailRenderer.emitting = false;
    
   }




}

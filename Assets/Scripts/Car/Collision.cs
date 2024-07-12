using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

     private TopDownCarController speedSetup; 

    private void Start()
    {
        speedSetup = GetComponent<TopDownCarController>();
    }
    void  OnCollisionEnter2D() {
    Debug.Log("Speed" + speedSetup.velocityVsUp);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

     private SpeedSetup speedSetup; 

    private void Start()
    {
        speedSetup = GetComponent<SpeedSetup>();
    }
    void  OnCollisionEnter2D() {
    Debug.Log("Speed" + speedSetup.velocityVsUp);
   }
}

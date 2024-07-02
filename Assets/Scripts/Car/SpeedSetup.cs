using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSetup : MonoBehaviour
{
  
    [SerializeField] float turnFactor  = 4;
    [SerializeField] float accelertaionFactor = 10f;

    [SerializeField] float rotationAngle = 0;
    [SerializeField] float maxSpeed = 15;
    [SerializeField] float tireFriction = 0.95f;

 
    //Components
    Rigidbody2D rigidbody2D;


    public float velocityVsUp =0;
    float speedAmount=0;
    float steerAmount =0;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        speedUpdate(Input.GetAxisRaw("Vertical"));
        killOrthofonalVelocity();

        applyStering(Input.GetAxisRaw("Horizontal"));





        
    }


    private void speedUpdate(float input){
        velocityVsUp = Vector2.Dot(transform.up,rigidbody2D.velocity);

        if(velocityVsUp > maxSpeed && input >0) return;
        if(velocityVsUp < -maxSpeed * 0.5f && input <0){
            return;
        } 
        if(rigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && input >0) return;



        if(input ==0){
            rigidbody2D.drag = Mathf.Lerp(rigidbody2D.drag,3.0f,Time.fixedDeltaTime*3);

            Debug.Log(input);
        }else{
            rigidbody2D.drag = 0;

        }
      

    Vector2 engineForceVector = transform.up * input * accelertaionFactor;

    rigidbody2D.AddForce(engineForceVector,ForceMode2D.Force);

    
    }

    void applyStering(float input){
        float minSpeedBeforeAllowTurningFactor = (rigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        if(velocityVsUp > 0 ){
        rotationAngle -= input * turnFactor * minSpeedBeforeAllowTurningFactor;
        }else{
             rotationAngle += input * turnFactor * minSpeedBeforeAllowTurningFactor;
        }
        rigidbody2D.MoveRotation(rotationAngle);


    }
    void killOrthofonalVelocity(){
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidbody2D.velocity,transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody2D.velocity,transform.right);

        rigidbody2D.velocity = forwardVelocity + rightVelocity * tireFriction;
    }
}
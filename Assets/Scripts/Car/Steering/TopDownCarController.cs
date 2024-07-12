using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
  
    [SerializeField] float turnFactor  = 4;
    [SerializeField] float accelertaionFactor = 10f;

    [SerializeField] float rotationAngle = 0;
    [SerializeField] float maxSpeed = 15;
    [SerializeField] float tireFriction = 0.95f;

 
    //Components
    Rigidbody2D carRigidBody2D;


    public float velocityVsUp =0;
    float speedAmount=0;
    float steerAmount =0;

    float accelerationInput=0;
    void Start()
    {
        carRigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        accelerationInput = Input.GetAxisRaw("Vertical");
        speedUpdate( accelerationInput);

        
        killOrthofonalVelocity();

        applyStering(Input.GetAxisRaw("Horizontal"));





        
    }


    private void speedUpdate(float input){
        velocityVsUp = Vector2.Dot(transform.up,carRigidBody2D.velocity);

        if(velocityVsUp > maxSpeed && input >0) return;
        if(velocityVsUp < -maxSpeed * 0.5f && input <0){
            return;
        } 
        if(carRigidBody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && input >0) return;



        if(input ==0){
            carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag,3.0f,Time.fixedDeltaTime*3);

            Debug.Log(input);
        }else{
            carRigidBody2D.drag = 0;

        }
      

    Vector2 engineForceVector = transform.up * input * accelertaionFactor;

    carRigidBody2D.AddForce(engineForceVector,ForceMode2D.Force);

    
    }

    void applyStering(float input){
        float minSpeedBeforeAllowTurningFactor = (carRigidBody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        if(velocityVsUp > 0 ){
        rotationAngle -= input * turnFactor * minSpeedBeforeAllowTurningFactor;
        }else{
             rotationAngle += input * turnFactor * minSpeedBeforeAllowTurningFactor;
        }
        carRigidBody2D.MoveRotation(rotationAngle);


    }
    void killOrthofonalVelocity(){
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidBody2D.velocity,transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidBody2D.velocity,transform.right);

        carRigidBody2D.velocity = forwardVelocity + rightVelocity * tireFriction;
    }

    float getLateralVelocity()
    {
        return Vector2.Dot(transform.right,carRigidBody2D.velocity);
    }

    public bool IsTireScreecing(out float lateralVelovity,out bool isBreaking)
    {
        lateralVelovity = getLateralVelocity();
        isBreaking = false;


        if(accelerationInput < 0 && velocityVsUp >0)
        {
            isBreaking = true;
            return true;

        }
        if(Math.Abs(getLateralVelocity()) > 4.0f)
        {
            return true;
        }

        return false;
    }
}
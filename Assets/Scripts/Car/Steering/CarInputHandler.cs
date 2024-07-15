using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;

    public bool isUiInput = false;
    public int playerNumber =0;
    Vector2 inputVector = Vector2.zero;



    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }


     private void Update() {


        if(isUiInput)
        {
           
        }
        else
        {
        inputVector = Vector2.zero;
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");


        }
        topDownCarController.SetInputVector(inputVector);

      
    }

    public void SetInput(Vector2 newInput)
    {
        inputVector =  newInput;
    }

}

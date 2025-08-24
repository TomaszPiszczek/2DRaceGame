using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    public int playerNumber = 1;
    public bool isUIInput = false;
    
    private float steeringInput = 0;
    private float accelerationInput = 0;
    private Vector2 inputVector = Vector2.zero;

    // Components
    private TopDownCarController topDownCarController;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame and is frame dependent
    void Update()
    {
        if (isUIInput)
        {
            // Handle UI input if needed
        }
        else
        {
            inputVector = Vector2.zero;

            switch (playerNumber)
            {
                case 1:
                    // Get input from Unity's input system.
                    inputVector.x = Input.GetAxis("Horizontal");
                    inputVector.y = Input.GetAxis("Vertical");
                    break;
             
            }
        }

        // Store the input values
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;

        // Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }

    // Method for UI buttons or external systems to set input
    public void SetInput(Vector2 newInput)
    {
        inputVector = newInput;
        steeringInput = newInput.x;
        accelerationInput = newInput.y;
        
        // Apply the input to the car controller
        topDownCarController.SetInputVector(newInput);
    }

    // Method to retrieve the current input vector
    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    // Individual getters
    public float GetSteeringInput()
    {
        return steeringInput;
    }

    public float GetAccelerationInput()
    {
        return accelerationInput;
    }
}
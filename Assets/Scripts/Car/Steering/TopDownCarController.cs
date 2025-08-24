using System.Collections;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    [Header("Sprites")]
    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;

    // Local variables
    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private float velocityVsUp = 0;

    // Components
    private Rigidbody2D carRigidbody2D;
    private Collider2D carCollider;
    private MonoBehaviour audioHandler;
    private CarSurfaceHandler carSurfaceHandler;
    private CarJumpHandler carJumpHandler; // Reference to the jump handler

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
        
        // Try to get CarSFXHandler first, then fallback to SFX if it exists
        audioHandler = GetComponent<CarSFXHandler>();
        if (audioHandler == null)
        {
            audioHandler = GetComponent<SFX>();
        }
        
        carSurfaceHandler = GetComponent<CarSurfaceHandler>();
        carJumpHandler = GetComponent<CarJumpHandler>(); // Get the CarJumpHandler
    }

    void Start()
    {
        rotationAngle = transform.rotation.eulerAngles.z;
        
        // Verify required components are assigned
        if (carSpriteRenderer == null)
        {
            Debug.LogError("Car Sprite Renderer is not assigned in the inspector!");
        }
        
        if (carShadowRenderer == null)
        {
            Debug.LogError("Car Shadow Renderer is not assigned in the inspector!");
        }
        
        if (carJumpHandler == null)
        {
            Debug.LogError("CarJumpHandler component is not attached to this GameObject!");
        }
    }

    // Frame-rate independent for physics calculations.
    void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    void ApplyEngineForce()
    {
        // Don't let the player brake while in the air, but we still allow some drag so it can be slowed slightly. 
        if (IsJumping() && accelerationInput < 0)
            accelerationInput = 0;

        // Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 0, Time.fixedDeltaTime * 10);

        // Apply more drag depending on surface
        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Sand:
                carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 9.0f, Time.fixedDeltaTime * 3);
                break;

            case Surface.SurfaceTypes.Grass:
                carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 10.0f, Time.fixedDeltaTime * 3);
                break;

            case Surface.SurfaceTypes.Oil:
                carRigidbody2D.drag = 0;
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
        }

        // Calculate how much "forward" we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        // Limit so we cannot go faster than the max speed in the "forward" direction
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        // Limit so we cannot go faster than the 50% of max speed in the "reverse" direction
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        // Limit so we cannot go faster in any direction while accelerating
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0 && !IsJumping())
            return;

        // Create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        // Apply force and pushes the car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        // Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply steering by rotating the car object
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        // Get forward and right velocity of the car
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        float currentDriftFactor = driftFactor;

        // Apply more drag depending on surface
        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Sand:
                currentDriftFactor *= 1.05f;
                break;

            case Surface.SurfaceTypes.Oil:
                currentDriftFactor = 1.00f;
                break;
        }
        if (IsJumping())
            currentDriftFactor = 1;

        // Kill the orthogonal velocity (side velocity) based on how much the car should drift. 
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * currentDriftFactor;
    }

    float GetLateralVelocity()
    {
        // Returns how how fast the car is moving sideways. 
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (IsJumping())
            return false;

        // Check if we are moving forward and if the player is hitting the brakes. In that case the tires should screech.
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        // If we have a lot of side movement then the tires should be screeching
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }

    public Surface.SurfaceTypes GetSurface()
    {
        if (carSurfaceHandler != null)
            return carSurfaceHandler.GetCurrentSurface();
        
        // Default surface if carSurfaceHandler is not available
        return Surface.SurfaceTypes.Road;
    }

    // Method to check if the car is jumping - delegates to the jump handler
    public bool IsJumping()
    {
        if (carJumpHandler != null)
            return carJumpHandler.IsJumping();
        return false;
    }

    // Detect Jump trigger
    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Jump") && carJumpHandler != null)
        {
            // Get the jump data from the jump
            JumpData jumpData = collider2d.GetComponent<JumpData>();
            if (jumpData != null)
            {
                // Delegate to CarJumpHandler
                carJumpHandler.Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
            }
            else
            {
                Debug.LogWarning("Jump trigger doesn't have JumpData component");
                // Use default values if no JumpData component is found
                carJumpHandler.Jump(1.0f, 1.0f);
            }
        }
    }
}
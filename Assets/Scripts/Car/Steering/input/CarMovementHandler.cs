using UnityEngine;

public class CarMovementHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    private float steeringInput = 0;
    private float accelerationInput = 0;
    private float rotationAngle = 0;
    private float velocityVsUp = 0;

    private Rigidbody2D carRigidbody2D;

    private void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rotationAngle = transform.rotation.eulerAngles.z;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public void UpdateMovement(bool isJumping, Surface.SurfaceTypes currentSurface)
    {
        ApplyEngineForce(isJumping, currentSurface);
        KillOrthogonalVelocity(isJumping, currentSurface);
        ApplySteering();
    }

    private void ApplyEngineForce(bool isJumping, Surface.SurfaceTypes currentSurface)
    {
        // Don't let the player brake while in the air
        if (isJumping && accelerationInput < 0)
            accelerationInput = 0;

        // Apply drag if there is no accelerationInput so the car stops
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else 
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 0, Time.fixedDeltaTime * 10);

        // Apply more drag depending on surface
        switch (currentSurface)
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

        // Forward speed limit
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        // Reverse speed limit (50% of max speed)
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        // Overall speed limit
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0 && !isJumping)
            return;

        // Create a force for the engine and apply it
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        // Limit the car's ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply steering by rotating the car object
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity(bool isJumping, Surface.SurfaceTypes currentSurface)
    {
        // Get forward and right velocity of the car
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        float currentDriftFactor = driftFactor;

        // Apply more drag depending on surface
        switch (currentSurface)
        {
            case Surface.SurfaceTypes.Sand:
                currentDriftFactor *= 1.05f;
                break;

            case Surface.SurfaceTypes.Oil:
                currentDriftFactor = 1.00f;
                break;
        }
        
        if (isJumping)
            currentDriftFactor = 1;

        // Kill the orthogonal velocity (side velocity) based on how much the car should drift
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * currentDriftFactor;
    }

    private float GetLateralVelocity()
    {
        // Returns how fast the car is moving sideways
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        // Don't screech while jumping
        if (GetComponent<CarJumpHandler>().IsJumping())
            return false;

        // Check if we are moving forward and hitting the brakes
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

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }
}
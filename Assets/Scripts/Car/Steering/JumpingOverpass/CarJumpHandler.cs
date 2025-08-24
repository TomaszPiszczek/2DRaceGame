using System.Collections;
using UnityEngine;

public class CarJumpHandler : MonoBehaviour
{
    [Header("Jump Settings")]
    public AnimationCurve jumpCurve;
    public ParticleSystem landingParticleSystem;
    // Adding a small delay before allowing another jump
    public float jumpCooldown = 0.5f;
    // Adding parameters to control collision detection
    public float collisionCheckRadius = 0.8f;
    // Adding layer mask to control what obstacles we detect
    public LayerMask landingCollisionMask;

    [Header("Visual References")]
    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;

    private bool isJumping = false;
    private Rigidbody2D carRigidbody2D;
    private Collider2D carCollider;
    private float lastJumpTime = -999f;
    
    // Audio component reference - using a more generic approach to handle different audio handlers
    private MonoBehaviour audioHandler;

    private void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
        
        // Try to get CarSFXHandler first, then fallback to SFX if it exists
        audioHandler = GetComponent<CarSFXHandler>();
        if (audioHandler == null)
        {
            audioHandler = GetComponent<SFX>();
        }
    }

    private void Start()
    {
        // Verify required components are assigned
        if (carSpriteRenderer == null)
        {
            Debug.LogError("Car Sprite Renderer is not assigned in the inspector!");
        }
        
        if (carShadowRenderer == null)
        {
            Debug.LogError("Car Shadow Renderer is not assigned in the inspector!");
        }
        
        if (jumpCurve == null)
        {
            Debug.LogError("Jump Curve is not assigned in the inspector!");
        }
        
        if (landingParticleSystem == null)
        {
            Debug.LogWarning("Landing Particle System is not assigned in the inspector!");
        }
        
        // Set default collision mask if none is specified
        if (landingCollisionMask.value == 0)
        {
            landingCollisionMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
            // Exclude the ObjectFlying layer from collision checks
            landingCollisionMask &= ~(1 << LayerMask.NameToLayer("ObjectFlying"));
        }
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        // Add cooldown to prevent rapid consecutive jumps
        if (!isJumping && Time.time > lastJumpTime + jumpCooldown)
        {
            lastJumpTime = Time.time;
            int carColliderLayerBeforeJump = carCollider.gameObject.layer;
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale, carColliderLayerBeforeJump));
        }
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale, int carColliderLayerBeforeJump)
    {
        // Safety check for required components
        if (carSpriteRenderer == null || carShadowRenderer == null || jumpCurve == null)
        {
            Debug.LogError("Missing required components for jump. Make sure carSpriteRenderer, carShadowRenderer, and jumpCurve are assigned.");
            yield break;
        }
        
        Vector3 baseScale = carSpriteRenderer.transform.localScale;
        baseScale.x = 0.6f;
        baseScale.y = 0.6f;

        isJumping = true;

        float jumpStartTime = Time.time;
        float jumpDuration = carRigidbody2D.velocity.magnitude * 0.05f;
        jumpDuration = Mathf.Clamp(jumpDuration, 0.3f, 2.0f); // Ensure minimum and maximum jump duration

        jumpHeightScale = jumpHeightScale * carRigidbody2D.velocity.magnitude * 0.05f;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.1f, 1.0f);

        // Change the layer of the car to flying
        carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectFlying");

        // Play jump sound
        PlayJumpSound();

        // Change sorting layer to flying
        carSpriteRenderer.sortingLayerName = "Flying";
        carShadowRenderer.sortingLayerName = "Flying";

        // Push the car forward as we jumped
        carRigidbody2D.AddForce(carRigidbody2D.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        while (isJumping)
        {
            // Percentage 0 - 1.0 of where we are in the jumping process
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);

            // Scale the car based on jump curve
            carSpriteRenderer.transform.localScale = baseScale + baseScale * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            // Scale the shadow (smaller than car)
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale * 0.75f;

            // Offset the shadow
            carShadowRenderer.transform.localPosition = new Vector3(1, -1, 0.0f) * 3 * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            // When we reach 100% we are done
            if (jumpCompletedPercentage == 1.0f)
                break;

            yield return null;
        }

        // Wait a small amount of time before checking for landing collision
        yield return new WaitForSeconds(0.05f);

        // Perform the landing check
        bool canLandSafely = CanLandSafely();

        // Handle landing based on collision check
        if (!canLandSafely)
        {
            // Debug visualization for the area we're checking
            Debug.DrawRay(transform.position, Vector3.up * 2, Color.red, 1.0f);
            
            // Something is blocking landing, so we'll give a small bounce
            isJumping = false;
            
            // Add a small jump, but with diminishing height to eventually let the car land
            Jump(0.2f, 0.3f);
        }
        else
        {
            FinishLanding(baseScale, carColliderLayerBeforeJump, jumpHeightScale);
        }
    }
    
    // New method to check if landing is possible
    private bool CanLandSafely()
    {
        // Temporarily disable the car collider so we don't detect ourselves
        carCollider.enabled = false;
        
        // Set up a contact filter that excludes triggers
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(landingCollisionMask);
        contactFilter.useLayerMask = true;
        
        // Use a smaller radius for the collision check to avoid false positives
        Collider2D[] hitResults = new Collider2D[4];
        int numberOfHitObjects = Physics2D.OverlapCircle(transform.position, collisionCheckRadius, contactFilter, hitResults);
        
        // Re-enable the car collider
        carCollider.enabled = true;
        
        // If we hit any objects that would block our landing, return false
        return numberOfHitObjects == 0;
    }
    
    // New method to handle the landing completion
    private void FinishLanding(Vector3 baseScale, int originalLayer, float jumpHeightScale)
    {
        // Handle landing, scale back the object
        carSpriteRenderer.transform.localScale = baseScale;
        
        // Reset the shadow's position and scale
        carShadowRenderer.transform.localPosition = Vector3.zero;
        carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale;
        
        // Restore the original collision layer
        carCollider.gameObject.layer = originalLayer;
        
        // Change sorting layer back to regular
        carSpriteRenderer.sortingLayerName = "Car";
        carShadowRenderer.sortingLayerName = "Car";
        
        // Play landing effects for bigger jumps
        if (jumpHeightScale > 0.2f)
        {
            if (landingParticleSystem != null)
            {
                landingParticleSystem.Play();
            }
            PlayLandingSound();
        }
        
        // We're no longer jumping
        isJumping = false;
    }
    
    // Helper methods to handle sound playback regardless of which audio component is used
    private void PlayJumpSound()
    {
        if (audioHandler != null)
        {
            // Try to call PlayJumpSfx on either CarSFXHandler or SFX component
            var carSfxHandler = audioHandler as CarSFXHandler;
            if (carSfxHandler != null)
            {
                carSfxHandler.PlayJumpSfx();
                return;
            }
            
            var sfx = audioHandler as SFX;
            if (sfx != null)
            {
                sfx.PlayJumpSfx();
                return;
            }
        }
        
        // If no sound handler, just log a warning
        Debug.LogWarning("No audio component found to play jump sound");
    }
    
    private void PlayLandingSound()
    {
        if (audioHandler != null)
        {
            // Try to call PlayLandingSfx on either CarSFXHandler or SFX component
            var carSfxHandler = audioHandler as CarSFXHandler;
            if (carSfxHandler != null)
            {
                carSfxHandler.PlayLandingSfx();
                return;
            }
            
            var sfx = audioHandler as SFX;
            if (sfx != null)
            {
                sfx.PlayLandingSfx();
                return;
            }
        }
        
        // If no sound handler, just log a warning
        Debug.LogWarning("No audio component found to play landing sound");
    }
}
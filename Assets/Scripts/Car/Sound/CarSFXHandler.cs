using UnityEngine;

public class CarSFXHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource engineAudioSource;
    public AudioSource tireScreechAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource landingAudioSource;

    [Header("Audio Clips")]
    public AudioClip engineClip;
    public AudioClip tireScreechClip;
    public AudioClip jumpClip;
    public AudioClip landingClip;

    private CarMovementHandler movementHandler;
    private float desiredEnginePitch = 0.5f;
    private float tireScratchTime = 0;

    private void Awake()
    {
        movementHandler = GetComponent<CarMovementHandler>();
        
        // Initialize audio sources if they're not assigned in the inspector
        if (engineAudioSource == null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.clip = engineClip;
            engineAudioSource.loop = true;
            engineAudioSource.playOnAwake = true;
            engineAudioSource.Play();
        }

        if (tireScreechAudioSource == null && tireScreechClip != null)
        {
            tireScreechAudioSource = gameObject.AddComponent<AudioSource>();
            tireScreechAudioSource.clip = tireScreechClip;
            tireScreechAudioSource.loop = true;
            tireScreechAudioSource.playOnAwake = false;
        }

        if (jumpAudioSource == null && jumpClip != null)
        {
            jumpAudioSource = gameObject.AddComponent<AudioSource>();
            jumpAudioSource.clip = jumpClip;
            jumpAudioSource.loop = false;
            jumpAudioSource.playOnAwake = false;
        }

        if (landingAudioSource == null && landingClip != null)
        {
            landingAudioSource = gameObject.AddComponent<AudioSource>();
            landingAudioSource.clip = landingClip;
            landingAudioSource.loop = false;
            landingAudioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        UpdateEngineSound();
        UpdateTireScreechSound();
    }

    private void UpdateEngineSound()
    {
        // Get the car's velocity for pitch adjustment
        float velocityMagnitude = movementHandler.GetVelocityMagnitude();

        // Calculate the desired engine pitch based on the car's speed
        desiredEnginePitch = 0.5f + velocityMagnitude * 0.025f;
        
        // Smoothly adjust the pitch
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 10);

        // Volume can also be adjusted based on acceleration input
        float accelerationInput = GetComponent<CarInputHandler>().GetAccelerationInput();
        engineAudioSource.volume = 0.5f + Mathf.Abs(accelerationInput) * 0.5f;
    }

    private void UpdateTireScreechSound()
    {
        // Check if tires are screeching
        bool isTireScreeching = movementHandler.IsTireScreeching(out float lateralVelocity, out bool isBraking);

        // Adjust the volume and playing state based on the screeching intensity
        if (isTireScreeching)
        {
            if (!tireScreechAudioSource.isPlaying)
                tireScreechAudioSource.Play();

            // Increase the volume of the screech based on the amount of sliding
            tireScratchTime += Time.deltaTime;
            float desiredVolume = Mathf.Abs(lateralVelocity) * 0.05f;
            
            // Braking should be louder
            if (isBraking)
                desiredVolume = 0.5f;

            tireScreechAudioSource.volume = Mathf.Lerp(0.1f, desiredVolume, tireScratchTime * 2);
        }
        else
        {
            tireScratchTime = 0;
            
            if (tireScreechAudioSource.isPlaying)
                tireScreechAudioSource.Stop();
        }
    }

    public void PlayJumpSfx()
    {
        if (jumpAudioSource != null && jumpClip != null)
        {
            jumpAudioSource.pitch = Random.Range(0.95f, 1.05f);
            jumpAudioSource.Play();
        }
    }

    public void PlayLandingSfx()
    {
        if (landingAudioSource != null && landingClip != null)
        {
            landingAudioSource.pitch = Random.Range(0.95f, 1.05f);
            landingAudioSource.Play();
        }
    }
}
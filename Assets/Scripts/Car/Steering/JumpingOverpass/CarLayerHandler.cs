using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLayerHandler : MonoBehaviour
{
    public SpriteRenderer carOutlineSpriteRenderer;
    
    // Fixed collections - you were missing the type for the List generic
    private List<SpriteRenderer> defaultLayerSpriteRenderers = new List<SpriteRenderer>();
    private List<Collider2D> overpassColliderList = new List<Collider2D>();
    private List<Collider2D> underpassColliderList = new List<Collider2D>();
    private Collider2D carCollider;
    
    // State
    private bool isDrivingOnOverpass = false;
    
    void Awake()
    {
        // Get all sprite renderers in children
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.sortingLayerName == "Default")
                defaultLayerSpriteRenderers.Add(spriteRenderer);
        }
        
        // Find all overpass colliders - you were missing the component type in GetComponent
        foreach (GameObject overpassColliderGameObject in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {
            Collider2D collider = overpassColliderGameObject.GetComponent<Collider2D>();
            if (collider != null)
                overpassColliderList.Add(collider);
        }
        
        // Find all underpass colliders - you were missing the component type in GetComponent
        foreach (GameObject underpassColliderGameObject in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
        {
            Collider2D collider = underpassColliderGameObject.GetComponent<Collider2D>();
            if (collider != null)
                underpassColliderList.Add(collider);
        }
        
        // Get car collider - you were missing the component type in GetComponentInChildren
        carCollider = GetComponentInChildren<Collider2D>();
        
        // Default drive on underpass.
        if (carCollider != null)
            carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnUnderpass");
        else
            Debug.LogError("No Collider2D found on car or its children!");
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSortingAndCollisionLayers();
    }

    void UpdateSortingAndCollisionLayers()
    {
        if (isDrivingOnOverpass)
        {
            SetSortingLayer("RaceTrackOverpass");
            if (carOutlineSpriteRenderer != null)
                carOutlineSpriteRenderer.enabled = false;
        }
        else
        {
            SetSortingLayer("Default");
            if (carOutlineSpriteRenderer != null)
                carOutlineSpriteRenderer.enabled = true;
        }
        
        SetCollisionWithOverPass();
    }

    void SetCollisionWithOverPass()
    {
        if (carCollider == null)
            return;
            
        foreach (Collider2D collider2D in overpassColliderList)
        {
            if (collider2D != null)
                Physics2D.IgnoreCollision(carCollider, collider2D, !isDrivingOnOverpass);
        }
        
        foreach (Collider2D collider2D in underpassColliderList)
        {
            if (collider2D != null)
            {
                if (isDrivingOnOverpass)
                    Physics2D.IgnoreCollision(carCollider, collider2D, true);
                else 
                    Physics2D.IgnoreCollision(carCollider, collider2D, false);
            }
        }
    }

    void SetSortingLayer(string layerName)
    {
        foreach (SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
        {
            if (spriteRenderer != null)
                spriteRenderer.sortingLayerName = layerName;
        }
    }

    public bool IsDrivingOnOverpass()
    {
        return isDrivingOnOverpass;
    }

    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (carCollider == null)
            return;
            
        if (collider2d.CompareTag("UnderpassTrigger"))
        {
            isDrivingOnOverpass = false;
            carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnUnderpass");
            UpdateSortingAndCollisionLayers();
        }
        else if (collider2d.CompareTag("OverpassTrigger"))
        {
            isDrivingOnOverpass = true;
            carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnOverpass");
            UpdateSortingAndCollisionLayers();
        }
    }
}
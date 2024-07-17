using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSurfaceHandler : MonoBehaviour
{
    [Header("Surface detection")]
    public LayerMask surfaceLayer;


    Vector3 lastSampledSurfacePosition = Vector3.one * 10000;
    Collider2D[] surfaceColidersHit = new Collider2D[10];
    Surface.SurfaceTypes drivingOnSurface = Surface.SurfaceTypes.Road;
    Collider2D carCollider;
    private void Awake() {
        carCollider = GetComponentInChildren<Collider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - lastSampledSurfacePosition).sqrMagnitude < 0.75f)
            return;
        
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = surfaceLayer;
        contactFilter2D.useLayerMask = true;
        contactFilter2D.useTriggers = true;
        int numbersOfHits =  Physics2D.OverlapCollider(carCollider,contactFilter2D,surfaceColidersHit);

        float lastSurfaceZValue  =-1000;

        for(int i=0 ; i< numbersOfHits;i++)
        {
            Surface surface  = surfaceColidersHit[i].GetComponent<Surface>();

            if(surface.transform.position.z > lastSurfaceZValue)
            {
                drivingOnSurface = surface.surfaceType;
                lastSurfaceZValue = surface.transform.position.z;

            }
        }
        if(numbersOfHits ==0)
        {
            drivingOnSurface = Surface.SurfaceTypes.Road;

        }
        lastSampledSurfacePosition = transform.position;
        Debug.Log("Driving on" + drivingOnSurface);
    }


    public Surface.SurfaceTypes GetCurrentSurface()
    {
        return drivingOnSurface;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    public float smoothedSpeed = 0.125f;

    

    
   
    void FixedUpdate()
    {
       
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothedSpeed);
        transform.position = smoothedPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class PlayerRunnerController : MonoBehaviour
{
    public bool autoCentering, keepWalking;

    public float sensitivity, speed, rotateClamp, xPosClamp;
    
    private bool holding;

    private Vector3 pos1, pos2;

    private Rigidbody rb;

    private Animator anim;

    private bool initWalk;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}

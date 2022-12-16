using System;
using _Managers;
using UnityEngine;

namespace Player
{
    public class TargetHeadRotation : MonoBehaviour
    {

        public Transform target;
        public float rotationspeed = 3;
        public bool okHead= false;

        private void Awake()
        {
            target = EX_GameManager.Instance.headTargetTarget;
        }

        public void HeadRotation()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 targetDirection = target.position - transform.position;
                float singleStep = rotationspeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                Debug.DrawRay(transform.position, newDirection, Color.black);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

        }
    }
}

using System;
using _Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using Random = System.Random;

namespace Player
{
    public abstract class PlayerValues : Singleton<AIPlayerMovement>
    {
        public float speed = 1.0f;
        protected Rigidbody Rb;
        protected Animator Anim;
        protected float Distance = 3;
        public Vector3 target;
        protected Vector3 firstTarget;
        public float rotationspeed = 5;
        protected NavMeshAgent _agent;
        public float extraCoin;
       
        public bool stoppedOnce;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.avoidancePriority = UnityEngine.Random.Range(0, 50);
        }

        public void TargetHole(Vector3 targetPos)
        {
            if (stoppedOnce)
            {
                if (Vector3.Distance(transform.position, target) < 1000f)
                {
                    target /= 2;
                }
                var step =  speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, EX_GameManager.Instance.holeTransform.position, step);
            }
            else
            {
                if(_agent.enabled)
                    _agent.SetDestination(EX_GameManager.Instance.holeTransform.position);
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using _Managers;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Player
{
    public class AIPlayerMovement : PlayerValues
    {
        #region Values

        [SerializeField] private bool rotate = false;

        public bool deadlife;

        // The target marker.
        [SerializeField] public bool playerStartLife = false;
        [SerializeField] public bool life = false;
        [SerializeField] private string endlineName;
        public Transform gameObjectCam;
        [SerializeField] private TargetHeadRotation targetHeadRotation;
        [SerializeField] private Collider playerCollider;
        private Vector3 hitPosition;

        public float velocityX = 0.0f;
        public float acceleration = 1;

        [SerializeField] private float endDistance;

        public List<AIPlayerMovement> aIPlayerMovements;

        public bool canShoot;

        public float baseCoin = 10;

        //"Animasyon Name"
        private static readonly int İsDead = Animator.StringToHash("isDead");
        private static readonly int InputX = Animator.StringToHash("InputX");

        #endregion

        private void Start()
        {
            hitPosition = EX_GameManager.Instance.holeTransform.position;
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody>();
            playerCollider = GetComponent<Collider>();
            Vector3 randDir = EX_GameManager.Instance.holeTransform.forward;
            randDir = Quaternion.Euler(0, Random.Range(0, 360), 0) * randDir;
            Vector3 targetPos = EX_GameManager.Instance.holeTransform.position;
            targetPos = targetPos + (randDir * 6);

            firstTarget = targetPos;
            ChooseTarget();
            gameObjectCam = EX_GameManager.Instance.camTransform;
            playerStartLife = true;
            life = true;
            // endlineName = "EndLine";
            transform.DOLookAt(Vector3.zero, 0.2f, AxisConstraint.Y);
        }


        void Update()
        {
            if (rotate)
            {
                //rotationspeed = 100;
                //RotatePlayer();
                transform.DOLookAt(EX_GameManager.Instance.holeTransform.position, 0.2f, AxisConstraint.Y);
            }

            if (playerStartLife)
            {
                //RotatePlayer();
                BlendAnim();
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0, 0.01f),
                    transform.position.z);
            }
        }

        private void RotatePlayer()
        {
            if (life) return;
            var position = transform.position;
            Vector3 targetDirection = target - position;
            float singleStep = rotationspeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Debug.DrawRay(position, newDirection, Color.black);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        private void BlendAnim()
        {
            if (life)
            {
                if (velocityX <= 0.9)
                {
                    velocityX += Time.deltaTime * acceleration;
                    Anim.SetFloat("InputX", velocityX);
                }

                if (!deadlife)
                    TargetHole(target);
            }

            if (!life)
            {
                // target = gameObjectCam;
            }
        }

        void ChooseTarget()
        {
            target = firstTarget;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("StopP") && !on)
            {
                if (aIPlayerMovements.Count < 1)
                    collision.gameObject.GetComponent<AIPlayerMovement>().aIPlayerMovements.Add(this);
                TriggerAndCollision();
            }
        }

        public bool dump;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Shooting") && !gameObject.CompareTag("Dead"))
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
                if(EX_GameManager.Instance.value == 0)
                    EX_GameManager.Instance.tutorial.SetActive(true);
            }

            if (other.gameObject.CompareTag("Dump"))
            {
                gameObject.layer = LayerMask.NameToLayer("Dump");
                stoppedOnce = true;
            }

            if (other.gameObject.CompareTag("EndLine"))
            {
                TriggerAndCollision();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!deadlife && other.gameObject.CompareTag("Hole") && !life &&
                (gameObject.CompareTag("StopP") || gameObject.CompareTag("Player")))
            {
                _agent.enabled = false;
                deadlife = true;
                canShoot = false;
                gameObject.tag = "Dead";
                Anim.SetTrigger("Falling");
                life = false;
                playerStartLife = false;
                rotate = false;
                transform.DOMoveY(-40, 8);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                // Rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX;
                InstantiePlayer.Instance.players.Remove(gameObject);
                InstantiePlayer.Instance.rbPlayers.Remove(Rb);
                _agent.enabled = false;
                // EX_TimeManager.Instance.transform.DOMoveX(0, 0.6f).OnComplete(() =>
                // {
                //     Destroy(gameObject, 2);
                //     TriggerEffect.Instance.ParticleStart(baseCoin + extraCoin);
                // });
                var deadPos = DeadPositionCheck();
                deadPos = new Vector3(deadPos.x, -30, deadPos.z);
                transform.DOJump(deadPos, 23f, 1, 1.2f).OnComplete(() =>
                {
                    Destroy(gameObject, 0.1f);
                    TriggerEffect.Instance.ParticleStart(baseCoin + extraCoin);
                    InstantiePlayer.Instance.BoundaryText();
                });
            }

            if (other.CompareTag("Dump") && !deadlife) canShoot = true;
        }

        private bool on = false;

        public void TriggerAndCollision()
        {
            if (!on)
            {
                Rb.mass = 1000f;
                gameObject.tag = "StopP";
                _agent.enabled = false;
                // target = gameObjectCam;
                life = false;
                playerStartLife = false;
                // Rb.constraints = RigidbodyConstraints.None;
                rotate = true;
                velocityX = 0;
                on = true;
            }

            Rb.velocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;
            targetHeadRotation.HeadRotation();
            InstantiePlayer.Instance.SpeedStop();
            Anim.SetFloat(InputX, velocityX);
        }

        [SerializeField] private float torque;

        public void Dead()
        {
            deadlife = true;
            canShoot = false;
            playerCollider.isTrigger = true;
            for (int i = 0; i < aIPlayerMovements.Count; i++)
            {
                if (aIPlayerMovements[i])
                    aIPlayerMovements[i].MoveCheck();
            }

            target = EX_GameManager.Instance.holeTransform.position;
            life = false;
            rotate = false;
            playerStartLife = false;
            Anim.SetTrigger(İsDead);
            var deadPos = DeadPositionCheck();
            deadPos = new Vector3(deadPos.x, -30, deadPos.z);
            InstantiePlayer.Instance.players.Remove(gameObject);
            InstantiePlayer.Instance.rbPlayers.Remove(Rb);
            transform.DOLookAt(Vector3.zero, 0.2f, AxisConstraint.Y);
            EX_GameManager.Instance.SlowVib();
            _agent.enabled = false;
            transform.DOJump(deadPos, 23f, 1, 1.2f).OnComplete(() =>
            {
                Destroy(gameObject, 0.1f);
                TriggerEffect.Instance.ParticleStart(baseCoin + extraCoin);
                InstantiePlayer.Instance.BoundaryText();
            });
        }

        public void MoveCheck()
        {
            if (deadlife) //ati
                return;

            gameObject.tag = "Player";
            on = false;
            _agent.enabled = true;
            playerStartLife = true;
            if (!life)
                for (int i = 0; i < aIPlayerMovements.Count; i++)
                {
                    if (aIPlayerMovements[i])
                        aIPlayerMovements[i].MoveCheck();
                }

            life = true;
        }

        public void DumpMoveCheck()
        {
            on = false;
            playerStartLife = true;
            _agent.enabled = true;
            if (!life)
                for (int i = 0; i < aIPlayerMovements.Count; i++)
                {
                    if (aIPlayerMovements[i])
                        aIPlayerMovements[i].MoveCheck();
                }

            life = true;
        }

        Vector3 DeadPositionCheck()
        {
            var position = transform.position;
            Vector3 distance = new Vector3(position.x - endDistance, position.y, position.z - endDistance).normalized;
            Vector3 totalPos = position - target;
            Vector3 deadPos = totalPos - distance;
            return deadPos;
        }

        public void DistanceHole()
        {
            Distance += EX_GameManager.Instance.holeScaleValue;
        }

        public void Interact(Interactable.InteractableType interactableType)
        {
            switch (interactableType)
            {
                case Interactable.InteractableType.Level1:
                    transform.DOScale(1.5f, 0.2f);
                    break;
                case Interactable.InteractableType.Level2:

                    break;
                case Interactable.InteractableType.Level3:
                    break;
            }

            ChooseTarget();
        }

        // private void OnDrawGizmos()
        // {
        //     if (firstTarget != Vector3.zero)
        //     {
        //         Gizmos.color = Color.red;
        //         Gizmos.DrawWireSphere(firstTarget,1f);
        //     }
        // }
    }
}
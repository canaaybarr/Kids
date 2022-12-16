using _Managers;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class RaycastController : Singleton<RaycastController>
    {
        [SerializeField] public Camera cam;
        [SerializeField] private LayerMask layer;
        [SerializeField] private LayerMask dumpLayer;

         public float rayScale = 1.5f;
        
        

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Input.GetMouseButton(0))
            {
                InstantiePlayer.Instance.SpeedStop();
                if (Physics.SphereCast(ray,rayScale ,out hit,Mathf.Infinity,layer))
                {
                    if (!hit.transform.GetComponent<AIPlayerMovement>().deadlife && hit.collider != null && hit.collider.gameObject.GetComponent<AIPlayerMovement>().playerStartLife == false && hit.collider.gameObject.layer != LayerMask.NameToLayer("Dead"))
                    {
                        var hitObj = hit.collider.gameObject;
                        var layermask = LayerMask.NameToLayer("Dead");
                        hitObj.tag = "Dead";
                        hitObj.layer = layermask;
                        hitObj.GetComponent<Rigidbody>().isKinematic = false;
                        hitObj.GetComponent<AIPlayerMovement>().Dead();
                        InstantiePlayer.Instance.BoundaryText();
                        InstantiePlayer.Instance.SpeedStop();
                    }
                }
                
                if (Physics.SphereCast(ray,rayScale,out hit,Mathf.Infinity,dumpLayer))
                {
                    if (hit.collider != null && hit.collider.CompareTag("StopP"))
                    {
                        hit.collider.gameObject.transform.DOLookAt(Vector3.zero, 0.2f,AxisConstraint.Y);
                        hit.collider.gameObject.GetComponent<AIPlayerMovement>().DumpMoveCheck();
                        InstantiePlayer.Instance.SpeedStop();
                    }
                }
            }
        }
    }
}

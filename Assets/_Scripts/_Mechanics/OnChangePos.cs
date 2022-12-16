using System;
using UnityEngine;

namespace _Mechanics
{
    public class OnChangePos : MonoBehaviour
    {
        public PolygonCollider2D hole2DCollider;
        public PolygonCollider2D ground2DCollider;
        private Mesh genarateMesh;
        public float initialScale = 0.5f;
        public MeshCollider genarateMeshCollider;


        private void FixedUpdate()
        {
            if (transform.hasChanged == true)
            {
                transform.hasChanged = false;
                hole2DCollider.transform.position = new Vector2(transform.position.x,transform.position.y);
                hole2DCollider.transform.localScale = transform.localScale * initialScale;
                Make2DHole();
                MakeMesh3DCollider();
            }
        }

        private void MakeMesh3DCollider()
        {
            if (genarateMesh != null) Destroy(genarateMesh);
            genarateMesh = ground2DCollider.CreateMesh(true, true);
            genarateMeshCollider.sharedMesh = genarateMesh;
        }

        private void Make2DHole()
        {
            Vector2[] PointPositions = hole2DCollider.GetPath(0);
            
            for (int i = 0; i < PointPositions.Length; i++)
            {
                PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
            }

            ground2DCollider.pathCount = 2;
            ground2DCollider.SetPath(1,PointPositions );
        }
        

    }
}

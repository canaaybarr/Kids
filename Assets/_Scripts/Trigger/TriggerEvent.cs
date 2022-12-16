using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent onEnter;
        private void OnTriggerEnter(Collider other)
        {
            onEnter?.Invoke();
        }
    }
}
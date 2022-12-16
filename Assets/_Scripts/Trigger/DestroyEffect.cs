using DG.Tweening;
using UnityEngine;

namespace Trigger
{
    public class DestroyEffect : MonoBehaviour
    {
        void Start()
        {
            EX_TimeManager.Instance.transform.DOMoveX(0, 2f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}

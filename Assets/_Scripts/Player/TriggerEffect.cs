using _Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Player
{
    public class TriggerEffect : Singleton<TriggerEffect>
    {

        public GameObject moneyText;
        public float effectScale = 1.25f;

        
        public void ParticleStart(float extraCoin)
        {   
            EX_GameManager.Instance.ThrowedMan();
            EX_GameManager.Instance.SlowVib();
            var tranformOther = gameObject.transform.position;
            var money = Instantiate(moneyText, new Vector3(tranformOther.x, tranformOther.y, tranformOther.z), Quaternion.Euler(0, 0, 0), EX_GameManager.Instance.effectParent);
            money.GetComponentInChildren<TMP_Text>().text = "+" + (CoinManager.Instance.coinValue + extraCoin);
            CoinManager.Instance.AddCoin(CoinManager.Instance.coinValue + extraCoin);
            EX_TimeManager.Instance.transform.DOMoveX(0, 4f).OnComplete(() =>
            {
                money.SetActive(false);
            });
        }
    }
}

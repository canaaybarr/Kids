using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Managers
{
    [System.Serializable]
    public class CoinManager : Singleton<CoinManager>
    {
        [Header("Button List")]
        [SerializeField] private Button spawnRateButton;
        [SerializeField] private GameObject holeScale;
        [SerializeField] private Button incomeButton;

        [Header("CoinText")] 
        public Color grayColor, blackColor,textColorGray;
        public TMP_Text spawnColorText, incomeColorText, holeColorText;
        
        [Header("Spawn")] 

        [SerializeField] private Image spawnColorButton, spawnColorImage, spawnColorSembol;
        
        [Header("İncome")] 

        [SerializeField] private Image incomeColorButton, incomeColorImage, incomeColorSembol;
        
        [Header("Hole")] 

        [SerializeField] private Image holeColorButton, holeColorImage, holeColorSembol;

        
        [Header("CoinText")] [SerializeField]
        public TMP_Text coinText;
        

        [Header("Coin Variable")] public float totalCoin;  

        
        [Header("Upgrade Coin Value")]
        public float coinValue;
        
        [SerializeField]private EnableExtension extension;


        private void Update()
        {
            UpgradeColorCheck();
        }

        public void AddCoin(float extraCoin)
        {
            totalCoin += (coinValue + extraCoin);
            CallTwenScale();
            PlayerPrefs.SetFloat("TotalMoney",totalCoin);
        }

        public void RemoveCoin()
        {
            totalCoin -= coinValue;
            CallTwenScale();
            PlayerPrefs.SetFloat("TotalMoney",totalCoin);
        }
        public void RemoveCoin(float cost)
        {
            totalCoin -= cost;
            CallTwenScale();
            PlayerPrefs.SetFloat("TotalMoney",totalCoin);
        }

        public void UpgradeCoin()
        {
            if (EX_GameManager.Instance.incomeSellCoin <= totalCoin && coinValue <= 100)
            {
                coinValue++;
                totalCoin -= EX_GameManager.Instance.incomeSellCoin;
                EX_GameManager.Instance.incomeSellCoin *= 1.5f;
                EX_GameManager.Instance.incomeSellCoin = Mathf.RoundToInt(EX_GameManager.Instance.incomeSellCoin);
                EX_GameManager.Instance.incomeText.text = "" + EX_GameManager.Instance.incomeSellCoin;
                PlayerPrefs.SetFloat("IncomeSellCoin",EX_GameManager.Instance.incomeSellCoin);
                PlayerPrefs.SetFloat("CoinDeadValue",coinValue);
                PlayerPrefs.SetFloat("TotalMoney",totalCoin);
                CallTwenScale();
            }
        }

        public void ColorChangeGray(TMP_Text textColor)
        {
            textColor.color = textColorGray;
        }
        public void ColorChangeBlack(TMP_Text textColor)
        {
            textColor.color = blackColor;
        }

        public void CallTwenScale()
        {
            extension.CallTween();
            coinText.text = ""+ totalCoin;
        }

        public void UpgradeColorCheck()
        {
            Debug.Log(EX_GameManager.Instance.spawnSpeedText.color);
            Debug.Log("renk");
            if (totalCoin <= EX_GameManager.Instance.spawnSellCoin)
            {
                // spawnColorText, incomeColorText, holeColorText
                ColorChangeGray(EX_GameManager.Instance.spawnSpeedText);
                ColorChangeGray(spawnColorText);
                ColorButtonImageGray(spawnColorButton);
                ColorButtonImageGray(spawnColorImage);
                ColorButtonImageGray(spawnColorSembol);
            }
            else
            {
                ColorChangeBlack(EX_GameManager.Instance.spawnSpeedText);
                ColorChangeBlack(spawnColorText);
                ColorButtonImageBlack(spawnColorButton);
                ColorButtonImageBlack(spawnColorImage);
                ColorButtonImageBlack(spawnColorSembol);
            }
//
            if (totalCoin <= EX_GameManager.Instance.incomeSellCoin)
            {
                ColorChangeGray(EX_GameManager.Instance.incomeText);
                ColorChangeGray(incomeColorText);
                ColorButtonImageGray(incomeColorButton);
                ColorButtonImageGray(incomeColorImage);
                ColorButtonImageGray(incomeColorSembol);
            }
            else
            {
                ColorChangeBlack(EX_GameManager.Instance.incomeText);
                ColorChangeBlack(incomeColorText);
                ColorButtonImageBlack(incomeColorButton);
                ColorButtonImageBlack(incomeColorImage);
                ColorButtonImageBlack(incomeColorSembol);
            }
            if (totalCoin <= EX_GameManager.Instance.holeSellCoin)
            {
                ColorChangeGray(EX_GameManager.Instance.holeText);
                ColorChangeGray(holeColorText);
                ColorButtonImageGray(holeColorButton);
                ColorButtonImageGray(holeColorImage);
                ColorButtonImageGray(holeColorSembol);
            }
            else
            {
                ColorChangeBlack(EX_GameManager.Instance.holeText);
                ColorChangeBlack(holeColorText);
                ColorButtonImageBlack(holeColorButton);
                ColorButtonImageBlack(holeColorImage);
                ColorButtonImageBlack(holeColorSembol);
            }
        }
        
        
        
        public void ColorButtonImageGray(Image imageColor)
        {
            imageColor.color = grayColor;
        }
        public void ColorButtonImageBlack(Image imageColor)
        {
            imageColor.color = Color.white;
        }
    }
}

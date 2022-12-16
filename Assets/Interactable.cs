using System;
using System.Collections;
using System.Collections.Generic;
using _Managers;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Interactable : MonoBehaviour
{
    public float cost;
    public float manCost;

    public bool unlocked;
    public bool manCostUnlocked;
    public InteractableType interactableType;

    public enum InteractableType
    {
        Level1,
        Level2,
        Level3,
    }

    public int level;
    public List<UpgradeStats> upgrades;

    [Serializable]
    public class UpgradeStats
    {
        public int upgradeCost;
        public float spawnRate;
        public int spawnCount;
        public Color spawnColor;
        public float extraCoin;
    }

    public Transform building;
    public Transform buildingModel;

    public GameObject interactableUi;
    public GameObject upgradeUi;
    public GameObject spawnerUi;

    public GameObject buyUi;
    public GameObject manCostUi;
    public Image spawnerFill;

    public TextMeshProUGUI levelText;

    public Button button;
    public TextMeshProUGUI costText;
    public TextMeshPro manCostText;

    public Button upgradeButton;
    public TextMeshProUGUI upgradeCostText;

    public GameObject upgradeParticle;

    private float spawnRate;
    private float spawnCount;
    private Color spawnColor;
    private float extraCoin;

    private float spawnTimer;

    public List<Transform> spawnTransforms;

    public List<GameObject> triggeringPlayers;

    private void Awake()
    {
        costText.text = cost.ToString();
        int unlockedInt = PlayerPrefs.GetInt((int)interactableType + "Unlocked", 0);
        int manCostunlockedInt = PlayerPrefs.GetInt((int)interactableType + "ManCostUnlocked", 0);
        level = PlayerPrefs.GetInt((int)interactableType + "Level", 0);
        unlocked = unlockedInt == 1;
        manCostUnlocked = manCostunlockedInt == 1;
        CheckBuy();
        CheckUpgrade();
    }

    private void Update()
    {
        if (!manCostUnlocked && EX_GameManager.Instance.throwedMan >= manCost)
        {
            manCostUnlocked = true;
            PlayerPrefs.SetInt((int)interactableType + "ManCostUnlocked",1);
        }
        else if (!manCostUnlocked)
        {
            manCostText.text = EX_GameManager.Instance.throwedMan + "/" + manCost;
        }
        else if (manCostUnlocked)
        {
            manCostUi.SetActive(false);
            buyUi.SetActive(true);
            button.interactable = CoinManager.Instance.totalCoin >= cost;

            if (unlocked)
            {    
                levelText.text = "Lvl " + (level + 1);
                UpgradeUi();
                spawnerFill.fillAmount = spawnTimer / spawnRate;
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnRate && triggeringPlayers.Count <= 0 && InstantiePlayer.Instance.players.Count < InstantiePlayer.Instance.boundary)
                {
                    switch (interactableType)
                    {
                        case InteractableType.Level1:
                            for (int i = 0; i < spawnTransforms.Count; i++)
                            {
                                for (int j = 0; j < spawnCount; j++)
                                {   
                                    Vector3 randPos = UnityEngine.Random.insideUnitCircle * 2.5f;
                                    Vector3 spawnPos = spawnTransforms[i].position + new Vector3(randPos.x, 0, randPos.z);
                                    InstantiePlayer.Instance.Spawn(5, 1f, spawnPos,
                                        spawnTransforms[i].eulerAngles, Color.black, extraCoin);
                                }
                            }

                            break;
                        case InteractableType.Level2:
                            for (int i = 0; i < spawnTransforms.Count; i++)
                            {
                                for (int j = 0; j < spawnCount; j++)
                                {   
                                    Vector3 randPos = UnityEngine.Random.insideUnitCircle * 2.5f;
                                    Vector3 spawnPos = spawnTransforms[i].position + new Vector3(randPos.x, 0, randPos.z);
                                    InstantiePlayer.Instance.Spawn(20, 2f, spawnPos,
                                        spawnTransforms[i].eulerAngles, Color.black, extraCoin);
                                }
                            }

                            break;
                        case InteractableType.Level3:


                            for (int i = 0; i < spawnTransforms.Count; i++)
                            {
                                for (int j = 0; j < spawnCount; j++)
                                {   
                                    Vector3 randPos = UnityEngine.Random.insideUnitCircle * 2.5f;
                                    Vector3 spawnPos = spawnTransforms[i].position + new Vector3(randPos.x, 0, randPos.z);
                                    
                                    InstantiePlayer.Instance.Spawn(30, 1.5f, spawnPos,
                                        spawnTransforms[i].eulerAngles, spawnColor, extraCoin);
                                }
                            }

                            break;
                    }

                    BuildingAnim();
                    spawnTimer = 0;
                }
            }
        }
    }


    public void Buy()
    {
        if (CoinManager.Instance.totalCoin >= cost)
        {
            CoinManager.Instance.RemoveCoin(cost);
            unlocked = true;
            CheckBuy();
            PlayerPrefs.SetInt((int)interactableType + "Unlocked", 1);
        }
    }

    public void CheckBuy()
    {
        if (unlocked)
        {
            building.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
            interactableUi.SetActive(false);
            manCostUi.SetActive(false);
            upgradeUi.SetActive(true);
            spawnerUi.SetActive(true);
        }
    }

    public void Upgrade()
    {
        if (CoinManager.Instance.totalCoin >= upgrades[level].upgradeCost)
        {
            GameObject upgradeParticleObj =
                Instantiate(upgradeParticle, transform.position, Quaternion.identity);

            transform.DOScale(1.1f, 0.3f).SetLoops(2, LoopType.Yoyo);

            CoinManager.Instance.RemoveCoin(upgrades[level].upgradeCost);
            level++;
            PlayerPrefs.SetInt((int)interactableType + "Level", level);
            CheckUpgrade();
        }
    }

    void CheckUpgrade()
    {
        spawnRate = upgrades[level].spawnRate;
        spawnCount = upgrades[level].spawnCount;
        spawnColor = upgrades[level].spawnColor;
        extraCoin = upgrades[level].extraCoin;
    }

    void UpgradeUi()
    {
        if (level == upgrades.Count - 1)
        {
            upgradeUi.SetActive(false);
            return;
        }
        else
        {
            upgradeCostText.text = upgrades[level].upgradeCost.ToString();
        }

        if (CoinManager.Instance.totalCoin >= upgrades[level].upgradeCost)
            upgradeButton.interactable = true;
        else
            upgradeButton.interactable = false;
    }

    void BuildingAnim()
    {
        DOTween.Rewind(buildingModel);
        switch (interactableType)
        {
            case InteractableType.Level1:
                buildingModel.DOScaleY(1f, 0.25f).SetRelative(true).SetLoops(2, LoopType.Yoyo);
                break;
            case InteractableType.Level2:
                buildingModel.DOScaleY(0.4f, 0.25f).SetRelative(true).SetLoops(2, LoopType.Yoyo);
                break;
            case InteractableType.Level3:
                buildingModel.DOScaleY(0.4f, 0.25f).SetRelative(true).SetLoops(2, LoopType.Yoyo);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Dump")) && !triggeringPlayers.Contains(other.gameObject))
        {
            triggeringPlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Dump")) && triggeringPlayers.Contains(other.gameObject))
        {
            triggeringPlayers.Remove(other.gameObject);
        }
    }
    // private void OnTriggerEnter(Collider other)
    // {  
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         other.GetComponent<AIPlayerMovement>().Interact(interactableType);
    //     }
    // }
}
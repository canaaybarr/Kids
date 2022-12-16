using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Player;
using TMPro;
using UnityEngine;

namespace _Managers
{
    [System.Serializable]
    public class EX_GameManager : GameManager<EX_GameManager>
    {
        #region Stage Parameters

        [Header("Camera Transform")] public Transform camTransform;

        public Transform effectParent;
        [Header("Hole Value")] public Transform holeTransform;

        public Transform headTargetTarget;

        public int i;

        [SerializeField] private Collider holeCollider;
        [SerializeField] public Collider wallholeCollider;

        public Transform holeColliderScale;
        public GameObject holeScale;
        public float holeScaleValue;
        public float holeScaleTime = 1f;
        [SerializeField] private float scaleAddValue;

        [Header("Upgrade SpawnSecond")] public float negativeSecond;

        [Header("Upgrade Start Money")] public float startCoin = 10;

        [Header("Upgrade value")] public float holeSellCoin = 10;
        public float incomeSellCoin = 10;
        public float spawnSellCoin = 10;

        [Header("Upgrade Text")] public TMP_Text holeText;
        public TMP_Text incomeText;
        public TMP_Text spawnSpeedText;

        [SerializeField] public int value = 0;

        [HideInInspector] // If the game has stages, delete this line.
        public int stageCount;

        int currentStageIndex;
        public float camSize;

        public GameObject tutorial;
        public GameObject playerTutorial;

        [Header("Throwd Man")] public int throwedMan;

        #endregion

        private void Awake()
        {
            holeText.text = "" + holeSellCoin;
            incomeText.text = "" + incomeSellCoin;
            spawnSpeedText.text = "" + spawnSellCoin;
        }

        [SerializeField] private bool playerprefDelet;


        public override void Start()
        {
            value = PlayerPrefs.GetInt("bool");
            ElephantSDK.Elephant.LevelStarted(EX_LevelManager.Instance.currentLevelNumber + 1);
            base.Start();
            if (value == 0)
            {
                SetNumSave();
                // value = 1;
                // PlayerPrefs.SetInt("bool", value);
                InstantiePlayer.Instance.StartSpawn();
                CoinManager.Instance.UpgradeColorCheck();
            }
            else
            {
                Destroy(playerTutorial);
                tutorial.SetActive(false);
                InstantiePlayer.Instance.StartSpawn();
            }

            CoinManager.Instance.UpgradeColorCheck();
            camSize = PlayerPrefs.GetFloat("CamSize");
            holeScaleValue = PlayerPrefs.GetFloat("PosX");
            holeSellCoin = PlayerPrefs.GetFloat("HoleSellCoin");
            spawnSellCoin = PlayerPrefs.GetFloat("SpawnSellCoin");
            InstantiePlayer.Instance.spawnSeconds = PlayerPrefs.GetFloat("spawnSeconds");
            incomeSellCoin = PlayerPrefs.GetFloat("IncomeSellCoin");
            TriggerEffect.Instance.effectScale = PlayerPrefs.GetFloat("EffectScale");
            CoinManager.Instance.coinValue = PlayerPrefs.GetFloat("CoinDeadValue");
            InstantiePlayer.Instance.boundary = PlayerPrefs.GetInt("Boundary");
            InstantiePlayer.Instance.minPosX = PlayerPrefs.GetFloat("MinPosX");
            InstantiePlayer.Instance.maxPosX = PlayerPrefs.GetFloat("MaxPosX");
            InstantiePlayer.Instance.minPosZ = PlayerPrefs.GetFloat("MinPosZ");
            InstantiePlayer.Instance.maxPosZ = PlayerPrefs.GetFloat("MaxPosZ");
            CoinManager.Instance.totalCoin = PlayerPrefs.GetFloat("TotalMoney");
            CoinManager.Instance.coinText.text = "" + CoinManager.Instance.totalCoin;
            throwedMan = PlayerPrefs.GetInt("ThrowedMan", 0);
            Debug.Log(holeSellCoin);
            holeText.text = "" + holeSellCoin;
            incomeText.text = "" + incomeSellCoin;
            spawnSpeedText.text = "" + spawnSellCoin;
            RaycastController.Instance.cam.orthographicSize = camSize;
            InstantiePlayer.Instance.BoundaryText();
            //StageUIManager.Instance.StartStageUIManager(); // If the game has stages, uncomment this line.

            //CharacterUnRagdoll(player); // If the game has ragdoll character, uncomment this line.
        }

        int sizeClamp = 0;

        public void HoleSize()
        {
            if (holeSellCoin <= CoinManager.Instance.totalCoin && sizeClamp <= 6)
            {
                InstantiePlayer.Instance.PosPositive();
                wallholeCollider.enabled = false;
                holeCollider.enabled = true;
                sizeClamp++;
                var orthographicSize = RaycastController.Instance.cam.orthographicSize;
                orthographicSize += 2f;
                RaycastController.Instance.cam.orthographicSize = orthographicSize;
                camSize = orthographicSize;
                AIPlayerMovement.Instance.DistanceHole();
                holeScaleValue += scaleAddValue;
                holeScale.transform.DOScale(new Vector3(holeScaleValue, holeScaleValue, holeScaleValue), holeScaleTime);
                CoinManager.Instance.totalCoin -= holeSellCoin;
                holeSellCoin *= 2;
                holeText.text = "" + holeSellCoin;
                if (InstantiePlayer.Instance.boundary <= 400)
                {
                    InstantiePlayer.Instance.boundary *= 2;
                }

                RaycastController.Instance.rayScale += 0.1f;
                InstantiePlayer.Instance.BoundaryText();
                PlayerPrefs.SetInt("Boundary", InstantiePlayer.Instance.boundary);
                PlayerPrefs.SetFloat("PosX", holeScaleValue);
                PlayerPrefs.SetFloat("HoleSellCoin", holeSellCoin);
                PlayerPrefs.SetFloat("CamSize", camSize);
                CoinManager.Instance.CallTwenScale();
                PlayerPrefs.SetFloat("TotalMoney", CoinManager.Instance.totalCoin);
                EX_TimeManager.Instance.transform.DOMoveX(0, 5f).OnComplete(() =>
                {
                    wallholeCollider.enabled = true;
                    holeCollider.enabled = false;
                });
            }
            else if (sizeClamp > 6)
            {
                holeText.fontSize = 6;
                holeText.text = "Max Level";
            }
        }

        public void NegativeSecond()
        {
            if (spawnSellCoin <= CoinManager.Instance.totalCoin)
            {
                if (InstantiePlayer.Instance.spawnSeconds > 1)
                {
                    var bla = InstantiePlayer.Instance.spawnSeconds / 8;
                    InstantiePlayer.Instance.spawnSeconds -= 0.1f;
                    PlayerPrefs.SetFloat("spawnSeconds", InstantiePlayer.Instance.spawnSeconds);
                }

                CoinManager.Instance.totalCoin -= spawnSellCoin;
                spawnSellCoin *= 1.5f;
                spawnSellCoin = Mathf.RoundToInt(spawnSellCoin);
                spawnSpeedText.text = "" + spawnSellCoin;
                PlayerPrefs.SetFloat("SpawnSellCoin", spawnSellCoin);
                PlayerPrefs.SetFloat("spawnSeconds", InstantiePlayer.Instance.spawnSeconds);
                PlayerPrefs.SetFloat("TotalMoney", CoinManager.Instance.totalCoin);
                CoinManager.Instance.CallTwenScale();
            }
        }


        public override void Update()
        {
            base.Update();

            switch (state)
            {
                case STATE.Play:

                    // if (Input.GetMouseButtonDown(0))
                    // {
                    // }
                    //
                    // if (Input.GetMouseButtonUp(0))
                    // {
                    //     MMVibrationManager.Haptic(HapticTypes.Selection); // Ekrandan her parmagini cektiginde hafif titretir
                    // }

                    break;
            }
        }

        public void SlowVib()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            // Ekrana her dokundugunda hafif titretir
        }

        public override void Perfect()
        {
            base.Perfect();
        }

        public void Tutorial()
        {
            value = 1;
            PlayerPrefs.SetInt("bool", value);
        }

        public override void Win()
        {
            if (state == STATE.Win || state == STATE.Lose)
            {
                return;
            }

            ElephantSDK.Elephant.LevelCompleted(EX_LevelManager.Instance.currentLevelNumber + 1);
            base.Win();
        }

        public override void Lose()
        {
            if (state == STATE.Win || state == STATE.Lose)
            {
                return;
            }

            ElephantSDK.Elephant.LevelFailed(EX_LevelManager.Instance.currentLevelNumber + 1);
            base.Lose();
        }

        public void NextStage()
        {
            if (currentStageIndex + 1 < stageCount)
            {
                currentStageIndex++;
                StageUIManager.Instance.NextStage();
            }
            else
            {
                StageUIManager.Instance.Finish();
                Win();
            }
        }

        #region Ragdoll

        public void CharacterRagdoll(Transform characterParent)
        {
            foreach (Rigidbody rb in characterParent.GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }

            if (characterParent.GetComponent<Animator>())
            {
                characterParent.GetComponent<Animator>().enabled = false;
            }

            if (characterParent.GetComponent<Rigidbody>())
            {
                characterParent.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        public void CharacterUnRagdoll(Transform characterParent)
        {
            foreach (Rigidbody rb in characterParent.GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = true;
            }

            if (characterParent.GetComponent<Animator>())
            {
                characterParent.GetComponent<Animator>().enabled = true;
            }

            if (characterParent.GetComponent<Rigidbody>())
            {
                characterParent.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        #endregion


        public void SetNumSave()
        {
            camSize = 30f;
            CoinManager.Instance.coinValue = 0;
            PlayerPrefs.SetFloat("CoinDeadValue", CoinManager.Instance.coinValue);
            PlayerPrefs.SetFloat("PosX", holeScaleValue);
            PlayerPrefs.SetInt("bool", value);
            PlayerPrefs.SetFloat("CamSize", camSize);
            PlayerPrefs.SetFloat("HoleSellCoin", holeSellCoin);
            PlayerPrefs.SetInt("Boundary", InstantiePlayer.Instance.boundary);
            PlayerPrefs.SetFloat("SpawnSellCoin", spawnSellCoin);
            PlayerPrefs.SetFloat("spawnSeconds", InstantiePlayer.Instance.spawnSeconds);
            PlayerPrefs.SetFloat("IncomeSellCoin", incomeSellCoin);
            PlayerPrefs.SetFloat("TotalMoney", CoinManager.Instance.totalCoin);
            PlayerPrefs.SetFloat("MinPosX", InstantiePlayer.Instance.minPosX);
            PlayerPrefs.SetFloat("MaxPosX", InstantiePlayer.Instance.maxPosX);
            PlayerPrefs.SetFloat("MinPosZ", InstantiePlayer.Instance.minPosZ);
            PlayerPrefs.SetFloat("MaxPosZ", InstantiePlayer.Instance.maxPosZ);

            //InstantiePlayer.Instance.boundary += 100;
        }

        public void ThrowedMan()
        {
            throwedMan++;
            PlayerPrefs.SetInt("ThrowedMan", throwedMan);
        }
    }
}
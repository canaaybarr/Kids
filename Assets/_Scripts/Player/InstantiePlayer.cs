using System;
using System.Collections;
using System.Collections.Generic;
using _Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Player
{
    public class InstantiePlayer : Singleton<InstantiePlayer>
    {
        #region Variables

        [Header("Spawn Saniyesi")] public float spawnSeconds;

        public GameObject deadParent;
        [Header("Player")] [SerializeField] private GameObject playerObject;

        [Header("Sahnedeki Playerlerin Listesi")]
        public List<GameObject> players = new List<GameObject>();

        public List<Rigidbody> rbPlayers = new List<Rigidbody>();

        [Header("Spawn Sınırı")] public int boundary;

        private List<Vector3> generalPos = new List<Vector3>();
        private Vector3 spawnPos;

        [Header("Ekran Boyutuna Göre Ayarlanmalı")]
        public float maxPosX;

        public float minPosX;
        public float maxPosZ;
        public float minPosZ;


        public bool gameStart = false;


        [SerializeField] private TMP_Text boundaryText;

        [SerializeField] private Image filledSpawnImage;

        #endregion

        private void Start()
        {
            BoundaryText();
            gameStart = true;
        }

        public void BoundaryText()
        {
            boundaryText.text = EX_GameManager.Instance.throwedMan.ToString();
        }

        public void PosPositive()
        {
            minPosX += -1;
            maxPosX += 1;
            minPosZ += -2;
            maxPosZ += 2;

            //
            PlayerPrefs.SetFloat("MinPosX", minPosX);
            PlayerPrefs.SetFloat("MaxPosX", maxPosX);
            PlayerPrefs.SetFloat("MinPosZ", minPosZ);
            PlayerPrefs.SetFloat("MaxPosZ", maxPosZ);
        }

        Vector3 RandomPos()
        {
            Vector3 randompos;
            var nXpos = new Vector3(Random.Range(minPosX, -20), 0.5f, Random.Range(37, -37));
            var pXpos = new Vector3(Random.Range(maxPosX, 20), 0.5f, Random.Range(37, -37));
            var nZpos = new Vector3(Random.Range(20, -20), 0.5f, Random.Range(minPosZ, -37));
            var pZpos = new Vector3(Random.Range(20, -20), 0.5f, Random.Range(maxPosZ, 37));
            generalPos.Add(nXpos);
            generalPos.Add(pXpos);
            generalPos.Add(nZpos);
            generalPos.Add(pZpos);
            randompos = generalPos[Random.Range(0, 4)];
            //Debug.Log(message: randompos);
            generalPos.Clear();
            return randompos;
        }

        public void StartSpawn()
        {   
            for (int i = 0; i < 15; i++)
            {
                Spawn();
            }
            StartCoroutine(DelayTime());
        }

        private bool spawnDedector = false;

        public IEnumerator DelayTime()
        {
            if (gameStart)
            {
                for (;;)
                {
                    BoundaryText();
                    spawnDedector = false;
                    for (; players.Count < boundary;)
                    {
                        filledSpawnImage.DOFillAmount(1, spawnSeconds).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            filledSpawnImage.fillAmount = 0;
                        });
                        yield return new WaitForSeconds(spawnSeconds);
                        BoundaryText();
                        if (spawnDedector) yield break;

                        for (int i = 0; i < 15; i++)
                        {
                            Spawn();
                        }
                    }

                    spawnDedector = true;
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        public void Spawn(float baseCoin = 10,float scale = 1.5f, Vector3 spawnPos = default(Vector3),
            Vector3 rot = default(Vector3), Color? color = null,float extraCoin = 0)
        {
            if (spawnPos == Vector3.zero)
                spawnPos = RandomPos();
            
            GameObject player;
            
            player = Instantiate(playerObject, spawnPos, Quaternion.Euler(rot), transform);
            player.transform.localScale = new Vector3(scale, scale, scale);
            // player.GetComponent<Animator>().speed = 1 - (1 / scale);
            Color _color =  color??  Color.black;
            if (_color != Color.black)
                player.GetComponentInChildren<SkinnedMeshRenderer>().material.color = _color;
            if (extraCoin != 0)
                player.GetComponent<PlayerValues>().extraCoin = extraCoin;
           
            Debug.Log(baseCoin);
            player.GetComponent<AIPlayerMovement>().baseCoin = baseCoin;
            
            players.Add(player);
            rbPlayers.Add(player.GetComponent<Rigidbody>());
            BoundaryText();
        }

        public void SpeedStop()
        {
            for (int i = 0; i < rbPlayers.Count; i++)
            {
                rbPlayers[i].velocity = Vector3.zero;
                rbPlayers[i].angularVelocity = Vector3.zero;
            }
        }

        public IEnumerator StartGame()
        {
            for (; players.Count < 10;)
            {
                filledSpawnImage.DOFillAmount(1, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    filledSpawnImage.fillAmount = 0;
                });
                yield return new WaitForSeconds(0.2f);
                BoundaryText();
                if (spawnDedector) yield break;
                spawnPos = RandomPos();
                // GameObject player;
                // player = Instantiate(playerObject, spawnPos, Quaternion.identity, transform);
                InstantiePlayer.Instance.Spawn();
                // players.Add(player);
                BoundaryText();
            }
        }
    }
}
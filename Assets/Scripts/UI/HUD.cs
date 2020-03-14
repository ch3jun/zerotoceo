﻿using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : UIObject
    {
        private LevelManager _levelManager;
        private PlayerController _playerController;
        
        public TextMeshProUGUI hudPointsText;
        public GameObject heartOne, heartTwo, heartThree, futureToken, businessToken, leaderToken, americaToken;
        
        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _playerController = FindObjectOfType<PlayerController>();
            blocksRaycasts = false;
        }

        void Update()
        {
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level)
            {
                Enable();
                hudPointsText.SetText($"Points: {_playerController.points}");
        
                foreach (Transform child in heartOne.transform.parent)
                    child.gameObject.SetActive(false);
                switch (_playerController.lives)
                {
                    case 1:
                        heartOne.SetActive(true);
                        break;
                    case 2:
                        heartOne.SetActive(true);
                        heartTwo.SetActive(true);
                        break;
                    case 3:
                        heartOne.SetActive(true);
                        heartTwo.SetActive(true);
                        heartThree.SetActive(true);
                        break;
                }
        
                foreach (Transform child in futureToken.transform.parent)
                    child.gameObject.SetActive(false);
                switch (_levelManager.levelIndex)
                {
                    case 2:
                        futureToken.SetActive(true);
                        break;
                    case 3:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        break;
                    case 4:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        leaderToken.SetActive(true);
                        break;
                    case 5:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        leaderToken.SetActive(true);
                        americaToken.SetActive(true);
                        break;
                }
            }
        }
    }
}
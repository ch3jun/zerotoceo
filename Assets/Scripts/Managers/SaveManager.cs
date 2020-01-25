﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static bool LoadFlag;

    private UIManager _uiManager;
    private MinigameManager _minigameManager;
    private LevelManager _levelManager;
    
    void Awake ()
    {
        _levelManager = GetComponent<LevelManager>();
        _uiManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        _uiManager.uiReadyEvent.AddListener(CheckLoadOrNew);
        _minigameManager = GetComponent<MinigameManager>();
    }

    [System.Serializable]
    public class Data
    {
        public int level;
        public int lives;
        public int points;
        public Dictionary<string, float[]> characterPositions;
        public Dictionary<string, string> dialogueData;
        public Dictionary<string, int> scoreboard;
        public string minigame;
        public int minigameProgression;
        public Dictionary<string, int> gameFlags;
    }

    private void CheckLoadOrNew()
    {
        if (LoadFlag)
        {
            Load();
            LoadFlag = false;
        } 
        else if (LevelManager.CurrentLevelType == LevelManager.LevelType.Level) // FOR DEBUG PURPOSES
        {
            New();
        }
    }

    public void Save ()
    {
        // Retrieve character positions
        var characters = new List<GameObject>();
        characters.AddRange(GameObject.FindGameObjectsWithTag("Player").ToList());
        characters.AddRange(GameObject.FindGameObjectsWithTag("NPC").ToList());
        var characterPositions = new Dictionary<string, float[]>();
        foreach (var controller in characters)
        {
            var controllerGameObject = controller.gameObject;
            var controllerVector = controllerGameObject.transform.position;
            var characterPosition = new float[3];
            characterPosition[0] = controllerVector.x;
            characterPosition[1] = controllerVector.y;
            characterPosition[2] = controllerVector.z;
            characterPositions.Add(controllerGameObject.name, characterPosition);
        }
        
        // Convert Ink list object to a dictionary
        var gameFlags = new Dictionary<string, int>();
        foreach (var item in DialogueManager.gameFlags)
        {
            gameFlags.Add(item.Key.ToString(), item.Value);
        }

        // Create new Data object with more retrieved data
        var data = new Data
        {
            level = _levelManager.LevelIndex,
            scoreboard = _levelManager.scoreboard,
            points = PlayerController.Points,
            lives = PlayerController.Lives,
            dialogueData = DialogueManager.SessionDialogueData,
            characterPositions = characterPositions,
            minigame = MinigameManager.MinigameID,
            minigameProgression = _minigameManager.minigameProgression,
            gameFlags = gameFlags
        };

        // Save data into system
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/savedata";
        var stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        
        _uiManager.QueuePopup("save");
    }

    public void Load ()
    {
        // Retrieve data from system
        var path = Application.persistentDataPath + "/savedata";
        if (!File.Exists(path)) return;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        if (data == null) return;
        
        // Apply data
        _levelManager.LevelIndex = data.level;
        _levelManager.scoreboard = data.scoreboard;
        PlayerController.Points = data.points;
        PlayerController.Lives = data.lives;
        DialogueManager.SessionDialogueData = data.dialogueData;
        MinigameManager.MinigameID = data.minigame;
        _minigameManager.minigameProgression = data.minigameProgression;

        // Convert dictionary to InkList and apply
        var gameFlags = new InkList();
        foreach (var item in data.gameFlags)
        {
            gameFlags.Add(new InkListItem(item.Key), item.Value);
        }
        DialogueManager.gameFlags = gameFlags;

        // Apply character positions
        foreach (KeyValuePair<string, float[]> entry in data.characterPositions)
        {
            Vector3 characterPosition;
            characterPosition.x = entry.Value[0];
            characterPosition.y = entry.Value[1];
            characterPosition.z = entry.Value[2];
            GameObject.Find(entry.Key).transform.position = characterPosition;
        }
    }

    public static int GetSavedLevelIndex()
    {
        // Retrieve data from system
        var path = Application.persistentDataPath + "/savedata";
        if (!File.Exists(path)) return 0;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        return data == null ? 0 : data.level;
    }
    
    private void New ()
    {
        PlayerController.Points = 0;
        PlayerController.Lives = 3;
        DialogueManager.SessionDialogueData = new Dictionary<string, string>();
        MinigameManager.MinigameID = "";
        DialogueManager.gameFlags = new InkList();
}
}
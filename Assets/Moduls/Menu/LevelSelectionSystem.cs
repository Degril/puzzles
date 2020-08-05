using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionSystem : MonoBehaviour
{
    private void Start()
    {
        var levelOptionList = GetComponentsInChildren<LevelButtonComponent>();
        int id = 1;
        foreach (var levelOption in levelOptionList)
        {
            levelOption.Text.text = id.ToString();
            levelOption.Button.onClick.AddListener(delegate { LoadLevel(levelOption.GameLoaderSettings); });
            id++;
        }
    }

    private void LoadLevel(GameLoaderData loaderData)
    {
        
        var gameLoader = Game.Get<GameLoader>();
        DontDestroyOnLoad(gameLoader.gameObject);
        gameLoader.Init(loaderData);
        SceneManager.LoadSceneAsync("game");
    }
}

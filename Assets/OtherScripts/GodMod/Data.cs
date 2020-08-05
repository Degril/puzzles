using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class Data : SingletonBase<Data>
{

    [JsonIgnore] private float _cooldownSeconds = 5;
    [JsonIgnore] private bool _isLoaded = false;

    protected override void OnGet()
    {
        if (_isLoaded == false)
            if (PlayerPrefs.HasKey("Data"))
                _instance = JsonConvert.DeserializeObject<Data>(PlayerPrefs.GetString("Data"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("Data", JsonConvert.SerializeObject(_instance));
    }

    public float MusicVolume { get; set; }
    public float AllSoundVolume { get; set; }
}

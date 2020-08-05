using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSystem : MonoBehaviour
{
    [SerializeField] private Slider _allSoundSlider;
    [SerializeField] private Slider _musicSlider;

    private void Start()
    {
        _allSoundSlider.onValueChanged.AddListener(delegate(float value)  { OnAllSoundVolumeChanged(value); });
        _musicSlider.onValueChanged.AddListener(delegate(float value)  { OnMusicVolumeChanged(value); });
        _allSoundSlider.value = Game.Get<Data>().AllSoundVolume;
        _musicSlider.value = Game.Get<Data>().MusicVolume;
        
        Game.Get<AutoSaver>().AutoSaveStart();
    }
    
    public void OnAllSoundVolumeChanged(float value)
    {
        Game.Get<Data>().AllSoundVolume = value;
    }
    
    public void OnMusicVolumeChanged(float value)
    {
        Game.Get<Data>().MusicVolume = value;
    }
}

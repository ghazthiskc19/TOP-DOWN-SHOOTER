using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public static VolumeController instance;
    public AudioSource SFXSource;
    public AudioSource BGMSource;
    public Slider SFXSliderMainMenu;
    public Slider BGMSliderMainMenu;
    public Slider SFXSliderSideMenu;
    public Slider BGMSliderSideMenu;

    private bool isUpdateSFX = false;
    private bool isUpdateBGM = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        SFXSliderMainMenu.value = SFXSource.volume;
        SFXSliderSideMenu.value = SFXSource.volume;

        BGMSliderMainMenu.value = BGMSource.volume;
        BGMSliderSideMenu.value = BGMSource.volume;

        SFXSliderMainMenu.onValueChanged.AddListener(ChangeVolumeSFX);
        SFXSliderSideMenu.onValueChanged.AddListener(ChangeVolumeSFX);
    
        BGMSliderMainMenu.onValueChanged.AddListener(ChangeVolumeBGM);
        BGMSliderSideMenu.onValueChanged.AddListener(ChangeVolumeBGM);
    }

    private void ChangeVolumeSFX(float value)
    {
        SFXSource.volume = value;
        SFXSliderMainMenu.value = value;
        SFXSliderSideMenu.value = value;
    }

    private void ChangeVolumeBGM(float value)
    {
        BGMSource.volume = value;
        BGMSliderMainMenu.value = value;
        BGMSliderSideMenu.value = value;
    }
    public void Save(ref VolumeGame data)
    {
        data.SFXVolume = SFXSource.volume;
        data.BGMVolume = BGMSource.volume;
    }

    public void Load(VolumeGame data)
    {
        SFXSource.volume = data.SFXVolume;
        BGMSource.volume = data.BGMVolume;
        ChangeVolumeSFX(SFXSource.volume);
        ChangeVolumeBGM(BGMSource.volume);
    }
}


[System.Serializable]
public struct VolumeGame
{
    public float SFXVolume;
    public float BGMVolume;
}
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource SFXSource;
    public AudioSource BGMSource;
    // create header for sound source
    [Header("SFX Sound Meele")]
    public AudioClip HitMeele;
    public AudioClip AttackMeele;
    public AudioClip SwitchMeele;
    [Header("SFX Sound Meele")]
    public AudioClip Pistol;
    public AudioClip Rifle;

    [Header("SFX Sound Weapon Reload")]
    public AudioClip revolverReload;
    public AudioClip SniperReload;
    [Header("SFX Sound Enemy Attack")]
    public AudioClip SeranganSuaraEnemy;
    public AudioClip SniperEnemy;

    [Header("SFX Sound Hit")]
    public AudioClip HitPlayer;
    public AudioClip HitEnemy;
    [Header("SFX Sound Jalan ")]
    public AudioClip JalanVer1;
    public AudioClip JalanVer2;
    public AudioClip JalanVer3;

    [Header("BGM Soudn Ambience ")]
    public AudioClip Ambience;
    public AudioSource reloadSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            reloadSource = gameObject.AddComponent<AudioSource>(); 
        }else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip audio)
    {
        SFXSource.PlayOneShot(audio);
    }

    public AudioSource PlayReloadSFX(AudioClip audio)
    {
        if(reloadSource.isPlaying){
            reloadSource.Stop();
        }
        reloadSource.clip = audio;
        reloadSource.Play();
        return reloadSource;
    }

    public void StopReloadSFX()
    {
        if(reloadSource.isPlaying){
            reloadSource.Stop();
        }
    }

    public void PlayBGM(AudioClip audio)
    {
        BGMSource.clip = audio;
        BGMSource.loop = true;
        BGMSource.Play();
    }
    public void StopBGM(AudioClip audio)
    {
        BGMSource.Stop();
    }
    public void PreloadBGM(AudioClip audio){
        BGMSource.clip = audio;
        BGMSource.loop = true;
        BGMSource.playOnAwake = false;
        BGMSource.Stop();
    }
}

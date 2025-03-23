using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    [SerializeField] private float _fadeDuration;
    private Volume _globalVolume;
    private ColorAdjustments _colorAdjustments; 
    private CanvasGroup canvasGroup;
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _globalVolume = GetComponent<Volume>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        instance = this;

        if(_globalVolume.profile.TryGet(out _colorAdjustments)){
            _colorAdjustments.active = false;
        }
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        gameObject.SetActive(false);
    }

    public void ScreenDeathAppear()
    {
        gameObject.SetActive(true);
        _colorAdjustments.active = true;

        LeanTween.value(gameObject, 0, 1, _fadeDuration)
            .setOnUpdate((float val) => _globalVolume.weight = val).setEaseOutExpo()
            .setOnComplete(() =>{
                LeanTween.alphaCanvas(canvasGroup, 1, _fadeDuration).setEaseOutExpo()
                .setOnComplete(() => Time.timeScale = 0);
            });
    }


    public void ScreenDeathDisappear()
    {
        StartCoroutine(DisableScreenDeath());
    }
    public IEnumerator DisableScreenDeath()
    {
        yield return new WaitUntil(() => Time.timeScale == 0);
        Time.timeScale = 1;
        _colorAdjustments.active = false;
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        playerMovement.gameObject.transform.position = RespawnController.instance.lastCheckpointPos;
        playerMovement.GetComponentInChildren<Animator>().SetBool("IsDead", false);
    }

    public void ExitGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

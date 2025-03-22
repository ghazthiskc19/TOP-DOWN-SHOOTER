using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayMen : MonoBehaviour
{
    public CanvasGroup OverlayCanvasGroup;
    public CanvasGroup PlayMenuUI;
    public GameObject batasAtas; 
    public GameObject batasBawah;
    public CanvasGroup[] overlay;
    public CanvasGroup[] overlayText;
    public UnityEvent AfterTransition;
    public UnityEvent BeforeTransition;
    public CinemachineVirtualCamera _CMVM;
    private float duration = 0.5f;
    private float normalOrthoSize = 5f;
    private float targetOrthoSize = 6f;
    public AnimationCurve animationCurve;
    private  PlayerMovement pm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _CMVM.m_Lens.OrthographicSize = targetOrthoSize;
        SoundManager.instance.PreloadBGM(SoundManager.instance.Ambience);
        pm = FindAnyObjectByType<PlayerMovement>();
        foreach (CanvasGroup cg in overlay){
            cg.alpha = 0f;
            cg.gameObject.SetActive(false);
        }

        foreach(CanvasGroup text in overlayText)
        {
            text.alpha  = 0f;
        }
    }
    public void FadeOutTransitionGone()
    {
        BeforeTransition?.Invoke();

        StartCoroutine(FirstOverlayTransition());
        RectTransform rtAtas = batasAtas.GetComponent<RectTransform>();
        RectTransform rtBawah = batasBawah.GetComponent<RectTransform>();
        float offsetAtas = rtAtas.rect.height;
        float offsetBawah = rtBawah.rect.height;

        float startFOV = _CMVM.m_Lens.OrthographicSize;
        float endPOV = normalOrthoSize;
        LeanTween.value(startFOV, endPOV, duration).setEase(animationCurve)
        .setOnUpdate((float val) => {
            _CMVM.m_Lens.OrthographicSize = val;
        }).setOnComplete(() => {
            LeanTween.moveY(rtAtas, rtAtas.anchoredPosition.y + offsetAtas, duration);
            LeanTween.moveY(rtBawah, rtBawah.anchoredPosition.y - offsetBawah, duration);
            LeanTween.alphaCanvas(OverlayCanvasGroup, 0, duration)
            .setOnComplete(() => {
                SoundManager.instance.PlayBGM(SoundManager.instance.Ambience);
                AfterTransition?.Invoke();
            });
        });
        LeanTween.alphaCanvas(PlayMenuUI, 0, duration);
    }

    public void StartTransitionWithLoading()
    {
        StartCoroutine(ShowLoadingAndStartTransition());
    }

    private IEnumerator ShowLoadingAndStartTransition()
    {
        foreach (CanvasGroup cg in overlay){
            cg.gameObject.SetActive(true);
        }

        // overlay pertama
        LeanTween.alphaCanvas(overlay[0], 1, 0.5f);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlayText[0], 1, 0.5f);
        yield return new WaitForSeconds(5f);
        LeanTween.alphaCanvas(overlayText[0], 0, 0.5f);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlay[0], 0, 0)
        .setOnComplete(() => {
            overlay[0].gameObject.SetActive(false);
            overlay[0].alpha = 1f; 
        });

        // overlay kedua
        LeanTween.alphaCanvas(overlay[1], 1, 0);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlayText[1], 1, 0.5f);
        yield return new WaitForSeconds(5f);
        LeanTween.alphaCanvas(overlayText[1], 0, 0.5f);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlay[1], 0, .5f)
        .setOnComplete(() => {
            overlay[1].gameObject.SetActive(false);
            overlay[1].alpha = 1f; 
        });

        // overlay keduketiga
        LeanTween.alphaCanvas(overlay[2], 1, 0);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlayText[2], 1, 0.5f);
        yield return new WaitForSeconds(5f);
        LeanTween.alphaCanvas(overlayText[2], 0, 0.5f);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(overlay[2], 0, .5f)
        .setOnComplete(() => {
            overlay[2].gameObject.SetActive(false);
            overlay[2].alpha = 1f; 
        });
        StartCoroutine(FirstOverlayTransition());
    }

    public IEnumerator FirstOverlayTransition()
    {
        yield return new WaitForSeconds(0);
        LeanTween.alphaCanvas(PlayMenuUI, 0, 0);
        RectTransform rtAtas = batasAtas.GetComponent<RectTransform>();
        RectTransform rtBawah = batasBawah.GetComponent<RectTransform>();

        float offsetAtas = rtAtas.rect.height;
        float offsetBawah = rtBawah.rect.height;

        float startFOV = _CMVM.m_Lens.OrthographicSize;
        float endPOV = normalOrthoSize;
        LeanTween.value(startFOV, endPOV, duration).setEase(animationCurve)
        .setOnUpdate((float val) => {
            _CMVM.m_Lens.OrthographicSize = val;
        }).setOnComplete(() => {
            LeanTween.moveY(rtAtas, rtAtas.anchoredPosition.y + offsetAtas, duration);
            LeanTween.moveY(rtBawah, rtBawah.anchoredPosition.y - offsetBawah, duration);
            LeanTween.alphaCanvas(OverlayCanvasGroup, 0, duration)
            .setOnComplete(() => {
                SoundManager.instance.PlayBGM(SoundManager.instance.Ambience);
                AfterTransition?.Invoke();
            });
        });
        _CMVM.OnTargetObjectWarped(pm.gameObject.transform, Vector3.zero);
    }

        public void FadeOutTransitionAppear()
    {
        PlayMenuUI.gameObject.SetActive(true);
        LeanTween.alphaCanvas(PlayMenuUI, 1, 0);
        SoundManager.instance.StopBGM(SoundManager.instance.Ambience);
        RectTransform rtAtas = batasAtas.GetComponent<RectTransform>();
        RectTransform rtBawah = batasBawah.GetComponent<RectTransform>();

        float offsetAtas = rtAtas.rect.height;
        float offsetBawah = rtBawah.rect.height;

        float startFOV = _CMVM.m_Lens.OrthographicSize;
        float endPOV = normalOrthoSize;

        LeanTween.value(endPOV, startFOV, duration).setEase(animationCurve)
        .setOnUpdate((float val) => {
            _CMVM.m_Lens.OrthographicSize = val;
        }).setOnComplete(() => {
            LeanTween.moveY(rtAtas, rtAtas.anchoredPosition.y - offsetAtas, duration);
            LeanTween.moveY(rtBawah, rtBawah.anchoredPosition.y + offsetBawah, duration);
            LeanTween.alphaCanvas(OverlayCanvasGroup, 1, duration)
            .setOnComplete(() => {
                AfterTransition?.Invoke();
            });
        });
        _CMVM.OnTargetObjectWarped(pm.gameObject.transform, Vector3.zero);
    }


    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}

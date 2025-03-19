using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayMen : MonoBehaviour
{
    public CanvasGroup OverlayCanvasGroup;
    public CanvasGroup PlayMenuUI;
    public GameObject batasAtas; 
    public GameObject batasBawah;
    public UnityEvent AfterTransition;
    public CinemachineVirtualCamera _CMVM;
    private float duration = 0.5f;
    private float normalOrthoSize = 5f;
    private float targetOrthoSize = 6f;
    public AnimationCurve animationCurve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _CMVM.m_Lens.OrthographicSize = targetOrthoSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOutTransition()
    {
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
                AfterTransition?.Invoke();
            });
        });
        LeanTween.alphaCanvas(PlayMenuUI, 0, duration);
    }
}

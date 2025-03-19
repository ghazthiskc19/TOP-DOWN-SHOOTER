using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SideMenu : MonoBehaviour
{
    public GameObject sideMenuPanel; // Panel yang akan muncul
    public Volume globalVolume; // Assign Volume dari Inspector
    private DepthOfField dof;
    private CanvasGroup sideMenuCanvasGroup;
    public CinemachineVirtualCamera _CMVM;
    private bool isPaused = false;
    private float slideDuration = 0.3f; // Durasi animasi
    private float zoom = 0.5f;
    private float normalOrthoSize = 5f;
    private float targetOrthoSize = 4f;
    public AnimationCurve animationCurve;

    void Start()
    {
        // Ambil Depth of Field dari Global Volume
        if (globalVolume.profile.TryGet(out dof))
        {
            dof.active = false;
        }

        sideMenuCanvasGroup = sideMenuPanel.GetComponent<CanvasGroup>();
        sideMenuCanvasGroup.alpha = 0;
        sideMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            OpenSideMenu();
        }
        else
        {
            CloseSideMenu();
        }
    }

    public void OpenSideMenu()
    {
        StartCoroutine(ZoomEffect(true));
        StartBlurEffect();
    }

    public void CloseSideMenu()
    {
        StartCoroutine(ZoomEffect(false));
        HideBlurEffect();
    }

   private void StartBlurEffect()
    {
        if (dof == null) return;
        dof.active = true;

        LeanTween.value(gameObject, 0, 1, slideDuration)
            .setOnUpdate((float val) => globalVolume.weight = val)
            .setEaseOutExpo().setIgnoreTimeScale(true).setDelay(slideDuration)
            .setOnComplete(() =>
            {
                sideMenuPanel.SetActive(true);
                LeanTween.alphaCanvas(sideMenuCanvasGroup, 1, 0.3f).setEaseInExpo()
                    .setOnComplete(() => Time.timeScale = 0);
            });
    }

    private void HideBlurEffect()
    {
        if (dof == null) return;

        Time.timeScale = 1;
        LeanTween.alphaCanvas(sideMenuCanvasGroup, 0, 0.3f).setEaseInExpo()
            .setOnComplete(() =>
            {
                sideMenuPanel.SetActive(false);

                LeanTween.value(gameObject, 1, 0, slideDuration)
                    .setOnUpdate((float val) => globalVolume.weight = val)
                    .setEaseInExpo().setIgnoreTimeScale(true)
                    .setOnComplete(() =>
                    {
                        dof.active = false;
                    });
            });
    }

    private IEnumerator ZoomEffect(bool zoomIn){
        float startFOV = _CMVM.m_Lens.OrthographicSize;
        float endPOV = zoomIn ? targetOrthoSize : normalOrthoSize;

        float t = 0;
        while(t < 1){
            t += Time.deltaTime;
            float elapsed = t / zoom;
            _CMVM.m_Lens.OrthographicSize = Mathf.Lerp(startFOV, endPOV, animationCurve.Evaluate(elapsed));
            yield return null;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SideMenu : MonoBehaviour
{
    public GameObject sideMenuPanel; // Assign panel dari inspector
    private RectTransform menuTransform;
    private bool isPaused = false;
    private float slideDuration = 0.5f; // Durasi animasi

    void Start()
    {
        menuTransform = sideMenuPanel.GetComponent<RectTransform>();
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
        LeanTween.cancel(menuTransform);
        if (isPaused)
        {
            LeanTween.moveX(menuTransform, 5, slideDuration).setEaseOutExpo()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => Time.timeScale = 0);
        }
        else
        {
            LeanTween.moveX(menuTransform, -menuTransform.rect.width, slideDuration).setEaseInExpo()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => Time.timeScale = 1);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        LeanTween.moveX(menuTransform, -menuTransform.rect.width, slideDuration).setEaseInExpo().setIgnoreTimeScale(true)
            .setOnComplete(() => Time.timeScale = 1);
    }

    // public void GoToMainMenu()
    // {
    //     Time.timeScale = 1;
    //     SceneManager.LoadScene("MainMenu");
    // }
}

using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
public class ScoreUI : MonoBehaviour
{
    private TMP_Text scoreText;
    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore(ScoreController scoreController)
    {
        scoreText.text = $"Score: {scoreController.Score}";
    }
}

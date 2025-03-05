using UnityEngine;
using UnityEngine.Events;
public class ScoreController : MonoBehaviour
{
    public UnityEvent OnScoreChanged;
    public float Score {get; private set; }

    public void AddScore(int amounts)
    {
        Score += amounts;
        OnScoreChanged.Invoke();
    }
}

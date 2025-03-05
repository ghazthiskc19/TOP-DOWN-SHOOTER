using UnityEngine;

public class EnemyScoreAllocator : MonoBehaviour
{
    [SerializeField] private int _amountScore;
    private ScoreController _scoreController;

    void Awake()
    {
        _scoreController = FindAnyObjectByType<ScoreController>();
    }
    public void ScoreAllocator(){
        _scoreController.AddScore(_amountScore);
    }
}

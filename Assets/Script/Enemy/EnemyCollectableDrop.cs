using UnityEngine;

public class EnemyCollectableDrop : MonoBehaviour
{
    [SerializeField] private float _chanceColletable;

    private CollectableSpawner _collectableSpawner;

    private void Awake()
    {
        _collectableSpawner = FindAnyObjectByType<CollectableSpawner>();
    }
    public void RandomlyDropCollectabe()
    {
        float random = Random.Range(0f, 1f);
        if(_chanceColletable >= random){
            _collectableSpawner.SpawnCollectable(transform.position);   
        }
    }
}

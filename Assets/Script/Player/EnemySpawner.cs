using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefabs;
    [SerializeField]
    private float _minTimeSpawn;
    [SerializeField]
    private float _maxTimeSpawn;
    private float _timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if(_timeUntilSpawn <= 0){
            Instantiate(_enemyPrefabs, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }
    void SetTimeUntilSpawn(){
        _timeUntilSpawn = Random.Range(_minTimeSpawn, _maxTimeSpawn);
    }
}

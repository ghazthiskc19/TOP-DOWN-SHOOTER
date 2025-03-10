using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _colletablePrefabs;

    public void SpawnCollectable(Vector2 position){
        int index = Random.Range(0, _colletablePrefabs.Count);
        var selectedColletable = _colletablePrefabs[index];
        Instantiate(selectedColletable, position, Quaternion.identity);
    }
}

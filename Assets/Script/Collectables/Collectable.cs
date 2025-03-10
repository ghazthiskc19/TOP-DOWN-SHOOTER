using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ICollectableBehaviour _colletableBehaviour;
    private void Awake()
    {
        _colletableBehaviour = GetComponent<ICollectableBehaviour>();
            if(_colletableBehaviour == null)
    {
        Debug.LogError("Component ICollectableBehaviour not found di game object: " + gameObject.name);
    }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if(player != null){
            _colletableBehaviour.OnCollected(player.gameObject);
            Destroy(gameObject);
        }    
    }
}

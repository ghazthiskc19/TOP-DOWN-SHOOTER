using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    private ICollectableBehaviour _colletableBehaviour;
    private bool collect = false;
    public UnityEvent QTECalls;
    private void Awake()
    {
        _colletableBehaviour = GetComponent<ICollectableBehaviour>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if(player != null && !collect){
            collect = true;
            _colletableBehaviour.OnCollected(player.gameObject);
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
        }    
    }
}

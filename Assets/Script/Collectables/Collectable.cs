using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    private ICollectableBehaviour _colletableBehaviour;
    private QTEManager _QTEManager;
    private bool collect = false;
    public UnityEvent QTECalls;
    private void Awake()
    {
        _colletableBehaviour = GetComponent<ICollectableBehaviour>();
        _QTEManager = FindAnyObjectByType<QTEManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if(player != null && !collect){
            collect = true;
            _colletableBehaviour.OnCollected(player.gameObject);
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            QTECalls.Invoke();
            Destroy(gameObject, 1);
        }    
    }
}

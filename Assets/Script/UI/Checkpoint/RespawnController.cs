using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;
    public Vector3 lastCheckpointPos;
    public GameObject _interractText;
    [SerializeField] private bool hasReachCheckpoint;
    private void Awake()
    {
        instance = this;
        hasReachCheckpoint = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            lastCheckpointPos = collision.transform.position;
            StartCoroutine(CheckpointUI(2));
        }
    }

    private IEnumerator CheckpointUI(float duration)
    {
        if(!hasReachCheckpoint){
            SaveSystem.Save();
            _interractText.GetComponent<CanvasGroup>().alpha = 1;
            _interractText.GetComponentInChildren<TMP_Text>().text = "You reach checkpoint and game will be saved";
            yield return new WaitForSeconds(duration);
            _interractText.GetComponent<CanvasGroup>().alpha = 0;
            hasReachCheckpoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasReachCheckpoint = false;
    }
}

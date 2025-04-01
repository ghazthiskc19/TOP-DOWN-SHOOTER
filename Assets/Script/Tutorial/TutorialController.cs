using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private float offset = 5f;
    [SerializeField] private float duration = .3f;
    [SerializeField] private float delayPerText = .2f;
    private CanvasGroup[] canvasGroups;
    private void Start()
    {
        canvasGroups = GetComponentsInChildren<CanvasGroup>();
        for(int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = 0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerMovement>())
        {
            for(int i = 0; i < canvasGroups.Length; i++){
                LeanTween.alphaCanvas(canvasGroups[i], 1, duration).setDelay(delayPerText * i);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Keluar");
        if(collision.GetComponent<PlayerMovement>())
        {
            for(int i = 0; i < canvasGroups.Length; i++){
                LeanTween.alphaCanvas(canvasGroups[i], 0, duration).setDelay(delayPerText * i);
            }
        }
    }
}

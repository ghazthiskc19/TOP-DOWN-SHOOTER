using System.Collections;
using TMPro;
using UnityEngine;

public class HealthCollectableBehaviour : MonoBehaviour, ICollectableBehaviour
{
    [SerializeField] private float _healthAmount;
    [SerializeField] private GameObject _textCollectable;

    public void OnCollected(GameObject player){
        player.GetComponent<HealthController>().AddHealth(_healthAmount);
        UIOnCollected();
    }

    private void UIOnCollected(){

        if(_textCollectable != null){
            GameObject textObj = Instantiate(_textCollectable, transform.position + Vector3.up * 1, Quaternion.identity);
            TextMeshPro tmp = textObj.GetComponent<TextMeshPro>();
            textObj.transform.SetParent(null);

            textObj.GetComponent<TextMeshPro>().text = "+" + _healthAmount.ToString() + " Health";
            StartCoroutine(UIFadeAndDestroy(textObj, tmp));
        }
    }
    private IEnumerator UIFadeAndDestroy(GameObject textObj, TextMeshPro tmp)
    {
        float duration = 1f;
        float elapsedTime = 0;
        Vector3 startPos = textObj.transform.position;
        Vector3 endPos = startPos + Vector3.up * 1.5f;

        while(elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            textObj.transform.position = Vector3.Lerp(startPos, endPos, t);
            tmp.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        Destroy(textObj);
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyQTEEvent : MonoBehaviour, IPointerDownHandler
{
    public static event Action<GameObject, bool> OnButtonDestroyed;
    public float selfDestroyTimer;
    private UnityEngine.UI.Image _foregroundFill;
    public Coroutine currentCoroutine;
    private void Start()
    {
        if(MyQTEManager.instance.QTEIsStart){
            StartCoroutine(OnSelfDestroys(selfDestroyTimer));
            _foregroundFill = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); 
            StartCoroutine(FillAnimation(selfDestroyTimer));
        }
    }

    public IEnumerator OnSelfDestroys(float timer){
        yield return new WaitForSeconds(timer);
        OnButtonDestroyed?.Invoke(gameObject, false);
        Destroy(gameObject);
    }

    private IEnumerator FillAnimation(float timer)
    {
        float t = 0;
        while (t < 1){
            t += Time.deltaTime / timer;
            _foregroundFill.fillAmount = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonDestroyed?.Invoke(gameObject, true);
        Destroy(gameObject);
    }

}

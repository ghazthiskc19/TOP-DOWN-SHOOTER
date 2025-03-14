using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class DetectQTE : MonoBehaviour
{

    public Vector2 direction; 
    public float eventRange;
    public Vector2 QTESize;
    public float QTEAngle;
    public LayerMask playerMask;
    public GameObject QTEWrapper;
    private Vector2 QTECenter;
    private MyQTEManager _myQTEManager;
    private UnityEngine.UI.Image _foregroundCureFill;
    private Transform curingBar;
    public AnimationCurve animationCurve;
    private bool QTEDone = false;

    void Start()
    {
        _myQTEManager = FindAnyObjectByType<MyQTEManager>();
        Transform canvas = transform.Find("Canvas");
        curingBar = canvas.Find("CuringBar");
        _foregroundCureFill = curingBar.Find("Foreground").GetComponent<UnityEngine.UI.Image>();
        _myQTEManager.OnPointChanged += UpdateFillAmount;
        curingBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        QTECenter = (Vector2) transform.position + direction;
        Collider2D[] detectPlayerCollider = Physics2D.OverlapBoxAll(QTECenter, QTESize, QTEAngle, playerMask);
        if (detectPlayerCollider.Length > 0 && Input.GetKeyDown(KeyCode.T) && !_myQTEManager.QTEIsStart && !QTEDone)
        {
            Animator playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
            if(playerAnimator != null) 
            {
                playerAnimator.SetFloat("Horizontal", 0f);
                playerAnimator.SetFloat("Vertical", 0f);
            }
            if(curingBar != null){
                curingBar.gameObject.SetActive(true);

            }
            Debug.Log(transform.gameObject);
            _myQTEManager.StartQTE(transform);
            _myQTEManager.WhenQTEEnd.AddListener(() => {
                StartCoroutine(HideWithDelay(1));
            });
            QTEDone = true;
        }
    }

    private void UpdateFillAmount(int current, int max)
    {
        if(_myQTEManager._activeQTE == this){
            float targetProgress = (float) current / max;
            StartCoroutine(LerpFillAmount(targetProgress));
        }
    }

    private IEnumerator LerpFillAmount(float targetFill)
    {
        float start = _foregroundCureFill.fillAmount;
        float elapsedTime = 0;
        float duration = 0.5f;

        while(elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _foregroundCureFill.fillAmount = Mathf.Lerp(start, targetFill, animationCurve.Evaluate(t));
            yield return null;
        }
        _foregroundCureFill.fillAmount = targetFill;
    }
    private IEnumerator HideWithDelay(int delay){
        yield return new WaitForSeconds(delay);
        curingBar.gameObject.SetActive(false);
        _foregroundCureFill.fillAmount = 0;
    }
    private void OnDrawGizmos()
    {
        QTECenter = (Vector2) transform.position + direction * eventRange;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(QTECenter, Quaternion.Euler(0, 0, QTEAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, QTESize);
    }
}

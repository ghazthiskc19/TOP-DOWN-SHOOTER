using System.Collections;
using TMPro;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

public class DetectQTE : MonoBehaviour
{
    public Vector2 direction; 
    public float eventRange;
    public Vector2 QTESize;
    public float QTEAngle;
    public LayerMask playerMask;
    public GameObject QTEWrapper;
    public GameObject interractText;
    public GameObject healthPrefabs;
    public float chanceDropHealth;
    private Vector2 QTECenter;
    private UnityEngine.UI.Image _foregroundCureFill;
    private Transform curingBar;
    public AnimationCurve animationCurve;
    public  bool QTEStatus;
    public bool LastWinResult;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    void Start()
    {
        QTEStatus = false;
        Transform canvas = transform.Find("Canvas");
        curingBar = canvas.Find("CuringBar");
        _foregroundCureFill = curingBar.Find("Foreground").GetComponent<UnityEngine.UI.Image>();
        MyQTEManager.instance.OnPointChanged += UpdateFillAmount;
        curingBar.gameObject.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.size = QTESize;
    }

    private void Update()
    {
        QTECenter = (Vector2) transform.position + direction;
        Collider2D[] detectPlayerCollider = Physics2D.OverlapBoxAll(QTECenter, QTESize, QTEAngle, playerMask);
        if (detectPlayerCollider.Length > 0 && Input.GetKeyDown(KeyCode.T) && !MyQTEManager.instance.QTEIsStart && !QTEStatus)
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

            MyQTEManager.instance.StartQTE(transform);
            StartCoroutine(WhenQTEIsEnd());
            QTEStatus = true;
        }
    }

    private IEnumerator WhenQTEIsEnd(){
        yield return new WaitUntil(() => MyQTEManager.instance.QTEIsEnd == true);
        StartCoroutine(HideWithDelay(1));

    }
    private void UpdateFillAmount(int current, int max)
    {
        if(MyQTEManager.instance._activeQTE == this){
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

        // Interract text will invisible when QTE Event is done
        interractText.GetComponent<CanvasGroup>().alpha = 0f;
        ChangeAnimationNPC(QTEStatus);
    }

    public void ChangeAnimationNPC(bool status)
    {
        if(status){
            if(MyQTEManager.instance.LastQTEWin){
                _animator.SetTrigger("IsWin");
                Vector3 healtPos = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
                if(Random.Range(0, 1) > chanceDropHealth)
                {
                    GameObject healthCollect = Instantiate(healthPrefabs, healtPos, Quaternion.identity);
                }
                Instantiate(healthPrefabs, healtPos, Quaternion.identity);
            }else{
                _animator.SetTrigger("IsLose");
            }
        }
    }
    // Override function buat sprite NPC ketika dapet data dari save system
        public void ChangeAnimationNPC(bool status, bool lastWin)
    {
        if(status){
            if(lastWin){
                _animator.SetTrigger("IsWin");
            }else{
                _animator.SetTrigger("IsLose");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerMovement>() && !QTEStatus){
            interractText.GetComponent<CanvasGroup>().alpha = 1f;
            interractText.GetComponentInChildren<TMP_Text>().text = "Press <T> to interract";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() && !QTEStatus) interractText.GetComponent<CanvasGroup>().alpha = 0f;
    }
        
    private void OnDrawGizmos()
    {
        QTECenter = (Vector2) transform.position + direction * eventRange;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(QTECenter, Quaternion.Euler(0, 0, QTEAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, QTESize);
    }
}

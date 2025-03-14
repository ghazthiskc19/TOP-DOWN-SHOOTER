using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Cinemachine.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MyQTEManager : MonoBehaviour
{
    public static MyQTEManager instance; 
    [SerializeField] private int _maxPoints;
    public int currentPoint;
    public int generateButton;
    public GameObject QTEWrapper;
    public GameObject QTEPrefabsButton;
    private BoxCollider2D QTEOffset;
    public CinemachineVirtualCamera _CMVM;
    public GameObject CameraPosition;
    public CanvasGroup UIWrapper;
    public bool QTEIsStart = false;
    public float normalOrthoSize;
    public float targetOrthoSize;
    public float speedZoom;
    public float delayBetweenPoints;
    public float delayBetweenSpawn;
    public AnimationCurve animationCurve;
    public UnityEvent WhenQTEStart;
    public UnityEvent WhenQTEEnd;
    private List<GameObject> listQTEButtons = new List<GameObject>();
    private bool btnSelfDestroy = false;
    public event Action<int, int> OnPointChanged;
    public DetectQTE _activeQTE;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        QTEOffset = QTEWrapper.GetComponent<BoxCollider2D>();
        instance = this;
    }
    void Update()
    {
        if(QTEIsStart && Input.GetMouseButtonDown(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject()){
                HandleClickError();
            }
        }
    }

    private void HandleClickError(){
        for(int i = listQTEButtons.Count - 1; i >= 0; i--){
            Destroy(listQTEButtons[i]);
        }
        listQTEButtons.Clear();
        EndQTE(false);
    }
    void OnEnable()
    {
        MyQTEEvent.OnButtonDestroyed  += RemoveQTEButton;        
    }
    void OnDisable()
    {
        MyQTEEvent.OnButtonDestroyed  -= RemoveQTEButton;        
    }
    public void StartQTE(Transform NPC)
    {
        if (QTEIsStart)
        {
            return;
        }
        _activeQTE = NPC.GetComponent<DetectQTE>();
        btnSelfDestroy = false;
        _CMVM.Follow = NPC.transform;
        StartCoroutine(ZoomEffect(true));
        QTEIsStart = true;
        currentPoint = 0;
        UIWrapper.alpha = 0f;
        StartCoroutine(FadeAnimation(UIWrapper.gameObject, true));
        WhenQTEStart?.Invoke();
        StartCoroutine(StartQTEUI());
    }
    private IEnumerator PlayQTE()
    {
        while(currentPoint < _maxPoints && QTEIsStart)
        {
            if(listQTEButtons.Count == 0)
            {
                yield return new WaitForSeconds(delayBetweenPoints);
                for(int i = 0; i < generateButton; i++)
                {
                    Vector2 randomPos = GetRandomPosition();
                    GameObject buttonObj = Instantiate(QTEPrefabsButton, randomPos, Quaternion.identity, QTEWrapper.transform);
                    yield return new WaitForSeconds(delayBetweenSpawn);
                    listQTEButtons.Add(buttonObj);
                }
            }
            yield return new WaitUntil(() => listQTEButtons.Count == 0);
        }
    }

    public void EndQTE(bool win)
    {   
        if(win)
        {
            Debug.Log("Menang cik!!");
        }
        else
        {
            Debug.Log("Kalah kocak");
        }
        _CMVM.Follow = CameraPosition.transform;
        StartCoroutine(ZoomEffect(false));
        StartCoroutine(FadeAnimation(UIWrapper.gameObject, false));
        // UIWrapper.alpha = 1f;
        WhenQTEEnd?.Invoke();
        QTEIsStart = false;
    }
private Vector2 GetRandomPosition()
{
    Vector2 randomPos;
    bool isOverlapping;
    int maxAttempts = 100; 
    int attempts = 0;

    do
    {
        float randomPosX = UnityEngine.Random.Range(QTEOffset.bounds.min.x, QTEOffset.bounds.max.x);
        float randomPosY = UnityEngine.Random.Range(QTEOffset.bounds.min.y, QTEOffset.bounds.max.y);
        randomPos = new Vector2(randomPosX, randomPosY);

        // Cek apakah posisinya terlalu dekat dengan button lain
        isOverlapping = false;
        foreach (var button in listQTEButtons)
        {
            if (Vector2.Distance(randomPos, button.transform.position) < 200f)
            {
                isOverlapping = true;
                break;
            }
        }

        attempts++;
    } while (isOverlapping && attempts < maxAttempts);

    return randomPos;
}


    private void RemoveQTEButton(GameObject button, bool hit)
    {
        if(listQTEButtons.Contains(button)) listQTEButtons.Remove(button);
        if(hit)
        {
            if(listQTEButtons.Count == 0)
            {
                currentPoint++;
                OnPointChanged?.Invoke(currentPoint, _maxPoints);
                if(currentPoint == _maxPoints)
                {
                    EndQTE(true);
                }
            }
        }
        else
        {
            if(!btnSelfDestroy){
                Debug.Log($"Button Destroyed: {button.name}, Hit: {hit}");
                EndQTE(false);
                btnSelfDestroy = true;
            }
        }
    }
        private IEnumerator ZoomEffect(bool zoomIn){
        float startFOV = _CMVM.m_Lens.OrthographicSize;
        float endPOV = zoomIn ? targetOrthoSize : normalOrthoSize;

        float t = 0;
        while(t < 1){
            t += Time.deltaTime * speedZoom;
            _CMVM.m_Lens.OrthographicSize = Mathf.Lerp(startFOV, endPOV, animationCurve.Evaluate(t));
            yield return null;
        }
    }
    private IEnumerator StartQTEUI(){
        yield return new WaitForSeconds(2f);
        StartCoroutine(PlayQTE());
    }

    private IEnumerator FadeAnimation(GameObject button, bool oneToZero)
    {
        float startAlpha = 0f;
        float endAlpha = 1f;
        if(oneToZero){
            startAlpha = 1f;
            endAlpha = 0f;
        }
        float t = 0;
        float duration = 1f;
        while(t < 1){
            t += Time.deltaTime / duration;
            button.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }
    }
}

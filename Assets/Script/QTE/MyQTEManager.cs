using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    public GameObject startQTEFrepabs;
    private BoxCollider2D QTEOffset;
    public CinemachineVirtualCamera _CMVM;
    public GameObject CameraPosition;
    public CanvasGroup UIWrapper;
    public bool QTEIsStart = false;
    public bool QTEIsEnd;
    public Canvas MainCanvas;
    private bool btnSelfDestroy = false;
    public bool LastQTEWin {get; set;}
    public float normalOrthoSize;
    public float targetOrthoSize;
    public float speedZoom;
    public float delayBetweenPoints;
    public float delayBetweenSpawn;
    public AnimationCurve animationCurve;
    public UnityEvent WhenQTEStart;
    public UnityEvent WhenQTEEnd;
    private List<GameObject> listQTEButtons = new List<GameObject>();
    public event Action<int, int> OnPointChanged;
    public DetectQTE _activeQTE;
    public SanityController sanity;
    public int sanityAmount;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        QTEOffset = QTEWrapper.GetComponent<BoxCollider2D>();
        
        if(instance == null) {
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
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
        Debug.Log("HadleClicknya");
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
        QTEIsEnd = false;
        if (QTEIsStart)
        {
            return;
        }

        GameObject startUI = Instantiate(startQTEFrepabs, MainCanvas.transform);
        StartCoroutine(StartQTEUI(NPC, startUI));
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
            PlayerInformation.instance.AddCure();
            sanity.gainSanity(sanityAmount);
        }
        else
        {
            Debug.Log("Kalah kocak");
            sanity.lostSanity(sanityAmount);
        }
        _CMVM.Follow = CameraPosition.transform;
        StartCoroutine(ZoomEffect(false));
        StartCoroutine(FadeAnimation(UIWrapper.gameObject, false));
        LastQTEWin = win;
        WhenQTEEnd?.Invoke();
        QTEIsStart = false;
        QTEIsEnd = true;
        _activeQTE.LastWinResult = win;
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
    private IEnumerator StartQTEUI(Transform NPC, GameObject UI){
        CanvasGroup UICanvas = UI.GetComponent<CanvasGroup>();
        RectTransform UIRect = UI.GetComponent<RectTransform>();
        
        UICanvas.alpha = 0f;
        UIRect.anchoredPosition = new Vector2(0, -300);

        // Move UI QTE 
        LeanTween.alphaCanvas(UICanvas, 1, 0.4f).setEaseOutExpo();
        LeanTween.moveY(UIRect, 0, 0.5f).setEaseOutExpo();

        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(UICanvas, 0, 0.4f).setEaseOutExpo();
        LeanTween.moveY(UIRect, 300, 0.5f).setEaseOutExpo();

        // Hide Player UI
        UIWrapper.alpha = 0f;
        StartCoroutine(FadeAnimation(UIWrapper.gameObject, true));
        yield return new WaitForSeconds(1f);
        UI.SetActive(false);

        // Zoom effect
        _CMVM.Follow = NPC.transform;
        StartCoroutine(ZoomEffect(true));
        _activeQTE = NPC.GetComponent<DetectQTE>();
        btnSelfDestroy = false;
        QTEIsStart = true;
        currentPoint = 0;
        WhenQTEStart?.Invoke();
        yield return new WaitForSeconds(0.5f);
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

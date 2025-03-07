using System.Collections;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform[] _crosshairPartsTrasnform;
    [SerializeField] private float _expandOffset;
    [SerializeField] private float _expandSpeed;
    [SerializeField] private float _crosshairOffset;
    private PlayerMovement _playerMovement;
    private Vector3[] _originalPosition;
    private Vector3[] _expandDirections = new Vector3[] {
        new Vector3(-1, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(-1, -1, 0),
        new Vector3(1, -1, 0)
    };
    void Awake()
    {
        _camera = Camera.main;
        _originalPosition = new Vector3[_crosshairPartsTrasnform.Length];
        for(int i = 0; i < _crosshairPartsTrasnform.Length; i++)
        {
            _originalPosition[i] = _crosshairPartsTrasnform[i].localPosition;
        }
        // Cursor.visible = false;
    }
    void Start()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
    }
    void Update()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = new Vector2(_crosshairOffset, -_crosshairOffset);
        transform.position = mousePos + offset;
        transform.position = Vector2.Lerp(transform.position, mousePos + offset, Time.deltaTime * 20);
        
        if(_playerMovement._isAiming){
            SetCrosshairVisibility(true);
        }else{
            SetCrosshairVisibility(false);
        }
    }

    public void SetCrosshairVisibility(bool isVisible){
        for(int i = 0; i < _crosshairPartsTrasnform.Length; i++){
            _crosshairPartsTrasnform[i].gameObject.SetActive(isVisible);
        }
    }

    public void ExpandCrosshair()
    {
        StopAllCoroutines();
        StartCoroutine(ExpandCoroutine());
    }

    public void ResetCrosshair(){
        StopAllCoroutines();
        StartCoroutine(ResetCoroutine());
    }

    private IEnumerator ExpandCoroutine(){
        float t = 0;
        Vector3[] targetPositions = new Vector3[_crosshairPartsTrasnform.Length];
        for(int i = 0; i < _crosshairPartsTrasnform.Length; i++){
                targetPositions[i] = _originalPosition[i] + (_expandDirections[i] * _expandOffset);
        }
        while(t < 1){
            t += Time.deltaTime * _expandSpeed;
            for(int i = 0; i < _crosshairPartsTrasnform.Length; i++)
            {
                _crosshairPartsTrasnform[i].localPosition = Vector3.Lerp(_originalPosition[i], targetPositions[i], t);
            }
            yield return null;
        }
    }

    private IEnumerator ResetCoroutine(){
        float t = 0;
        Vector3[] currentPositions = new Vector3[_crosshairPartsTrasnform.Length];
        for(int i = 0; i < _crosshairPartsTrasnform.Length; i++){
            currentPositions[i] = _crosshairPartsTrasnform[i].localPosition;
        }

        while(t < 1){
            t += Time.deltaTime * _expandSpeed;
            for(int i = 0; i < _crosshairPartsTrasnform.Length; i++)
            {
                _crosshairPartsTrasnform[i].localPosition = Vector3.Lerp(currentPositions[i], _originalPosition[i], t);
            }
            yield return null;
        }
    }

    public void SetReloadEffect(){
        // Add reload effect here

    }
}

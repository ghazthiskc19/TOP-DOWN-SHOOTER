using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _threshold;
    // Update is called once per frame
    void Update()
    {  
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (_playerTransform.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _playerTransform.position.x, _threshold + _playerTransform.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _playerTransform.position.y, _threshold + _playerTransform.position.y);

        this.transform.position = targetPos;
    }
}

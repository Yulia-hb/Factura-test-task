using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;   // машина
    [SerializeField] private Vector3 _offset;     // відстань від машини
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;
        //Vector3 desiredPosition = _target.position + _offset;
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        transform.LookAt(_target); // камера дивиться на машину
    }
}

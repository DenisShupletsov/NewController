using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    private Vector3 _correctPosition;
    //[SerializeField] private Quaternion _correctRotation;
    private Transform _cameraTransform;

    private float angle = -1.5f;
    private float angle1 = 4f;
	private const float _speed = 0.3f;
    private const float _originalRadius = 8f;
    private float _radius;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cameraTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        angle1 = Mathf.Clamp(angle1, -0.5f, 8f) + _speed * Input.GetAxis("Mouse Y");
        angle += -_speed * Input.GetAxis("Mouse X");
        _radius = _originalRadius + Mathf.Sin(angle1);
    }

    private void FixedUpdate()
    {
        Vector3 B = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        Vector3 newPos = _targetTransform.position + (B * _radius) + new Vector3(0, angle1, 0);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, newPos, 0.5f);
        _cameraTransform.LookAt(_targetTransform);        
    }
}
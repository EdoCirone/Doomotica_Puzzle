using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private Transform _cameraPivot;

    [SerializeField] private float _cameraSensitivity = 10f;
    [SerializeField] private float _cameraRotationSensitivity = 10f;
    [SerializeField] private float _cameraZoomSensitivity = 10f;

    Vector3 _moveVector = Vector3.zero;
    Vector3 _rotationVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        _moveVector.x = Input.GetAxis("Horizontal");
        _moveVector.z = Input.GetAxis("Vertical");

        CameraMovement();
        CameraRotation();
        CameraZoom();


    }

    public void CameraMovement()
    {
        _moveVector = _cameraPivot.transform.right * _moveVector.x + _cameraPivot.transform.forward * _moveVector.z;

        _cameraPivot.transform.position += _moveVector * _cameraSensitivity * Time.deltaTime;

    }

    public void CameraRotation()
    {
        _rotationVector = Vector3.zero;

        if (Input.GetMouseButton(2))
        {

            _cameraPivot.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 5f, Space.World);


        }

        if (Input.GetKey(KeyCode.Q))
        {

            _rotationVector.y = _cameraRotationSensitivity * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.E))
        {
            _rotationVector.y = -(_cameraRotationSensitivity * Time.deltaTime);

        }
        _cameraPivot.transform.eulerAngles += _rotationVector;
    }

    public void CameraZoom()
    {
        float zoomChange = Input.GetAxis("Mouse ScrollWheel");
        if (zoomChange != 0f)
        {
            _mainCamera.m_Lens.FieldOfView -= zoomChange * _cameraZoomSensitivity;
            _mainCamera.m_Lens.FieldOfView = Mathf.Clamp(_mainCamera.m_Lens.FieldOfView, 20f, 60f);
        }
    }
}
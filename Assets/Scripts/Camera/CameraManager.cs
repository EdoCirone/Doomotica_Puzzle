using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header ("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private Transform _cameraPivot;

    [Header("Camera Sensitivity")]
    [SerializeField] private float _cameraSensitivity = 10f;
    [SerializeField] private float _mouseSpeed = 3f;


    [SerializeField] private float _cameraRotationSensitivity = 10f;
    [SerializeField] private float _cameraZoomSensitivity = 10f;
    [SerializeField] private float _cameraPitchSensitivity = 10f;

    [Header("Camera Bounds")]
    [SerializeField] private Vector2 _xLimits = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 _zLimits = new Vector2(-10f, 10f);

    [Header("Pitch Limits")]
    [SerializeField] private float _minPitch = 10f;   // più “orizzontale”
    [SerializeField] private float _maxPitch = 80f;   // più “in giù”

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

        //Blocco del cursore al click centrale
        if (Input.GetMouseButtonDown(2))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetMouseButtonUp(2))
            Cursor.lockState = CursorLockMode.None;


        CameraMovement();
        CameraRotation();
        CameraZoom();


    }

    public void CameraMovement()
    {
        // Ottieni i vettori orizzontali (ho eliminato la componente Y)
        Vector3 right = _cameraPivot.transform.right;
        Vector3 forward = _cameraPivot.transform.forward;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Movimento con tastiera
        Vector3 moveInput = right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");

        // Movimento con mouse (click destro)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 moveByMouse = (right * mouseX + forward * mouseY) * -_mouseSpeed * _cameraSensitivity * Time.deltaTime;
            _cameraPivot.position += moveByMouse;
        }

        // Applica il movimento totale
        _cameraPivot.position += moveInput * _cameraSensitivity * Time.deltaTime;

        // Limita il movimento dentro il perimetro
        Vector3 clampedPos = _cameraPivot.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, _xLimits.x, _xLimits.y);
        clampedPos.z = Mathf.Clamp(clampedPos.z, _zLimits.x, _zLimits.y);
        _cameraPivot.position = clampedPos;
    }


    public void CameraRotation()
    {
        _rotationVector = Vector3.zero;

        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // YAW (sinistra-destra)
            _cameraPivot.Rotate(Vector3.up * mouseX * _cameraRotationSensitivity * Time.deltaTime, Space.World);

            // PITCH (su-giù) – ruota attorno all’asse X locale
            Vector3 euler = _cameraPivot.localEulerAngles;
            float pitch = euler.x;

            if (pitch > 180f) pitch -= 360f;
            pitch -= mouseY * _cameraPitchSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, _minPitch, _maxPitch);

            euler.x = pitch;
            _cameraPivot.localEulerAngles = euler;
        }

        // Rotazione con Q/E
        if (Input.GetKey(KeyCode.Q))
            _rotationVector.y = _cameraRotationSensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            _rotationVector.y = -_cameraRotationSensitivity * Time.deltaTime;

        _cameraPivot.transform.eulerAngles += _rotationVector;
    }



    public void CameraZoom()
    {
        float zoomChange = Input.GetAxis("Mouse ScrollWheel");

        // Zoom da tastiera
        if (Input.GetKey(KeyCode.Z))
            zoomChange = 0.05f; 
        if (Input.GetKey(KeyCode.X))
            zoomChange = -0.05f; 

        if (zoomChange != 0f)
        {
            _mainCamera.m_Lens.FieldOfView -= zoomChange * _cameraZoomSensitivity;
            _mainCamera.m_Lens.FieldOfView = Mathf.Clamp(_mainCamera.m_Lens.FieldOfView, 20f, 60f);
        }
    }
}
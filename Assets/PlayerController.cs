using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    [Header("References")]
    public Transform eyes; // The player camera transform (named "eyes")
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Crouch Settings")]
    public float normalHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 10f;
    private CharacterController controller;
    private float pitch = 0f;
    private bool isCrouching = false;

    private float targetEyeHeight;
    private Vector3 eyesLocalPosOriginal;

    [Header("Shooting")]
    public ParticleSystem muzzleFlash;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        eyesLocalPosOriginal = eyes.localPosition;
        targetEyeHeight = eyes.localPosition.y;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleCrouch();
        HandleShooting();

        SmoothEyeHeight();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        eyes.localEulerAngles = Vector3.right * pitch;
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            StandUp();
        }
    }

    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Quaternion flippedRotation = firePoint.rotation * Quaternion.Euler(0f, 180f, 0f);
        Instantiate(bulletPrefab, firePoint.position, flippedRotation);
        muzzleFlash.Play();
    }

    void Crouch()
    {
        isCrouching = true;
        controller.height = crouchHeight;
        controller.center = new Vector3(0, crouchHeight / 2f, 0);

        float heightDifference = normalHeight - crouchHeight;
        targetEyeHeight = eyesLocalPosOriginal.y - heightDifference;
    }

    void StandUp()
    {
        isCrouching = false;
        controller.height = normalHeight;
        controller.center = new Vector3(0, normalHeight / 2f, 0);

        targetEyeHeight = eyesLocalPosOriginal.y;
    }

    void SmoothEyeHeight()
    {
        Vector3 currentPos = eyes.localPosition;
        float newY = Mathf.Lerp(currentPos.y, targetEyeHeight, Time.deltaTime * crouchTransitionSpeed);
        eyes.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
    }
}

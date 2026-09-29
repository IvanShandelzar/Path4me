using UnityEngine;

// SwordRoot rotation = (0, 90, 30), position = (0.35, -0.45, 0.5)
public class WeaponAnimator : MonoBehaviour
{
    [Header("Idle Sway")]
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySpeed = 2f;
    [SerializeField] private float maxSway = 0.04f;

    [Header("Walk Bob")]
    [SerializeField] private float bobAmount = 0.03f;
    [SerializeField] private float bobSpeed = 10f;

    [Header("Attack Swing")]
    [SerializeField] private float swingDuration = 0.25f;
    [SerializeField] private Vector3 swingRotation = new Vector3(-90f, 20f, 0f);
    [SerializeField] private Vector3 swingPosition = new Vector3(0f, -0.1f, -0.1f);

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float swingTimer = 0f;
    private bool isSwinging = false;
    private float bobTimer = 0f;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (isSwinging)
        {
            HandleSwing();
        }
        else
        {
            HandleSway();
            HandleWalkBob();
        }

        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartSwing();
        }
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 sway = new Vector3(-mouseX, -mouseY, 0f) * swayAmount;
        sway = Vector3.ClampMagnitude(sway, maxSway);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPosition + sway,
            Time.deltaTime * swaySpeed * 10f
        );
    }

    void HandleWalkBob()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool moving = new Vector2(x, z).magnitude > 0.1f;

        if (moving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bobX = Mathf.Sin(bobTimer) * bobAmount;
            float bobY = Mathf.Abs(Mathf.Cos(bobTimer)) * bobAmount;

            transform.localPosition = initialPosition + new Vector3(bobX, -bobY, 0f);
        }
        else
        {
            bobTimer = 0f;
        }
    }

    void StartSwing()
    {
        isSwinging = true;
        swingTimer = 0f;
    }

    void HandleSwing()
    {
        swingTimer += Time.deltaTime;
        float t = swingTimer / swingDuration;

        if (t >= 1f)
        {
            isSwinging = false;
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
            return;
        }

        // Дуга удара: вниз-вперёд и обратно
        float curve = Mathf.Sin(t * Mathf.PI); // 0 -> 1 -> 0
        transform.localRotation = initialRotation * Quaternion.Euler(swingRotation * curve);
        transform.localPosition = initialPosition + swingPosition * curve;
    }
}
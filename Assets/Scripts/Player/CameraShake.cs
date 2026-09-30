using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float defaultDuration = 0.15f;
    [SerializeField] private float defaultMagnitude = 0.08f;

    private Vector3 initialLocalPosition;
    private float shakeTimer;
    private float currentDuration;
    private float currentMagnitude;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(shakeTimer / currentDuration);
            float decay = progress * progress;
            Vector3 offset = Random.insideUnitSphere * currentMagnitude * decay;

            transform.localPosition = initialLocalPosition + offset;
        }
        else if (transform.localPosition != initialLocalPosition)
        {
            transform.localPosition = initialLocalPosition;
        }
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (duration < 0) duration = defaultDuration;
        if (magnitude < 0) magnitude = defaultMagnitude;

        currentDuration = duration;
        currentMagnitude = magnitude;
        shakeTimer = duration;
    }
}
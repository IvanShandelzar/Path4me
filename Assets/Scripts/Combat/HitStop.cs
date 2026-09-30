using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    [SerializeField] private float defaultDuration = 0.06f;
    [SerializeField] private float defaultTimeScale = 0.05f;

    private Coroutine currentRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Stop(float duration = -1f, float timeScale = -1f)
    {
        if (duration < 0) duration = defaultDuration;
        if (timeScale < 0) timeScale = defaultTimeScale;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DoStop(duration, timeScale));
    }

    IEnumerator DoStop(float duration, float timeScale)
    {
        float prevTimeScale = Time.timeScale;
        float prevFixedDelta = Time.fixedDeltaTime;

        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = prevTimeScale;
        Time.fixedDeltaTime = prevFixedDelta;
        currentRoutine = null;
    }
}
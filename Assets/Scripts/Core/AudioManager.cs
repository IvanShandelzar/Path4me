using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Player Combat")]
    [SerializeField] private AudioClip swordSwingClip;
    [SerializeField] private AudioClip swordHitClip;

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float swingVolume = 0.6f;
    [Range(0f, 1f)] [SerializeField] private float hitVolume = 1f;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D звук
    }

    public void PlaySwing()
    {
        if (swordSwingClip != null)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(swordSwingClip, swingVolume);
        }
    }

    public void PlayHit()
    {
        if (swordHitClip != null)
            source.PlayOneShot(swordHitClip, hitVolume);
    }
}
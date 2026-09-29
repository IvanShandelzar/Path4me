using UnityEngine;

public class ArenaPortal : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private GameObject portalVisual;

    void Start()
    {
        if (waveManager != null)
            waveManager.OnAllWavesComplete += Activate;

        if (portalVisual != null)
            portalVisual.SetActive(false);
    }

    void Activate()
    {
        if (portalVisual != null)
            portalVisual.SetActive(true);

        Debug.Log("Портал активирован!");
    }

    void OnDestroy()
    {
        if (waveManager != null)
            waveManager.OnAllWavesComplete -= Activate;
    }
}
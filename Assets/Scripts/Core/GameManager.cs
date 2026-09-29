using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private HashSet<string> completedTrials = new HashSet<string>();

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

    public void CompleteTrial(string trialId)
    {
        completedTrials.Add(trialId);
        Debug.Log($"Испытание '{trialId}' пройдено. Всего: {completedTrials.Count}");
    }

    public bool IsTrialCompleted(string trialId)
    {
        return completedTrials.Contains(trialId);
    }

    public int CompletedCount => completedTrials.Count;
}
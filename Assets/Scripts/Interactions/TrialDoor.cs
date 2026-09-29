using UnityEngine;
using UnityEngine.SceneManagement;

public class TrialDoor : Interactable
{
    [Header("Trial Settings")]
    [SerializeField] private string trialId = "warrior";
    [SerializeField] private string trialSceneName = "Trial_Warrior";

    public override void Interact(GameObject interactor)
    {
        if (string.IsNullOrEmpty(trialSceneName))
        {
            Debug.LogWarning($"TrialDoor '{name}': не задано имя сцены!", this);
            return;
        }
        SceneManager.LoadScene(trialSceneName);
    }

    public override string GetPrompt()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsTrialCompleted(trialId))
            return $"Войти снова: {trialId}";

        return $"Начать испытание: {trialId}";
    }
}
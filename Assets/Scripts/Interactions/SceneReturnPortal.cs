using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturnPortal : Interactable
{
    [SerializeField] private string targetSceneName = "Hub";
    [SerializeField] private string completedTrialId = "";

    public override void Interact(GameObject interactor)
    {
        if (!string.IsNullOrEmpty(completedTrialId) && GameManager.Instance != null)
        {
            GameManager.Instance.CompleteTrial(completedTrialId);
        }

        SceneManager.LoadScene(targetSceneName);
    }

    public override string GetPrompt() => "Вернуться в храм";
}
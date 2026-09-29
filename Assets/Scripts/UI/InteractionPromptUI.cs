using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text promptText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            panel.SetActive(false);
            return;
        }

        panel.SetActive(true);
        promptText.text = $"[E] {text}";
    }
}
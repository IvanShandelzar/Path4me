using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] protected string promptText = "Взаимодействовать";
    [SerializeField] protected bool canInteract = true;

    public virtual string GetPrompt() => promptText;

    public virtual bool CanInteract() => canInteract;

    public abstract void Interact(GameObject interactor);
}
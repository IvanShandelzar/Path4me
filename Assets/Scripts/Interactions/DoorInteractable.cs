using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private Vector3 openOffset = new Vector3(0, 0, 1.5f);
    private bool isOpen = false;
    private Vector3 closedPosition;

    void Start()
    {
        closedPosition = transform.position;
    }

    public override void Interact(GameObject interactor)
    {
        isOpen = !isOpen;
        transform.position = isOpen ? closedPosition + openOffset : closedPosition;
        canInteract = true; // дверь можно открывать бесконечно
    }

    public override string GetPrompt() => isOpen ? "Закрыть дверь" : "Открыть дверь";
}
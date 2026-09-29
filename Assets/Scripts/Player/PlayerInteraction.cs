using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    private IInteractable currentTarget;

    void Update()
    {
        DetectInteractable();
        HandleInput();
    }

    void DetectInteractable()
    {
        currentTarget = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                currentTarget = interactable;
            }
        }

        InteractionPromptUI.Instance?.Show(currentTarget != null ? currentTarget.GetPrompt() : null);
    }

    void HandleInput()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact(gameObject);
        }
    }
}
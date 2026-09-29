using UnityEngine;

public interface IInteractable
{
    /// Текст подсказки, который увидит игрок ("Открыть дверь", "Зажечь руну")
    string GetPrompt();

    /// Вызывается, когда игрок нажал E, глядя на объект
    void Interact(GameObject interactor);

    /// Можно ли взаимодействовать прямо сейчас (например, дверь уже открыта — нельзя)
    bool CanInteract();
}
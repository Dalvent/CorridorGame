namespace Script
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void Interact();
    }
}
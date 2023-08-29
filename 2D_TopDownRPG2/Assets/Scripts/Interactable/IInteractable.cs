/// <summary>
/// Only monobehaviour should be implemented this interface
/// </summary>
public interface IInteractable
{
    public void Interact();

    public void OnAssigned();
    public void OnCancelAssigned();
}

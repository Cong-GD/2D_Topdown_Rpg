using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private static IInteractable _currentAssigned;

    public static void Assign(IInteractable interactable)
    {
        _currentAssigned?.OnCancelAssigned();
        _currentAssigned = interactable;
        interactable.OnAssigned();
    }

    public static void CancelAssign(IInteractable interactable)
    {
        if(interactable == _currentAssigned)
        {
            _currentAssigned.OnCancelAssigned();
            _currentAssigned = null;
        }
    }

    private void Awake()
    {
        InputCentral.InputActions.Interact.Interact.performed += InteractWithCurrent;
    }

    private void OnDestroy()
    {
        InputCentral.InputActions.Interact.Interact.performed -= InteractWithCurrent;
    }

    public void InteractWithCurrent(InputAction.CallbackContext _)
    {
        if(_currentAssigned != null)
        {
            _currentAssigned.Interact();
        }
    }
}

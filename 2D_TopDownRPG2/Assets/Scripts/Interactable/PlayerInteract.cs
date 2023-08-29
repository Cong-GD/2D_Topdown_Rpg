using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private static IInteractable _currentAssigned;

    public static void Assign<T>(T interactable) where T : MonoBehaviour, IInteractable
    {
        if((MonoBehaviour)_currentAssigned != null)
        {
            _currentAssigned.OnCancelAssigned();
        }
        
        _currentAssigned = interactable;
        interactable.OnAssigned();
    }

    public static void CancelAssign<T>(T interactable) where T : MonoBehaviour, IInteractable
    {
        if(IsCurrentAssined(interactable))
        {
            _currentAssigned.OnCancelAssigned();
            _currentAssigned = null;
        }
    }

    public static bool IsCurrentAssined<T>(T interactable) where T : MonoBehaviour, IInteractable
    {
        return interactable == (MonoBehaviour)_currentAssigned;
    }

    private void Start()
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

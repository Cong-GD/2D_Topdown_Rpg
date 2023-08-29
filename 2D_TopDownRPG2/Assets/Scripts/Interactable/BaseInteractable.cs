using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected BaseIndicator indicator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerInteract.Assign(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteract.CancelAssign(this);
        }
    }

    protected virtual void OnDisable()
    {
        PlayerInteract.CancelAssign(this);
    }

    public virtual void OnAssigned()
    {
        if(indicator != null)
        {
            indicator.Active();
        }   
    }

    public virtual void OnCancelAssigned()
    {
        if (indicator != null)
        {
            indicator.Deactive();
        }
    }

    public abstract void Interact();
}

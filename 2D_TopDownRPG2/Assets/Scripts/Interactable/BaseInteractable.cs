using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected GameObject indicator;

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
            indicator.SetActive(true);
        }   
    }

    public virtual void OnCancelAssigned()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
    }

    public abstract void Interact();
}

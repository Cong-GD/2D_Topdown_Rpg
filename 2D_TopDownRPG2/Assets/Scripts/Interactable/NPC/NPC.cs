using UnityEngine;
using UnityEngine.Events;

public class NPC : BaseInteractable
{
    [SerializeField] private UnityEvent onCancelAssigned;
    [SerializeField] private DialogueObject dialogueObject;

    public override void Interact()
    {
        DialoguePanel.ShowDialogue(dialogueObject);
    }

    public override void OnCancelAssigned()
    {
        base.OnCancelAssigned();
        onCancelAssigned.Invoke();
    }
}

using UnityEngine;

public class NPC : BaseInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject shop;

    public override void Interact()
    {
        if (shop.activeSelf)
        {
            shop.SetActive(false);
            return;
        }
        DialoguePanel.ShowDialogue(dialogueObject);
    }

    public override void OnCancelAssigned()
    {
        base.OnCancelAssigned();
        shop.SetActive(false);
    }
}

using UnityEngine;

public class Portal : BaseInteractable
{
    [SerializeField] private string mapName;

    [TextArea]
    [SerializeField] private string message;

    public override void Interact()
    {
        ConfirmPanel.Ask(message, () => GameManager.Instance.ChangeMap(mapName));
    }
}
    
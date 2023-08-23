using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : BaseInteractable
{
    [SerializeField] private Canvas chatBox;
    [TextArea]
    public string message;

    public override void Interact()
    {
        StopAllCoroutines();
        StartCoroutine(ChatBoxDisplay());
    }

    private IEnumerator ChatBoxDisplay()
    {
        chatBox.gameObject.SetActive(true);
        var textBox = chatBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        textBox.text = "";
        var wait = new WaitForSeconds(0.07f);
        for (int i = 0; i < message.Length; i++)
        {
            textBox.text += message[i];
            yield return wait;
        }
        yield return new WaitForSeconds(2);
        chatBox.gameObject.SetActive(false);
    }

}

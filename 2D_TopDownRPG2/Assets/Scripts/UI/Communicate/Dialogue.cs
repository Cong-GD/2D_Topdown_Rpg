using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueObject testDialogue;

    [SerializeField] private float writeSpeed;

    private bool _isWriting = false;
    private Coroutine _writeRoutine;

    private Coroutine _clickStateRoutine;
    private bool _wasPointerClickThisFrame;

    private void Start()
    {
        ShowDialogue(testDialogue);
    }
    private void OnEnable()
    {
        _wasPointerClickThisFrame = false;
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        StartCoroutine(ShowDialogueCoroutine(dialogueObject));
    }

    public IEnumerator ShowDialogueCoroutine(DialogueObject dialogueObject)
    {
        yield return 2f.Wait();
        foreach (var sentence in dialogueObject.Sentences)
        {
            StartWrite(sentence);
            yield return new WaitWhile(() => _isWriting && !_wasPointerClickThisFrame);
            StopWrite();

            dialogueText.text = sentence.RichText(Color.green);
            yield return null;
            yield return new WaitUntil(() => _wasPointerClickThisFrame);
            yield return null;
        }
        dialogueText.text = string.Empty;
    }

    private IEnumerator TypeWritterCoroutine(string sentence)
    {
        _isWriting = true;
        dialogueText.text = string.Empty;
        int current = 0;
        while (current < sentence.Length - 1)
        {
            current += Mathf.Max(1, Mathf.FloorToInt(Time.fixedDeltaTime * writeSpeed));
            current = Mathf.Clamp(current, 0, sentence.Length - 1);
            dialogueText.text = sentence[..(current + 1)];
            yield return 0.1f.Wait();
        }
        _isWriting = false;
    }

    private void StartWrite(string sentence)
    {
        _writeRoutine = StartCoroutine(TypeWritterCoroutine(sentence));
    }

    private void StopWrite()
    {
        if (_writeRoutine != null)
        {
            StopCoroutine(_writeRoutine);
            _writeRoutine = null;
        }
        _isWriting = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickStateRoutine != null)
        {
            StopCoroutine(_clickStateRoutine);
            _clickStateRoutine = null;
        }
        _wasPointerClickThisFrame = true;
        _clickStateRoutine = StartCoroutine(ClearClickState());
    }

    private IEnumerator ClearClickState()
    {
        yield return null;
        _wasPointerClickThisFrame = false;
    }
}

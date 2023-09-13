using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialoguePanel : MonoBehaviour, IPointerClickHandler
{
    public static bool IsUsing = false;
    private static Action<DialogueObject> _showDialogueAction;

    public static void ShowDialogue(DialogueObject dialogueObject)
    {
        IsUsing = true;
        if (_showDialogueAction == null)
        {
            var dialoguePanel = FindAnyObjectByType<DialoguePanel>();
            if (dialoguePanel != null)
            {
                _showDialogueAction = dialoguePanel.ShowDialogueInternal;
                _showDialogueAction.Invoke(dialogueObject);
            }
            else
            {
                Debug.LogWarning("Can't found Diablogue Panel in scene");
            }
        }
        else
        {
            _showDialogueAction.Invoke(dialogueObject);
        }

    }

    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private OptionBox optionBox;

    [SerializeField] private float writeSpeed;

    private bool _isWriting = false;
    private Coroutine _writeRoutine;

    private Coroutine _clickStateRoutine;
    private bool _wasPointerClickThisFrame;

    private void Awake()
    {
        _showDialogueAction = ShowDialogueInternal;
    }

    private void OnDisable()
    {
        IsUsing = false;
    }

    private void ShowDialogueInternal(DialogueObject dialogueObject)
    {
        gameObject.SetActive(true);
        InputCentral.Disable();
        _wasPointerClickThisFrame = false;
        StopAllCoroutines();
        StartCoroutine(ShowDialogueCoroutine(dialogueObject));
    }

    private IEnumerator ShowDialogueCoroutine(DialogueObject dialogueObject)
    {
        foreach (var sentence in dialogueObject.sentences)
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

        if (dialogueObject.options.Count == 1)
        {
            dialogueObject.options[0].choosenEvent.Invoke();
        }
        else if (dialogueObject.options.Count > 1)
        {
            optionBox.ShowOptions(dialogueObject.options.ToDictionary(op => op.message, op => op.choosenEvent.UnityEventToAction()));
            yield return new WaitWhile(() => optionBox.IsShowing);
        }
        CompleteDialogue();
    }

    private void CompleteDialogue()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        IsUsing = false;
        InputCentral.Enable();
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
            yield return CoroutineHelper.fixedUpdateWait;
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

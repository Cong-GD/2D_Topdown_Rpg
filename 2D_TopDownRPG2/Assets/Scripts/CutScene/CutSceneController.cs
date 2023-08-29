using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private Transform movingObject;

    [SerializeField] private DialogueObject[] dialoguesStorage;
    [SerializeField] private Image fadingImage;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Space]
    [Header("Core thread")]
    [SerializeField] private UnityEvent[] actionQueue;

    private Tweener _fadingTweener;
    private int _currentAction = 0;
    private float _moveSpeed = 0.1f;
    private int _runningAction = 0;
    private Coroutine _movingCoroutine;
    private Coroutine _camareSizeCoroutine;
    private Coroutine _delayActionCoroutine;
    private float _cameraChangeSizeSpeed = 10f;

    private Vector3 Position => movingObject.position;

    private IEnumerator Start()
    {
        while(_currentAction < actionQueue.Length)
        {
            _runningAction = 0;
            actionQueue[_currentAction++].Invoke();
            yield return new WaitWhile(() => _runningAction > 0);
        }
        SkipCutScene();
    }

    public void SkipCutScene()
    {
        StopAllCoroutines();
        StartCoroutine(SkipCutSceneCoroutine());
    }

    public void MoveTo(Transform target)
    {
        _runningAction++;
        if(_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _runningAction--;
        }
        _movingCoroutine = StartCoroutine(MoveToCoroutine(target));
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetAction(int index)
    {
        if (index < 0 || index >= actionQueue.Length)
        {
            Debug.Log($"Invalid action index: {index}");
        }
        _currentAction = Mathf.Clamp(index, 0, actionQueue.Length - 1);
    }
    public void PlayDialogue(int index)
    {
        if(DialoguePanel.IsUsing)
        {
            _runningAction--;
        }
        _runningAction++;
        if (index > dialoguesStorage.Length || index < 0)
        {
            Debug.LogWarning($"Don't have this dialogue with this index {index}");
        }
        StartCoroutine(PlayDialogueCoroutine(Mathf.Clamp(index, 0, dialoguesStorage.Length - 1)));
    }

    public void DelayActionEnd(float delay)
    {
        _runningAction++;
        if(_delayActionCoroutine != null)
        {
            StopCoroutine(_delayActionCoroutine);
            _runningAction--;
        }
        _delayActionCoroutine = StartCoroutine(EndActionDelayCoroutine(delay));
    }

    public void SetFade(bool state)
    {
        SetFade(state, duration : 1f);
    }

    public void SetFade(bool state, float duration)
    {
        _fadingTweener?.Complete();
        var alpha = state ? 1f : 0f;
        _fadingTweener = fadingImage.DOFade(alpha, duration);
    }

    public void SetCameraChangeSizeSpeed(float speed)
    {
        _cameraChangeSizeSpeed = speed;
    }

    public void ChangeCameraSize(float size)
    {
        _runningAction++;
        if(_camareSizeCoroutine != null)
        {
            StopCoroutine(_camareSizeCoroutine);
            _runningAction--;
        }
        _camareSizeCoroutine = StartCoroutine(ChangeCameraSizeCoroutine(size));
    }

    private IEnumerator ChangeCameraSizeCoroutine(float size)
    {
        while(Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - size) > Mathf.Epsilon)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, size, _cameraChangeSizeSpeed * Time.fixedDeltaTime);
            yield return CoroutineHelper.fixedUpdateWait;
        }
        _camareSizeCoroutine = null;
        _runningAction--;
    }

    private IEnumerator MoveToCoroutine(Transform target)
    {
        var distance = Vector2.Distance(Position, target.position);
        while (distance > 0.1f)
        {
            movingObject.position = Vector3.MoveTowards(Position, target.position, _moveSpeed * Time.fixedDeltaTime);
            distance = Vector2.Distance(Position, target.position);
            yield return CoroutineHelper.fixedUpdateWait;
        }
        _movingCoroutine = null;
        _runningAction--;
    }

    private IEnumerator PlayDialogueCoroutine(int index)
    {
        DialoguePanel.ShowDialogue(dialoguesStorage[index]);
        yield return new WaitWhile(() => DialoguePanel.IsUsing);
        _runningAction--;
    }

    private IEnumerator EndActionDelayCoroutine(float delay)
    {
        yield return delay.Wait();
        _delayActionCoroutine = null;
        _runningAction--;
    }

    private IEnumerator SkipCutSceneCoroutine()
    {
        SetFade(true);
        yield return 2f.Wait();
        DOTween.KillAll();
        SceneManager.LoadSceneAsync("Game");
    }
}

using UnityEngine;
using UnityEngine.Events;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PolygonCollider2D boundary;
    [SerializeField] private MonsterStageSpawner[] stageSpawners;

    public UnityEvent OnStageTrigger;
    public UnityEvent OnStageEnded;

    private bool _isTrigged = false;
    private bool _isActive = false;
    private int _workingSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isTrigged && collision.CompareTag("Player"))
        {
            TriggerStageEvent();
        }
    }

    public void TriggerStageEvent()
    {
        _isTrigged = true;
        _isActive = true;
        _workingSpawner = stageSpawners.Length;
        levelManager.SetConfinerCollider(boundary);
        OnStageTrigger.Invoke();

        foreach (var stageSpawner in stageSpawners)
        {
            stageSpawner.StartSpawning(OnSpawnerEnded);
        }
    }

    private void OnSpawnerEnded()
    {
        _workingSpawner--;
        if (_workingSpawner > 0 || !_isActive)
            return;

        _isActive = false;
        levelManager.SetToDefaultLevelBound();
        OnStageEnded.Invoke();

    }
    
}


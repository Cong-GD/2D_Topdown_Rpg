using System.Collections;
using UnityEngine;

public class MonstersAI : SeekerMovingAI
{
    [Header("Monster AI fields")]
    [SerializeField] private float moveRange;
    [SerializeField] private float directChangeInterval;
    [SerializeField] private float detectRange;
    [Min(0.1f)]
    [SerializeField] private float updateInterval;

    private BaseAICombatBehaviour _combatBehaviour;

    public Vector2 StartPosition { get; protected set; }

    [field: SerializeField] public MonstersController Controller { get; private set; }

    public Fighter Player { get; private set; }
    public Vector2 PlayerPosition => Player.HitBox.bounds.center;

    private void Awake()
    {
        SetCombatBehaviour(GetComponent<BaseAICombatBehaviour>());
    }

    private void OnEnable()
    {
        StartCoroutine(AICoroutine());
    }

    public void SetCombatBehaviour(BaseAICombatBehaviour combatBehaviour)
    {
        if (combatBehaviour == null)
        {
            throw new System.ArgumentNullException(nameof(combatBehaviour));
        }
        _combatBehaviour = combatBehaviour;
        _combatBehaviour.Prepare(this);
    }

    public void LookToward(Vector2 destination)
    {
        InputVector = (destination - (Vector2)transform.position).normalized;
    }

    private IEnumerator AICoroutine()
    {
        Player = PlayerController.Instance.Combat;
        StartPosition = transform.position;
        float walkTime = 0;
        while (!Controller.Combat.Health.IsEmpty)
        {
            var distanceToPlayer = Vector2.Distance(transform.position, PlayerPosition);
            if (distanceToPlayer < detectRange)
            {
                yield return _combatBehaviour.StartCombatState();
                continue;
            }
            RandomWalk(ref walkTime);
            yield return updateInterval.Wait();
        }
        StopMove();
        yield return 4f.Wait();
        Controller.ReturnToPool();
    }

    private void RandomWalk(ref float walkTime)
    {
        if (Time.time > walkTime)
        {
            var randomPosition = StartPosition + Random.insideUnitCircle * moveRange;
            walkTime = Time.time + directChangeInterval;
            MoveTo(randomPosition);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        DrawCircle(detectRange, Color.yellow);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartPosition, moveRange);
    }

    private void DrawCircle(float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif

}

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

    private bool _wasTakenHit;

    private void Awake()
    {
        SetCombatBehaviour(GetComponent<BaseAICombatBehaviour>());
        Controller.OnDoneSetup += StartAICoroutine;
        Controller.Combat.OnTakeDamage += TakeHit;
    }

    private void OnDestroy()
    {
        if (Controller != null)
        {
            Controller.OnDoneSetup -= StartAICoroutine;
            if (Controller.Combat != null)
            {
                Controller.Combat.OnTakeDamage -= TakeHit;
            }
        }
    }

    public bool IsAlive() => !Controller.Combat.Health.IsEmpty;

    private void StartAICoroutine()
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
        _wasTakenHit = false;
        float walkTime = 0;
        yield return 0.3f.Wait();
        while (IsAlive())
        {
            var distanceToPlayer = Vector2.Distance(transform.position, PlayerPosition);
            if (distanceToPlayer < detectRange || _wasTakenHit)
            {
                yield return _combatBehaviour.StartCombatState();
                _wasTakenHit = false;
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

    private void TakeHit(DamageBlock _)
    {
        _wasTakenHit = true;
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

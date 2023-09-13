using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CongTDev.TheBoss
{
    public class BossAIController : MonoBehaviour
    {
        private const float WAIT_UPDATE_INTERVAL = 1f;

        [Header("BasicReference")]
        [SerializeField] private Animator animator;
        [SerializeField] private BossAICombatHandler combatHandler;

        [Header("Prepare state field")]
        [SerializeField] private float detectRange;

        [field: SerializeField] public UnityEvent OnStartCombat { get; private set; }
        [field: SerializeField] public UnityEvent OnEndCombat { get; private set; }

        private void Start()
        {
            StartCoroutine(BossFightCoroutine());
        }

        private IEnumerator BossFightCoroutine()
        {
            yield return WaitForPlayerInRange();
            yield return BeforeCombatState();
            yield return combatHandler.StartCombatState();
            yield return EndCombatState();
        }

        private IEnumerator WaitForPlayerInRange()
        {
            var player = PlayerController.Instance.transform;
            while (true)
            {
                var distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer < detectRange)
                {
                    yield break;
                }
                yield return WAIT_UPDATE_INTERVAL.Wait();
            }
        }

        private IEnumerator BeforeCombatState()
        {
            OnStartCombat.Invoke();
            yield break;
        }

        private IEnumerator EndCombatState()
        {
            OnEndCombat.Invoke();
            yield break;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
#endif

    }
}
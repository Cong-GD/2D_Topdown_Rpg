using System;
using System.Collections;
using UnityEngine;

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

        private void Start()
        {
            StartCoroutine(BossFireCoroutine());
        }

        private IEnumerator BossFireCoroutine()
        {
            yield return WaitForPlayerInRange();
            yield return BeforCombatState();
            yield return combatHandler.StartCombatState();
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

        private IEnumerator BeforCombatState()
        {
            animator.Play("Unimmune");
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Unimmune"));
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
using System.Collections;
using UnityEngine;

public class MonsterRandomMove : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private MovementAnimator animator;
    [SerializeField] private float range;
    [SerializeField] private float moveInterval;

    private void Awake()
    {
        movement.OnStartMoving += () => animator.SetMovingState(true);
        movement.OnStopMoving += () => animator.SetMovingState(false);
    }


    private IEnumerator Start()
    {
        yield return Random.Range(0.1f, 3f).Wait();
        while (true)
        {
            var destination = (Vector3)Random.insideUnitCircle * range + transform.position;
            movement.MoveDirect = (destination - transform.position).normalized;
            yield return new WaitWhile(() => Vector3.Distance(transform.position, destination) > 0.3f);
            movement.MoveDirect = Vector2.zero;
            yield return moveInterval.Wait();
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
#endif
}

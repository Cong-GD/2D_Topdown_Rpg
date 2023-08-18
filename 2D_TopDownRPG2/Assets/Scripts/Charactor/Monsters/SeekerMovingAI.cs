using Pathfinding;
using System.Collections;
using UnityEngine;

public abstract class SeekerMovingAI : BaseMovementInput
{
    [Header("Seeker moving fields")]
    [SerializeField] private Seeker seeker;
    [SerializeField] private float nextWaypointDistance;

    private Path _path;
    private Coroutine _movingRoutine;

    public void MoveTo(Vector2 destination)
    {
        seeker.CancelCurrentPathRequest();
        seeker.StartPath(transform.position, destination, StatMovingRoutine);

        void StatMovingRoutine(Path p)
        {
            if (p.error)
                return;
 
            _path = p;
            if (_movingRoutine != null)
            {
                StopCoroutine(_movingRoutine);
            }
            _movingRoutine = StartCoroutine(MovingCoroutine());
        }
    }

    public void StopMove()
    {
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
        }
        InputVector = Vector2.zero;
    }

    private IEnumerator MovingCoroutine()
    {
        int currentPoint = 0;
        while (currentPoint < _path.vectorPath.Count)
        {
            Vector2 direction = (_path.vectorPath[currentPoint] - transform.position).normalized;

            InputVector = direction;
            yield return CoroutineHelper.fixedUpdateWait;
            var distance = Vector2.Distance(transform.position, _path.vectorPath[currentPoint]);
            if(distance < nextWaypointDistance)
            {
                currentPoint++;
            }
        }
        InputVector = Vector2.zero;
    }
}
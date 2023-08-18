using UnityEngine;

public class MovementAnimator : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }

    public static readonly int RunningHash = Animator.StringToHash("Running");

    public void ClearState()
    {
        Animator.Play("Idle");
    }

    public void SetMovingState(bool isMoving)
    {
        Animator.SetBool(RunningHash, isMoving);
    }
}
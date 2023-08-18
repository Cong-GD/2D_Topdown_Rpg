using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlaySpikeFire()
    {
        animator.Play("SpikeFire");
    }
}

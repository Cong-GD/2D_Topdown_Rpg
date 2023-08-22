using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Collider2D trapCollider;
    [SerializeField] private Sprite spikeDownSprite;
    [SerializeField] private Sprite spikeUpSprite;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private float damageDeal;
    [SerializeField] private float upTime;
    [SerializeField] private float downTime;
    [SerializeField] private float startDelay;

    private void OnEnable()
    {
        SpikeDown();
        StartCoroutine(SpikeUpCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Fighter>(out var fighter) && fighter.team == Fighter.Team.Hero)
        {
            var damageBlock = GenericPool<DamageBlock>.Get();
            damageBlock.Init(damageDeal);
            fighter.TakeDamage(damageBlock);
            GenericPool<DamageBlock>.Release(damageBlock);
        }
    }

    private IEnumerator SpikeUpCoroutine()
    {
        yield return startDelay.Wait();
        while(true)
        {
            SpikeUp();
            yield return upTime.Wait();
            SpikeDown();
            yield return downTime.Wait();
        }
    }

    private void SpikeDown()
    {
        render.sprite = spikeDownSprite;
        trapCollider.enabled = false;
    }

    private void SpikeUp()
    {
        render.sprite = spikeUpSprite;
        trapCollider.enabled = true;
    }
}

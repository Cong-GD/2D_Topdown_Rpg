using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LightRuneIndicator : BaseIndicator
{
    [SerializeField] private SpriteRenderer[] runes;

    private Color currentColor = new(0, 0.8f, 0.9f, 0f);

    private Coroutine fadeCoroutine;

    public override void Active()
    {
        DoFade(1f);
    }

    public override void Deactive()
    {
        DoFade(0f);
    }

    private void DoFade(float value)
    {
        if (!isActiveAndEnabled)
            return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(DoFadeCoroutine(value));
    }

    private IEnumerator DoFadeCoroutine(float value)
    {
        while(Mathf.Abs(currentColor.a - value) >= Mathf.Epsilon)
        {
            currentColor.a = Mathf.MoveTowards(currentColor.a, value, Time.fixedDeltaTime);
            foreach (var rune in runes)
            {
                rune.color = currentColor;
            }
            yield return CoroutineHelper.fixedUpdateWait;
        }
    }
}

using CongTDev.AbilitySystem;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilitySlot : ItemSlot<IActiveAbility>
{
    [SerializeField] private Image cooldownMask;

    [SerializeField] private Image backGround;

    private Tween _tween;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null or IActiveAbility;
    }

    protected override void OnItemGetIn(IActiveAbility ability)
    {
        base.OnItemGetIn(ability);
        ability.Install(PlayerController.Instance.AbilityCaster);
        if (backGround != null)
        {
            backGround.gameObject.SetActive(false);
        }
        if(!Item.IsReady())
        {
            RunCooldownAnim();
        }
    }

    protected override void OnItemGetOut(IActiveAbility item)
    {
        base.OnItemGetOut(item);
        if (backGround != null)
        {
            backGround.gameObject.SetActive(true);
        }
        _tween?.Kill();
        cooldownMask.fillAmount = 0;
    }

    public Respond TryUseSlotAbility()
    {
        if (IsSlotEmpty)
        {
            return Respond.CanNotUse;
        }
        var respond = Item.TryUse();
        if (respond == Respond.Success || respond == Respond.InCasting)
        {
            RunCooldownAnim();
        }
        return respond;
    }

    public void RunCooldownAnim()
    {
        _tween?.Kill();
        if (IsSlotEmpty || Item.IsReady())
        {
            cooldownMask.fillAmount = 0;
            return;
        }
        cooldownMask.fillAmount = 1;
        _tween = cooldownMask.DOFillAmount(0, Item.CurrentCoolDown).SetEase(Ease.Linear);
    }
}
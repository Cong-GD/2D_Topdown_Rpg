using DG.Tweening;
using CongTDev.AbilitySystem;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilitySlot : ItemSlot<IActiveAbility>
{
    [SerializeField] private Image cooldownMask;

    [SerializeField] private Image backGround;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null or IActiveAbility;
    }

    protected override void OnItemGetIn(IActiveAbility ability)
    {
        base.OnItemGetIn(ability);
        ability.Install(PlayerController.Instance.AbilityCaster);
        if(backGround != null)
        {
            backGround.gameObject.SetActive(false);
        }
    }

    protected override void OnItemGetOut(IActiveAbility item)
    {
        base.OnItemGetOut(item);
        if (backGround != null)
        {
            backGround.gameObject.SetActive(true);
        }
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
        if (IsSlotEmpty)
            return;

        cooldownMask.fillAmount = 1;
        cooldownMask.DOFillAmount(0, Item.GetCooldownTime()).SetEase(Ease.Linear);
    }

}



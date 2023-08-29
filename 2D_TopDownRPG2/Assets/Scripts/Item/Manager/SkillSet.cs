using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillSet : MonoBehaviour, ISerializable
{
    [SerializeField] private ActiveAbilitySlot[] activeAbilitySlots;
    [SerializeField] private ConsumableSlot consumableSlotX;
    [SerializeField] private ConsumableSlot consumableSlotC;
    [SerializeField] private ActiveAbilitySlot basicAbilitySlot;
    [SerializeField] private ActiveRune basicAbilityRune;

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Start()
    {
        basicAbilitySlot.PushItem(basicAbilityRune.CreateItem());
    }

    

    private void SubscribeEvents()
    {
        EventManager<IItemSlot>.AddListener("TryEquipActiveAbility", AcquipAbility);
        var abilityInput = InputCentral.InputActions.PlayerAbilityTrigger;
        abilityInput.BasicAttack.performed += UseAbility0;
        abilityInput.Ability1.performed += UseAbility1;
        abilityInput.Ability2.performed += UseAbility2;
        abilityInput.Ability3.performed += UseAbility3;
        abilityInput.Ability4.performed += UseAbility4;
        abilityInput.Ability5.performed += UseAbility5;
        abilityInput.Consum1.performed += UseConsumX;
        abilityInput.Consum2.performed += UseConsumC;
    }


    private void UnsubscribeEvents()
    {
        EventManager<IItemSlot>.RemoveListener("TryEquipActiveAbility", AcquipAbility);
        var abilityInput = InputCentral.InputActions.PlayerAbilityTrigger;
        abilityInput.BasicAttack.performed -= UseAbility0;
        abilityInput.Ability1.performed -= UseAbility1;
        abilityInput.Ability2.performed -= UseAbility2;
        abilityInput.Ability3.performed -= UseAbility3;
        abilityInput.Ability4.performed -= UseAbility4;
        abilityInput.Ability5.performed -= UseAbility5;
        abilityInput.Consum1.performed -= UseConsumX;
        abilityInput.Consum2.performed -= UseConsumC;
    }
    private void UseAbility0(InputAction.CallbackContext context)
    {
        basicAbilitySlot.TryUseSlotAbility();
    }

    private void UseAbility1(InputAction.CallbackContext context)
    {

        activeAbilitySlots[0].TryUseSlotAbility();
    }
    private void UseAbility2(InputAction.CallbackContext context)
    {
        activeAbilitySlots[1].TryUseSlotAbility();
    }
    private void UseAbility3(InputAction.CallbackContext context)
    {
        activeAbilitySlots[2].TryUseSlotAbility();
    }
    private void UseAbility4(InputAction.CallbackContext context)
    {
        activeAbilitySlots[3].TryUseSlotAbility();
    }
    private void UseAbility5(InputAction.CallbackContext context)
    {
        activeAbilitySlots[4].TryUseSlotAbility();
    }

    private void UseConsumX(InputAction.CallbackContext context)
    {
        consumableSlotX.UseItem();
    }

    private void UseConsumC(InputAction.CallbackContext context)
    {
       consumableSlotC.UseItem();
    }

    private void AcquipAbility(IItemSlot inventorySlot)
    {
        if (inventorySlot.BaseItem is not IActiveAbility)
            return;

        foreach (var abilitySlot in activeAbilitySlots)
        {
            if (abilitySlot.IsSlotEmpty)
            {
                IItemSlot.Swap(abilitySlot, inventorySlot);
            }
        }
    }

    public void SetToDefaultValue()
    {
        foreach (var abilitySlot in activeAbilitySlots)
        {
            abilitySlot.PushItem(null);
        }
    }

    #region IOSystem

    public SerializedObject Serialize()
    {
        return new SerializedSkillSet(this);
    }

    public class SerializedSkillSet : SerializedObject, ISerializable
    {
        public string[] activeAbilityJsons;

        public string consumSlotXJson;

        public string consumSlotCJson;

        public SerializedSkillSet() { }

        public SerializedSkillSet(SkillSet skillSet)
        {
            int activeAbilityCount = skillSet.activeAbilitySlots.Length;
            activeAbilityJsons = new string[activeAbilityCount];

            for (int i = 0; i < activeAbilityCount; i++)
            {
                activeAbilityJsons[i] = skillSet.activeAbilitySlots[i].Item.ToWrappedJson();
            }

            consumSlotXJson = skillSet.consumableSlotX.Item.ToWrappedJson();
            consumSlotCJson = skillSet.consumableSlotC.Item.ToWrappedJson();
        }

        public void Load(SkillSet skillSet)
        {
            int activeCount = Mathf.Min(skillSet.activeAbilitySlots.Length, activeAbilityJsons.Length);

            for (int i = 0; i < activeCount; i++)
            {
                var activeAbility = (IItem)JsonHelper.WrappedJsonToObject(activeAbilityJsons[i]);
                skillSet.activeAbilitySlots[i].PushItem(activeAbility);
            }

            skillSet.consumableSlotX.PushItem((IItem)JsonHelper.WrappedJsonToObject(consumSlotXJson));
            skillSet.consumableSlotC.PushItem((IItem)JsonHelper.WrappedJsonToObject(consumSlotCJson));
        }

        public override SerializedType GetSerializedType() => SerializedType.SkillSet;
        public SerializedObject Serialize()
        {
            return this;
        }

        public override object Deserialize()
        {
            return this;
        }
    }
    #endregion

}

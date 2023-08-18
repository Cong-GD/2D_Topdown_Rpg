using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet : MonoBehaviour, ISerializable
{
    [SerializeField] private ActiveAbilitySlot[] activeAbilitySlots;

    [SerializeField] private ConsumableSlot consumableSlotX;

    [SerializeField] private ConsumableSlot consumableSlotC;

    [SerializeField] private List<ActiveRuneSO> defaultAbility;

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        AcquipDefaultAbility();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            activeAbilitySlots[0].TryUseSlotAbility();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            activeAbilitySlots[1].TryUseSlotAbility();
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            activeAbilitySlots[2].TryUseSlotAbility();
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            activeAbilitySlots[3].TryUseSlotAbility();
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            activeAbilitySlots[4].TryUseSlotAbility();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        EventManager<IItemSlot>.AddListener("TryEquipActiveAbility", AcquipAbility);
    }

    private void UnsubscribeEvents()
    {
        EventManager<IItemSlot>.RemoveListener("TryEquipActiveAbility", AcquipAbility);
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

    private void AcquipDefaultAbility()
    {
        foreach (var rune in defaultAbility)
        {
            foreach (var abilitySlot in activeAbilitySlots)
            {
                if (abilitySlot.IsSlotEmpty)
                {
                    abilitySlot.TryPushItem(rune.GetAbility(), out _);
                    break;
                }
            }
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

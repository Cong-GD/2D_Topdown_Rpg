using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SkillBarController : MonoBehaviour
{
    private const string fileName = "SkillSets";

    [SerializeField] private SkillSet skillSet1;

    [SerializeField] private SkillSet skillSet2;

    [SerializeField] private AbilityCaster abilityCaster;

    [SerializeField] private ActiveRuneSO basicAttackRune;

    private IActiveAbility _basicAttackAbility;
    private Camera _camera;

    private void Awake()
    {
        _basicAttackAbility = (IActiveAbility)basicAttackRune.GetAbility();
        _basicAttackAbility.Install(abilityCaster);
        _camera = GameManager.Instance.Cam;
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager.AddListener("OnGameSave", SaveController);
        EventManager.AddListener("OnGameLoad", LoadController);
        GameManager.Instance.InputActions.PlayerAbilityTrigger.BasicAttack.started += TriggerBasicAttack;
    }

    private void UnsubscribeEvents()
    {
        EventManager.RemoveListener("OnGameSave", SaveController);
        EventManager.RemoveListener("OnGameLoad", LoadController);
        if(GameManager.Instance != null)
        {
            GameManager.Instance.InputActions.PlayerAbilityTrigger.BasicAttack.started -= TriggerBasicAttack;
        }
    }

    private void TriggerBasicAttack(InputAction.CallbackContext context)
    {
        abilityCaster.LookDirection = _camera.ScreenToWorldPoint(Mouse.current.position.value);
        _basicAttackAbility.TryUse();
    }

    private void SaveController()
    {
        Directory.CreateDirectory(fileName);
        SaveLoadHandler.SaveToFile(fileName + "SkillSet1", skillSet1);
        SaveLoadHandler.SaveToFile(fileName + "SkillSet2", skillSet2);
    }

    private void LoadController()
    {
        var serializedSkilSet1 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(fileName + "SkillSet1");
        var serializedSkilSet2 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(fileName + "SkillSet2");
        serializedSkilSet1.Load(skillSet1);
        serializedSkilSet2.Load(skillSet2);
    }
}

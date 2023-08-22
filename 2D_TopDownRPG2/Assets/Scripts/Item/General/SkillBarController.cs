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

    private void Awake()
    {
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
    }

    private void UnsubscribeEvents()
    {
        EventManager.RemoveListener("OnGameSave", SaveController);
        EventManager.RemoveListener("OnGameLoad", LoadController);
    }

    private void SaveController()
    {
        Directory.CreateDirectory(fileName);
        SaveLoadHandler.SaveToFile(fileName + "SkillSet1", skillSet1);
        //SaveLoadHandler.SaveToFile(fileName + "SkillSet2", skillSet2);
    }

    private void LoadController()
    {
        var serializedSkilSet1 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(fileName + "SkillSet1");
        //var serializedSkilSet2 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(fileName + "SkillSet2");
        serializedSkilSet1.Load(skillSet1);
        //serializedSkilSet2.Load(skillSet2);
    }
}

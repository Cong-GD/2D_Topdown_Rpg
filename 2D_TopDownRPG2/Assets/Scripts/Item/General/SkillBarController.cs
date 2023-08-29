using CongTDev.EventManagers;
using CongTDev.IOSystem;
using UnityEngine;

public class SkillBarController : MonoBehaviour
{
    private const string FILE_NAME = "SkillSet";

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
        SaveLoadHandler.SaveToFile(FILE_NAME, skillSet1);
        ////SaveLoadHandler.SaveToFile(fileName + "SkillSet2", skillSet2);
    }

    private void LoadController()
    {
        try
        {
            var serializedSkilSet1 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(FILE_NAME);
            //var serializedSkilSet2 = (SkillSet.SerializedSkillSet)SaveLoadHandler.LoadFromFile(fileName + "SkillSet2");
            serializedSkilSet1.Load(skillSet1);
            //serializedSkilSet2.Load(skillSet2);
        }
        catch
        {
            skillSet1.SetToDefaultValue();
        }

    }
}

using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    private const string FILE_NAME = "PlayerLevel";

    private const float XP_GROW = 1.5f;
    public static int CurrentLevel { get; private set; } = 1;

    public static int CurrentXp { get; private set; } = 0;

    public static int CapacityXp { get; private set; } = 100;

    public static event Action OnValueChange;

    [SerializeField] private Fighter player;
    [SerializeField] private BaseStatData statData;

    private Dictionary<Stat, StatModifier> _statModifiers = new();

    private void Awake()
    {
        LoadFromFile();
        EventManager.AddListener("OnGameSave", SaveToFile);
        EventManager.AddListener("OnGameLoad", LoadFromFile);
        EventManager<int>.AddListener("OnReceiveXp", AddXp);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("OnGameSave", SaveToFile);
        EventManager.RemoveListener("OnGameLoad", LoadFromFile);
        EventManager<int>.RemoveListener("OnReceiveXp", AddXp);
    }

    public void SaveToFile()
    {
        SaveLoadHandler.SaveToFile(FILE_NAME, new LevelData()
        {
            currentLevel = CurrentLevel,
            currentXp = CurrentLevel,
            capacityXp = CapacityXp
        });
    }

    public void LoadFromFile()
    {
        var levelDate = SaveLoadHandler.LoadFromFile<LevelData>(FILE_NAME);
        if (levelDate == null)
        {
            CurrentLevel = 1;
            CurrentXp = 0;
            CapacityXp = 100;
        }
        else
        {
            CurrentLevel = levelDate.currentLevel;
            CurrentXp = levelDate.currentXp;
            CapacityXp = levelDate.capacityXp;
        }
        UpdateLevelStat();
    }

    public void AddXp(int amount)
    {
        CurrentXp += amount;
        while (CurrentXp >= CapacityXp)
        {
            CurrentXp -= CapacityXp;
            CapacityXp = (int)(CapacityXp * XP_GROW);
            CurrentLevel++;
            UpdateLevelStat();
        }
        OnValueChange?.Invoke();
    }

    private void UpdateLevelStat()
    {
        foreach (var modifier in _statModifiers)
        {
            player.Stats.RemoveModifier(modifier.Key, modifier.Value);
        }

        _statModifiers = statData.growStat.GetGrowingStat(CurrentLevel);

        foreach (var modifier in _statModifiers)
        {
            player.Stats.ApplyModifier(modifier.Key, modifier.Value);
        }

        player.Health.Fill();
        player.Mana.Fill();

    }

    [Serializable]
    public class LevelData
    {
        public int currentLevel;
        public int currentXp;
        public int capacityXp;
    }
}
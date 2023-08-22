using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    private static string StatToString(Stat stat) => stat switch
    {
        Stat.MaxHealth => "Max health",
        Stat.MaxMana => "Max mana",
        Stat.MoveSpeed => "Move speed",
        Stat.AttackPower => "Attack point",
        Stat.Defence => "Defence",
        Stat.BlockChance => "Block chance",
        Stat.Accuracy => "Accuracy",
        Stat.Evasion => "Evasion",
        Stat.CriticalChance => "Critical chance",
        Stat.CritticalHitDamage => "Critical hit damage",
        _ => "Unknow stat"
    };


    [SerializeField] private Stat statDisplayed;

    [SerializeField] private bool isRound;

    [SerializeField] private TextMeshProUGUI statNameUI;

    [SerializeField] private TextMeshProUGUI statValueUI;

    private CharactorStat playerStats;

    private void Reset()
    {
        statNameUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        statValueUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        playerStats = PlayerController.Instance.Combat.Stats;
    }

    private void OnEnable()
    {
        statNameUI.text = StatToString(statDisplayed) + " : ";
        UpdateUI(playerStats[statDisplayed].FinalValue);
        playerStats[statDisplayed].OnValueChange += UpdateUI;
    }

    private void OnDisable()
    {
        playerStats[statDisplayed].OnValueChange -= UpdateUI;
    }


    private void UpdateUI(float value)
    {
        if (isRound)
        {
            statValueUI.text = $"{Mathf.Round(value)}";
        }
        else
        {
            statValueUI.text = $"{value:F2}";
        }
    }

}

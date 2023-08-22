using CongTDev.IOSystem;
using TMPro;
using UnityEngine;

public class UserAvatar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;

    private void OnEnable()
    {
        UpdateUI();
        PlayerLevelSystem.OnValueChange += UpdateUI;
    }

    private void OnDisable()
    {
        UpdateUI();
        PlayerLevelSystem.OnValueChange -= UpdateUI;
    }

    public void UpdateUI()
    {
        usernameText.text = FileNameData.CurrentUser;
        levelText.text = $"Lv : {PlayerLevelSystem.CurrentLevel}";
        xpText.text = $"Xp : {PlayerLevelSystem.CurrentXp} / {PlayerLevelSystem.CapacityXp}";
    }
}

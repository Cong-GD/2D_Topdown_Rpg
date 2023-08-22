using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Fighter player;

    [SerializeField] private Image healthImage;
    [SerializeField] private Image manaImage;
    [SerializeField] private Image xpImage;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        player.Health.OnValueChange += UpdateHealthValue;
        player.Mana.OnValueChange += UpdateManaValue;
        UpdateHealthValue(player.Health.Current, player.Health.Capacity);
        UpdateManaValue(player.Mana.Current, player.Mana.Capacity);

        PlayerLevelSystem.OnValueChange += UpdateLevelValue;
        UpdateLevelValue();

    }

    private void OnDisable()
    {
        if(player != null)
        {
            player.Health.OnValueChange -= UpdateHealthValue;
            player.Mana.OnValueChange -= UpdateManaValue;
        }
        PlayerLevelSystem.OnValueChange -= UpdateLevelValue;
    }

    private void UpdateLevelValue()
    {
        levelText.text = PlayerLevelSystem.CurrentLevel.ToString();
        xpImage.fillAmount = (float)PlayerLevelSystem.CurrentXp / PlayerLevelSystem.CapacityXp;
    }

    private void UpdateHealthValue(float current, float max)
    {
        ChangeUI(current, max, healthImage);
    }
    private void UpdateManaValue(float current, float max)
    {
        ChangeUI(current, max, manaImage);
    }

    private void ChangeUI(float current, float max, Image fillImage)
    {
        if (max == 0) return;
        fillImage.fillAmount = current / max;
    }
}

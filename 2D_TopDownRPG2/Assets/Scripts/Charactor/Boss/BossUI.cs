using CongTDev.AbilitySystem;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private AbilityCaster abilityCaster;

    private void Awake()
    {
        healthFill.fillAmount = abilityCaster.Owner.Health.Ratio;
        abilityCaster.Owner.Health.OnValueChange += UpdateUI;
    }

    private void OnDestroy()
    {
        if (abilityCaster != null)
        {
            abilityCaster.Owner.Health.OnValueChange -= UpdateUI;
        }
    }

    private void UpdateUI(float current, float capacity)
    {
        healthFill.fillAmount = current / capacity;
    }
}

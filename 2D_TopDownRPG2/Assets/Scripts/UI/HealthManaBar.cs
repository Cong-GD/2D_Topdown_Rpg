using UnityEngine;
using UnityEngine.UI;

public class HealthManaBar : MonoBehaviour
{
    public enum DisplayType
    {
        Health,
        Mana
    }

    [SerializeField] private DisplayType displayedValue;

    [SerializeField] private Fighter fighter;

    [SerializeField] private Image fillImage;

    private ResourceBlock _displayingResource;

    private void OnEnable()
    {
        if (displayedValue == DisplayType.Mana)
        {
            _displayingResource = fighter.Mana;
        }
        else
        {
            _displayingResource = fighter.Health;
        }

        _displayingResource.OnValueChange += ChangeUI;
        ChangeUI(_displayingResource.Current, _displayingResource.Capacity);
    }

    private void OnDisable()
    {
        _displayingResource.OnValueChange -= ChangeUI;
    }

    private void ChangeUI(float current, float max)
    {
        if (max == 0) return;
        fillImage.fillAmount = current / max;
    }
}

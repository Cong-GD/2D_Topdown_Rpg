using CongTDev.AbilitySystem;
using UnityEngine;

public static class ColorHelper
{
    private readonly static ColorData data;
    static ColorHelper()
    {
        data = Resources.Load<ColorData>("Data/ColorData");
    }
    public static string ToRGBHex(this Color color)
        => string.Format("{0:X2}{1:X2}{2:X2}", ToByte(color.r), ToByte(color.g), ToByte(color.b));

    private static byte ToByte(float value) => (byte)(value * 255);

    /// <summary>
    /// Wrap a string to rich text string with color
    /// </summary>
    public static string RichText(this string s, Color color)
    {
        return $"<color=#{ToRGBHex(color)}>{s}</color>";
    }

    public static Color GetRarityColor(ItemRarity rarity)
        => rarity switch
        {
            ItemRarity.Common => data.common,
            ItemRarity.Uncommon => data.uncommon,
            ItemRarity.Rare => data.rare,
            ItemRarity.Epic => data.epic,
            ItemRarity.Lengendary => data.lengendary,
            _ => Color.white,
        };


    public static Color GetItemTypeColor(this IItem item)
        => item.ItemType switch
        {
            Rune.ITEM_TYPE => data.rune,
            Equipment.ITEM_TYPE => data.equipment,
            IAbility.ITEM_TYPE => data.ability,
            ConsumableItem.ITEM_TYPE => data.comsumable,
            _ => Color.white
        };

    public static Color GetDamageFeedBackTextColor(this DamageType damageType)
    => damageType switch
    {
        DamageType.PhysicalDamage => data.physicsDamageFeedBackText,
        DamageType.MagicDamage => data.magicDamageFeedBackText,
        DamageType.EnviromentDamage => data.enviromentDamageText,
        _ => Color.white
    };
}

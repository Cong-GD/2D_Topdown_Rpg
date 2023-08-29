using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Data/Color data")]
public class ColorData : ScriptableObject
{
    [Header("Item rarity color")]
    public Color common;
    public Color uncommon;
    public Color rare;
    public Color epic;
    public Color lengendary;
    [Space]
    [Header("Item type color")]
    public Color rune;
    public Color ability;
    public Color equipment;
    public Color comsumable;
    [Space]
    [Header("Damage feedback text color")]
    public Color physicsDamageFeedBackText;
    public Color magicDamageFeedBackText;
    public Color enviromentDamageText;

}
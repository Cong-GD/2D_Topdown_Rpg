using Unity.VisualScripting;

public static class CombatHelper
{
    public static float CalculateCooldownTime(float baseCooldownTime, float coolDownReduce)
    {
        return baseCooldownTime - (coolDownReduce /100) * baseCooldownTime;
    }
}
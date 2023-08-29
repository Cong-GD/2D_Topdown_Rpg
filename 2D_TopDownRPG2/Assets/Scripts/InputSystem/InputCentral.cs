public static class InputCentral
{
    private static InputActions inputActions;

    public static InputActions InputActions => inputActions ??= new();

    public static void Enable()
    {
        InputActions.Enable();
    }

    public static void Disable()
    {
        InputActions.Disable();
    }

    public static void DisablePlayerMovement()
    {
        InputActions.PlayerMove.Disable();
    }

    public static void DisablePlayerAbility()
    {
        InputActions.PlayerAbilityTrigger.Disable();
    }
}
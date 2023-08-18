using CongTDev.AbilitySystem;
using CongTDev.Communicate;
using CongTDev.IOSystem;
using UnityEngine;

public static class CheatCode
{
    public const char CHEAT_KEY = '/';

    public static bool IsCheatAllow = true;

    public static void TryApplyCheat(string cheatCode)
    {
        if (string.IsNullOrEmpty(cheatCode) || cheatCode[0] != CHEAT_KEY)
            return;

        var code = cheatCode.Split(' ');
        if (code.Length != 2)
            return;

        switch (code[0])
        {
            case "/GetRune":
                GetRuneCheat(code[1]);
                break;
            case "/GetEquipment":
                GetEquipmentCheat(code[1]);
                break;
        }
    }

    private static Mail GetMailPattern() => new Mail("From cheat code", "This is your item");

    private static void SendItemToMailBox(IItem item)
    {
        if (item == null)
            return;

        var mail = GetMailPattern();
        mail.AddAttachItem(item);
        MailBox.Instance.ReceiveMail(mail);
    }

    public static void GetRuneCheat(string runeName)
    {
        var path = FileNameData.GetRuneResourcePath(runeName);
        var rune = Resources.Load<RuneSO>(path);

        SendItemToMailBox(rune);
    }

    public static void GetEquipmentCheat(string equipmentName)
    {
        var path = FileNameData.GetEquimentResourcePath(equipmentName);
        var equipment = Resources.Load<EquipmentFactorySO>(path);
        if (equipment != null)
        {
            SendItemToMailBox(equipment.CreateEquipment());
        }
    }
}

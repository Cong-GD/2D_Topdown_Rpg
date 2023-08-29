using CongTDev.AbilitySystem;
using CongTDev.AudioManagement;
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
            case "/Goto":
                ConfirmPanel.Ask("You are going to teleport", () => GameManager.Instance.ChangeMap(code[1]));
                break;
            case "/GetGold":
                if(int.TryParse(code[1], out var gold))
                {
                    ReceiveGold(gold);
                }
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
        var rune = Resources.Load<Rune>(path);

        SendItemToMailBox(rune);
    }

    public static void GetEquipmentCheat(string equipmentName)
    {
        var path = FileNameData.GetEquimentResourcePath(equipmentName);
        var equipment = Resources.Load<EquipmentFactory>(path);
        if (equipment != null)
        {
            SendItemToMailBox(equipment.CreateItem());
        }
    }

    public static void ReceiveGold(int gold)
    {
        ConfirmPanel.Ask($"You have got {gold} gold!", GetGold, GetGold);

        void GetGold()
        {
            GameManager.PlayerGold += gold;
            AudioManager.Play("BuySell");
        }
        
    }
}

using CongTDev.EventManagers;
using CongTDev.IOSystem;
using UnityEditor;
using UnityEngine;

public class GameManager : GlobalReference<GameManager>
{
    private InputActions _inputActions;
    public InputActions InputActions => _inputActions ??= new InputActions();

    public static bool IsPausing { get; private set; } = false;

    private void Start()
    {
        if (FileNameData.IsThisUserHasWritten(FileNameData.CurrentUser))
        {
            Debug.Log("Load game with " + FileNameData.CurrentUser);
        }
        else
        {
            Debug.Log("New game with " + FileNameData.CurrentUser);
        }

        EventManager<string>.RaiseEvent("SendSystemMessage", "Wellcome to summorner's rift!");
        InputActions.Enable();

        //MailBox.Instance.ReceiveMail(new Mail("Wellcome", "Greeting deer hero"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = IsPausing ? 1 : 0;
            IsPausing = !IsPausing;
        }
    }

    public void QuitGame()
    {
        ConfirmPanel.Instance.AskForComfirm(QuitGameWithoutConfirm, null, "Are you sure you want to quit game?");
    }

    private void QuitGameWithoutConfirm()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}

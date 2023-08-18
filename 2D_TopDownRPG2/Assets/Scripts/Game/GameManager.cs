using CongTDev.Communicate;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : GlobalReference<GameManager>
{
    private Camera _cam;

    private InputActions _inputActions;
    public InputActions InputActions => _inputActions ??= new InputActions();
    public Camera Cam
    {
        get
        {
            if (_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }

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

        if (Input.GetKey(KeyCode.G))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CheatCode.GetEquipmentCheat("Sword");
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheatCode.GetRuneCheat("FireBall");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheatCode.GetEquipmentCheat("Sword");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                CheatCode.GetEquipmentCheat("Sword");
            }
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = _isPausing ? 1 : 0;
            _isPausing = !_isPausing;
        }
    }

    bool _isPausing = false;

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

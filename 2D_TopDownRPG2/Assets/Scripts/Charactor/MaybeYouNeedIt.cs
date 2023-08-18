using UnityEngine;

public class MaybeYouNeedIt : MonoBehaviour
{
    private string[] graceful = new string[]
    {
        "Have a good day",
        "Good luck",
        "Just relax",
        "Good job"
    };

    private void Start()
    {
        Debug.Log($"<color=#CBC916>{graceful[Random.Range(0, graceful.Length)]}</color>");
    }

    public void PrintMessage( string message )
    {
        Debug.Log(message);
    }

    public void Check(bool toggle)
    {
        Debug.Log(toggle);
        
    }
}
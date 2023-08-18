using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    [field: TextArea]
    [field: SerializeField]  public string[] Sentences { get; private set; }


}

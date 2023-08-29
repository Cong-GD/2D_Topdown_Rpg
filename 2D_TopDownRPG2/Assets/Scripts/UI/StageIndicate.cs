using TMPro;
using UnityEngine;

public class StageIndicate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private Animator anim;

    [SerializeField] private string startText;
    [SerializeField] private string endText;

    public void IndicateStageStart()
    {
        gameObject.SetActive(true);
        stageText.text = startText;
        anim.Play("StageStart");
    }

    public void IndicateStageEnd()
    {
        gameObject.SetActive(true);
        stageText.text = endText;
        anim.Play("StageEnd");
    }
}

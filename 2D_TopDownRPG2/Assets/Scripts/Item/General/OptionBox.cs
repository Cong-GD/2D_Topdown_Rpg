using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionBox : MonoBehaviour
{
    [SerializeField] private SelectOption prefab;
    private List<SelectOption> _selectOptions = new();
    private Action _onComplete;

    public bool IsShowing { get; private set; }

    public OptionBox ShowOptions(IEnumerable<KeyValuePair<string, Action>> actionMap)
    {
        gameObject.SetActive(true);
        IsShowing = true;
        _onComplete = () =>
        {
            gameObject.SetActive(false);
            IsShowing = false;
        };
        ChangeOptionAmount(actionMap.Count());
        int i = 0;
        foreach (KeyValuePair<string, Action> pair in actionMap)
        {
            _selectOptions[i].ShowOption(pair.Key, pair.Value);
            _selectOptions[i].ClickAction += _onComplete;
            i++;
        }
        return this;
    }

    public OptionBox OnComplete(Action onComplete)
    {
        _onComplete += onComplete;
        return this;
    }

    public OptionBox Disable()
    {
        _onComplete?.Invoke();
        return this;
    }

    private void ChangeOptionAmount(int amount)
    {
        if (amount < 0)
            return;

        while (_selectOptions.Count < amount)
        {
            var selectOption = Instantiate(prefab);
            selectOption.transform.SetParent(transform);
            _selectOptions.Add(selectOption);
        }
        while (_selectOptions.Count > amount)
        {
            var lastIndex = _selectOptions.Count - 1;
            Destroy(_selectOptions[lastIndex].gameObject);
            _selectOptions.RemoveAt(lastIndex);
        }
    }
}

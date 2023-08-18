using System;

public class LevelSystem
{
    private const int ExpIncreasingEachLevel = 50;

    private int _currentLevel;
    private int _currentExp;

    public event Action OnLevelUp;
    public int CurrentLevel => _currentLevel;
    public int CurrentExp => _currentExp;
    public int CapacityExp => ExpIncreasingEachLevel * _currentLevel;

    public void AddExp(int exp)
    {
        _currentLevel += exp;
        while (_currentExp >= CapacityExp)
        {
            _currentExp -= CapacityExp;
            _currentLevel++;
            OnLevelUp?.Invoke();
        }
    }

    public LevelSystem()
    {
        _currentLevel = 1;
        _currentExp = 0;
    }

    public LevelSystem(int currentExp, int level)
    {
        _currentLevel = level > 1 ? level : 1;
        _currentExp = currentExp;
        if (_currentExp > CapacityExp)
        {
            _currentExp = CapacityExp - 1;
        }
    }

}
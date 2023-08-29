using UnityEngine;

public abstract class BaseItemFactory : ScriptableObject
{
    public abstract IItem CreateItem();
}
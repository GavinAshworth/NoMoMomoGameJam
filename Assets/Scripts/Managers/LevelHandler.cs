using UnityEngine;

[DefaultExecutionOrder(-1)] //We put this here so Unity runs this script first.

public class LevelHandler : SingletonMonoBehavior<LevelHandler>
{
    public int level { get; private set; } = 1;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void IncrementLevel()
    {
        level++;
        Debug.Log($"Level increased to {level}");
    }

    public void ResetLevel()
    {
        level = 1;
    }
}

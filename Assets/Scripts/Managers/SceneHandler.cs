using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class SceneHandler : SingletonMonoBehavior<SceneHandler>
{

    [Header("Scene Data")]
    [SerializeField] private List<string> levels;
    [SerializeField] private string menuScene;
    [Header("Transition Animation Data")]
    [SerializeField] private Ease animationType;
    [SerializeField] private float animationDuration;
    [SerializeField] private RectTransform transitionCanvas;
    private int nextLevelIndex;
    private string currentLevel;
    private float initXPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        nextLevelIndex = 0;
        base.Awake();
        initXPosition = transitionCanvas.transform.localPosition.x;
        SceneManager.LoadScene(menuScene);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode _)
    {
        transitionCanvas.DOLocalMoveX(initXPosition, animationDuration).SetEase(animationType);
    }

    public void LoadNextScene()
    {
        if (nextLevelIndex >= levels.Count)
        {
            LoadMenuScene();
        }
        else
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition(levels[nextLevelIndex]));
            nextLevelIndex++;
        }
    }

    public void LoadMenuScene()
    {
        StartCoroutine(LoadSceneAfterTransition(menuScene));
        nextLevelIndex = 0;
    }

    private IEnumerator LoadSceneAfterTransition(string scene)
    {
        yield return new WaitForSeconds(animationDuration);
        SceneManager.LoadScene(scene);
        PlayMusic(nextLevelIndex);
    }

    public void RestartGame()
    {
        nextLevelIndex = 0;
        LoadNextScene();
    }

    private IEnumerator LoadInitialScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(menuScene);
    }

    public int GetLevel()
    {
        return this.nextLevelIndex - 1;
    }

    public void PlayMusic(int level)
    {
        switch (level)
        {
            case 0:
                AudioManager.Instance.PlayMusic("Menu");
                break;
            case 1:
                AudioManager.Instance.PlayMusic("Air Level");
                break;
            case 2:
                AudioManager.Instance.PlayMusic("Water Level");
                break;
            case 3:
                AudioManager.Instance.PlayMusic("Earth Level");
                break;
            case 4:
                AudioManager.Instance.PlayMusic("Fire Level");
                break;
            default:
                AudioManager.Instance.PlayMusic("Spirit Level");
                break;
        }
    }
}


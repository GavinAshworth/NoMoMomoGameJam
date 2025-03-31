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

    private string currentLevel;
    private float initXPosition;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        initXPosition = transitionCanvas.transform.localPosition.x;
        SceneManager.LoadScene(menuScene);
        SceneManager.sceneLoaded += OnSceneLoad;
        currentLevel = "Start";
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode _)
    {
        transitionCanvas.DOLocalMoveX(initXPosition, animationDuration).SetEase(animationType);
    }

    public void LoadNextScene()
    {
        if(currentLevel.Equals("SpiritLevel"))
        {
            LoadMenuScene();
        } 
        else if(currentLevel.Equals("Start"))
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition("AirLevel"));
            currentLevel = "AirLevel";
        } 
        else if(currentLevel.Equals("AirLevel"))
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition("WaterLevel"));
            currentLevel = "WaterLevel";
        }  
        else if(currentLevel.Equals("WaterLevel"))
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition("EarthLevel"));
            currentLevel = "EarthLevel";
        }  
        else if(currentLevel.Equals("EarthLevel"))
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition("FireLevel"));
            currentLevel = "FireLevel";
        } 
        else if(currentLevel.Equals("FireLevel"))
        {
            transitionCanvas.DOLocalMoveX(initXPosition + transitionCanvas.rect.width, animationDuration).SetEase(animationType);
            StartCoroutine(LoadSceneAfterTransition("SpiritLevel"));
            currentLevel = "SpiritLevel";
        } 
    }

    public void LoadMenuScene() {
        StartCoroutine(LoadSceneAfterTransition(menuScene));
        currentLevel = "Start";
    }

    private IEnumerator LoadSceneAfterTransition(string scene)
    {
        Debug.Log(scene);
        yield return new WaitForSeconds(animationDuration);
        SceneManager.LoadScene(scene);
    }

    public void RestartGame() {
        nextLevelIndex = 0;
        LoadNextScene();
    }

    private IEnumerator LoadInitialScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(menuScene);
    }

    public int GetLevel() {
        return this.nextLevelIndex - 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : SingletonMonoBehavior<SceneManager>
{
    [Header("Scene Data")]
    [SerializeField] private List<string> levels;
    [SerializeField] private string menuScene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(-0)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverPopup;
    [SerializeField] private Momo momo;

    // Removed serialized UI fields - we'll find these by tag this is to prevent some issues with ui not maintaing
    private TMP_Text scoreText;
    private TMP_Text finalScoreText;
    private HealthUI healthUI;
    private TimerBarUI timerBarUI;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 50;
    public bool[] abilities { get; private set; } = new bool[4];
    public int level { get; private set; } = 1;
    [SerializeField] public float timeLimit { get; private set; } = 60f;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) {
            momo = playerObj.GetComponent<Momo>();
        }

        // Find and cache all UI elements by tag
        FindUIElements();
        
        // Update UI with current values
        RefreshUI();
    }

    private void FindUIElements()
    {
        // Find and assign UI elements by their tags
        GameObject healthUIObj = GameObject.FindGameObjectWithTag("HealthUI");
        if (healthUIObj != null) healthUI = healthUIObj.GetComponent<HealthUI>();

        GameObject scoreTextObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (scoreTextObj != null) scoreText = scoreTextObj.GetComponent<TMP_Text>();

        GameObject timerBarObj = GameObject.FindGameObjectWithTag("TimerBar");
        if (timerBarObj != null) timerBarUI = timerBarObj.GetComponent<TimerBarUI>();

        GameObject finalScoreObj = GameObject.FindGameObjectWithTag("finalScoreText");
        if (finalScoreObj != null) finalScoreText = finalScoreObj.GetComponent<TMP_Text>();

        GameObject gameOverPanelObj = GameObject.FindGameObjectWithTag("GameOverPanel");
        if (gameOverPanelObj != null) 
        {
            gameOverPopup = gameOverPanelObj;
            gameOverPopup.SetActive(false); // Ensure it starts hidden
        }
        else
        {
            Debug.LogWarning("GameOverPanel not found with tag 'GameOverPanel'");
        }
        // Log warnings if any UI elements are missing
        if (healthUI == null) Debug.LogWarning("HealthUI not found with tag 'HealthUI'");
        if (scoreText == null) Debug.LogWarning("ScoreText not found with tag 'ScoreText'");
        if (timerBarUI == null) Debug.LogWarning("TimerBar not found with tag 'TimerBar'");
        if (finalScoreText == null) Debug.LogWarning("FinalScoreText not found with tag 'finalScoreText'");
    }

    private void RefreshUI()
    {
        // Update all UI elements with current values
        if (scoreText != null) scoreText.text = score.ToString("D5");
        if (finalScoreText != null) finalScoreText.text = score.ToString("D5");
        if (healthUI != null) healthUI.UpdateHearts(lives);
        if (timerBarUI != null) timerBarUI.ResetTimer();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    public void RespawnForCost(){
        Respawn();
        AddScore(-5000); //Costs 5000 points to get revived
        SetLives(3); //Full lives
        timerBarUI.ResetTimer(); //Reset the timer
        gameOverPopup.SetActive(false);
    }

    private void Start(){
        NewGame();
    }

    private void Update()
    {
        if (timerBarUI != null && timerBarUI.getTime() <= 0f)
        {
            HasDied(1);
            timerBarUI.ResetTimer();
        }
    }

    public void NewGame(){
        timerBarUI.Play();
        gameOverPopup.SetActive(false);
        SetScore(0);
        SetLives(3);
        abilities = new bool[] {false,false,false,false};
        if (LevelHandler.Instance != null) {
            LevelHandler.Instance.ResetLevel();
            SceneHandler.Instance.RestartGame();
        } else {
            Debug.LogError("LevelHandler not found!");
        }
    }

    public void LevelUp(){
        //Called when Momo hits a checkpoint 
        GameManager.Instance.AddScore(300);
        level += 1;
        if (timerBarUI != null)
        {
            timerBarUI.ResetTimer();
        }
        else
        {
            Debug.LogWarning("TimerBar image is destroyed!");
        }
        SceneHandler.Instance.LoadNextScene();
        LevelHandler.Instance.IncrementLevel();
    }

    public void UnlockAbility(int ability){
        //When momo collects the elemental upgrade this will get called
        abilities[ability] = true;
    }

    private void SetScore(int score)
    {
        this.score = score;

        if (scoreText != null)
        {
            scoreText.text = score.ToString("D5"); // Displays like "Score: 00025"
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = score.ToString("D5");
        }
    }

    public void AddScore(int amount)
    {
        SetScore(score + amount);
    }



    private void SetLives(int lives){
        
        this.lives = lives;
        //ui changes here
        if (healthUI != null)
        {
        healthUI.UpdateHearts(this.lives);
        }
    }

    public void HasDied(int damage){
        //Gets called when momo takes damage
        SetLives(lives - damage);

        //if all lives gone then momo has lost the game
        if(lives<=0){
            Invoke(nameof(GameOver), 0.5f);
        }
        else{
            //Momo still has lives so we respawn him
            Invoke(nameof(Respawn), 1f);
        }

    }
    
    private void Respawn(){
        momo.Respawn();
    }

    private void GameOver(){
        //Called when momo loses all lives
        momo.gameObject.SetActive(false);
        //Game Over Screen here
        gameOverPopup.SetActive(true);
        timerBarUI.ResetTimer();
        timerBarUI.Pause();
    }
    
    public void Heal(){
        if(lives<3){ // set to 3 when done developing
            SetLives(lives + 1);
        }
        else{
            Debug.Log("Full Lives");
        }
        
    }

    public void LoadStartScene()
    {
        gameOverPopup.SetActive(false);
        SceneHandler.Instance.LoadMenuScene();
        Destroy(gameObject);
    }

    public void MadeItHome() 
    {
        AddScore(300);
        timerBarUI.AddTime(15f);
    }

    public void BrokeCystal() 
    {
        AddScore(500);
        timerBarUI.AddTime(30f);
    }

    public void BeatAzula(){
        AddScore(3000);
        gameOverPopup.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-0)]
public class GameManager : MonoBehaviour
{
    //This is our Game Manager script. This will keep track of 
        //Lives
        //Unlocked Abilities
        //Levels
        //Score
        //Timer
    //Also includes all the associated methods for updating the things above

    public static GameManager Instance { get; private set; } //singleton
    public GameObject gameOverPopup; 
    [SerializeField] private Momo momo; //Instance of our Momo script attached to momo

    [SerializeField] private TMPro.TMP_Text scoreText;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 50;
    public bool[] abilities { get; private set; }  = new bool[4];
    public int level { get; private set; } = 1;
    [SerializeField] public float timeLimit { get; private set; } = 60f;
    public TimerBarUI timerBarUI;

    //for managing the health bar
    [SerializeField] private HealthUI healthUI;


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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            momo = playerObj.GetComponent<Momo>();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void Start(){
        NewGame();
    }

    private void Update()
    {
        if (timerBarUI != null && timerBarUI.getTime() <= 0f)
        {
            HasDied(1);
            // GameOver();
        }
    }

    public void NewGame(){
        timerBarUI.Play();
        Respawn();
        gameOverPopup.SetActive(false);
        SetScore(0);
        SetLives(3);
        abilities = new bool[] {false,false,false,false};
        if (LevelHandler.Instance != null) {
            LevelHandler.Instance.ResetLevel();
        } else {
            Debug.LogError("LevelHandler not found!");
        }
    }

    public void LevelUp(){
        //Called when Momo hits a checkpoint 
        GameManager.Instance.AddScore(300);
        level += 1;
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
    }
}

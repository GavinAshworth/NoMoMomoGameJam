using UnityEngine;

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

    [SerializeField] private Momo momo; //Instance of our Momo script attached to momo

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 50;
    public bool[] abilities { get; private set; }  = new bool[4];
    public float timeLimit { get; private set; } = 300f;


    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start(){
        NewGame();
    }
    private void NewGame(){
        SetScore(0);
        SetLives(50);
        abilities = new bool[] {false,false,false,false};
        if (LevelHandler.Instance != null) {
            LevelHandler.Instance.ResetLevel();
        } else {
            Debug.LogError("LevelHandler not found!");
        }
    }

    public void LevelUp(){
        if (LevelHandler.Instance != null) {
            LevelHandler.Instance.IncrementLevel();
            SceneHandler.Instance.LoadNextScene();
        } else {
            Debug.LogError("LevelHandler not found!");
        }
    }

    public void UnlockAbility(int ability){
        //When momo collects the elemental upgrade this will get called
        abilities[ability] = true;
    }

    private void SetScore(int score){
        this.score = score;
        //ui changes here
    }

    private void SetLives(int lives){
        this.lives = lives;
        //ui changes here
    }

    public void HasDied(int damage){
        //Gets called when momo takes damage
        SetLives(lives - damage);

        //if all lives gone then momo has lost the game
        if(lives<=0){
            Invoke(nameof(GameOver), 2f);
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

    }
    public void Heal(){
        if(lives<50){ // set to 3 when done developing
            SetLives(lives + 1);
        }
        else{
            Debug.Log("Full Lives");
        }
        
    }

}

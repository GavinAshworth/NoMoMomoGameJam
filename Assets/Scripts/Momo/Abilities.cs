using UnityEngine;
using UnityEngine.InputSystem;

// Class for our elemental abilities
public class Abilities : MonoBehaviour
{
    //our elemental animations prefabs
    [Header("Ability Prefabs")]
    [SerializeField] private GameObject airEffectPrefab;
    [SerializeField] private GameObject waterEffectPrefab;
    [SerializeField] private GameObject earthEffectPrefab;
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private bool isTestMode;

    private GameObject effect;

    private bool isFlying = false;
    private bool isShielded = false;
    private bool isFire = false;
    private Momo momo; //Our momo script 
    private bool isAbilityActive = false; // Flag to track if an ability is currently active

    private void Start(){
        momo = GetComponent<Momo>();
    }

    public void OnAirAbility(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.level > 1 || isTestMode) {
            if (context.performed && !isAbilityActive)
            {
                SpawnEffect(airEffectPrefab);
                //  momo will speed up for a few seconds
                isFlying = true;
            }
        }
    }

    public void OnWaterAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive && (GameManager.Instance.level > 2 || isTestMode))
        {
            SpawnEffect(waterEffectPrefab);
             // momo regenerates 1 life
            //  GameManager.Instance.Heal(); this is what it will do once game manager is set up
        }
    }

    public void OnEarthAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive && (GameManager.Instance.level > 3 || isTestMode))
        {
            SpawnEffect(earthEffectPrefab);
            // momo will get a shield for a few seconds
            isShielded = true;
        }
    }

    public void OnFireAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive && (GameManager.Instance.level > 4 || isTestMode))
        {
            SpawnEffect(fireEffectPrefab);
             // Momo will shoot out a fire explosion, this is to destroy the crystals on the boss level
            isFire = true;
        }
    }

    private void SpawnEffect(GameObject effectPrefab)
    {
        if (effectPrefab == null)
        {
            Debug.LogError("Effect prefab is not assigned!");
            return;
        }

        // Set the ability as active
        isAbilityActive = true;

        // Instantiate the effect prefab as a child of Momo so we can make it follow him
        effect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);

        // Get the Animator component of the effect
        Animator effectAnimator = effect.GetComponent<Animator>();
        if (effectAnimator != null)
        {
            // Play the animation
            effectAnimator.Play(0, 0, 0f);
        }

        // Destroy the effect after the animation finishes
        float animationLength = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        // Reset the ability flag after the animation finishes
        StartCoroutine(ResetAbility(animationLength));
    }

    private System.Collections.IEnumerator ResetAbility(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        if(!isAbilityActive) yield break;
        StopAbility();
    }

    public void StopAbility(){
        Destroy(effect);
        isAbilityActive = false;
        isFlying = false;
        isShielded = false;
        isFire = false;
    }

    public bool GetIsFlying(){
        return isFlying;
    }
    public bool GetIsShielded(){
        return isShielded;
    }
    public bool GetIsFire(){
        return isFire;
    }
}
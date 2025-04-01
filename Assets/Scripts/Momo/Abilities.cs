using UnityEngine;
using UnityEngine.InputSystem;

public class Abilities : MonoBehaviour
{
    [Header("Ability Prefabs")]
    [SerializeField] private GameObject airEffectPrefab;
    [SerializeField] private GameObject waterEffectPrefab;
    [SerializeField] private GameObject earthEffectPrefab;
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private bool isTestMode;

    [Header("Cooldown Durations (seconds)")]
    private float airCooldown = 3f;
    private float waterCooldown = 7f;
    private float earthCooldown = 5f;
    private float fireCooldown = 12f;

    private float airCooldownTimer = 0f;
    private float waterCooldownTimer = 0f;
    private float earthCooldownTimer = 0f;
    private float fireCooldownTimer = 0f;

    private GameObject effect;
    private bool isFlying = false;
    private bool isShielded = false;
    private bool isFire = false;
    private bool isAbilityActive = false;

    [Header("UI Cooldown Fill Images")]
    [SerializeField] private UnityEngine.UI.Image airCooldownImage;
    [SerializeField] private UnityEngine.UI.Image waterCooldownImage;
    [SerializeField] private UnityEngine.UI.Image earthCooldownImage;
    [SerializeField] private UnityEngine.UI.Image fireCooldownImage;

    private Momo momo;

    private void Start() {
        momo = GetComponent<Momo>();
    }

    private void Update() {
        // Decrease cooldown timers each frame
        airCooldownTimer -= Time.deltaTime;
        waterCooldownTimer -= Time.deltaTime;
        earthCooldownTimer -= Time.deltaTime;
        fireCooldownTimer -= Time.deltaTime;

         // Clamp to prevent negative timers
        airCooldownTimer = Mathf.Max(airCooldownTimer, 0f);
        waterCooldownTimer = Mathf.Max(waterCooldownTimer, 0f);
        earthCooldownTimer = Mathf.Max(earthCooldownTimer, 0f);
        fireCooldownTimer = Mathf.Max(fireCooldownTimer, 0f);

        // Update UI fill amounts (0 = ready, 1 = cooling down)
        airCooldownImage.fillAmount = 1f - (airCooldownTimer / airCooldown);
        waterCooldownImage.fillAmount = 1f - (waterCooldownTimer / waterCooldown);
        earthCooldownImage.fillAmount = 1f - (earthCooldownTimer / earthCooldown);
        fireCooldownImage.fillAmount = 1f - (fireCooldownTimer / fireCooldown);
    }


    public void OnAirAbility(InputAction.CallbackContext context) {
        if ((LevelHandler.Instance.level > 1 || isTestMode) && context.performed && !isAbilityActive && airCooldownTimer <= 0f) {
            SpawnEffect(airEffectPrefab);
            isFlying = true;
            airCooldownTimer = airCooldown;
        }
    }

    public void OnWaterAbility(InputAction.CallbackContext context) {
        if ((LevelHandler.Instance.level > 2 || isTestMode) && context.performed && !isAbilityActive && waterCooldownTimer <= 0f) {
            SpawnEffect(waterEffectPrefab);
            GameManager.Instance.Heal();
            waterCooldownTimer = waterCooldown;
            AudioManager.Instance.PlaySFX("Water Ability");
        }
    }

    public void OnEarthAbility(InputAction.CallbackContext context) {
        if ((LevelHandler.Instance.level > 3 || isTestMode) && context.performed && !isAbilityActive && earthCooldownTimer <= 0f) {
            SpawnEffect(earthEffectPrefab);
            isShielded = true;
            earthCooldownTimer = earthCooldown;
            AudioManager.Instance.PlaySFX("Earth Ability");
        }
    }

    public void OnFireAbility(InputAction.CallbackContext context) {
        if ((LevelHandler.Instance.level > 4 || isTestMode) && context.performed && !isAbilityActive && fireCooldownTimer <= 0f) {
            SpawnEffect(fireEffectPrefab);
            fireCooldownTimer = fireCooldown;
            // Momo will shoot out a fire explosion, this is to destroy the crystals on the boss level
            isFire = true;
            AudioManager.Instance.PlaySFX("Fire Ability");
        }
    }

    private void SpawnEffect(GameObject effectPrefab) {
        if (effectPrefab == null) {
            Debug.LogError("Effect prefab is not assigned!");
            return;
        }

        isAbilityActive = true;

        effect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);

        Animator effectAnimator = effect.GetComponent<Animator>();
        if (effectAnimator != null) {
            effectAnimator.Play(0, 0, 0f);
        }

        float animationLength = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(ResetAbility(animationLength));
    }

    private System.Collections.IEnumerator ResetAbility(float animationLength) {
        yield return new WaitForSeconds(animationLength);
        if (!isAbilityActive) yield break;
        StopAbility();
    }

    public void StopAbility() {
        if (effect != null) Destroy(effect);
        isAbilityActive = false;
        isFlying = false;
        isShielded = false;
        isFire = false;
    }

    public bool GetIsFlying() => isFlying;
    public bool GetIsShielded() => isShielded;
    public bool GetIsFire() => isFire;
}

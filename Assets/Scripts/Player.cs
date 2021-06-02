using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public Rigidbody2D body;
  public Animator animator;
  public ParticleSystem dustParticles;
  public EnergyBar energyBar;
  public HealthBar healthBar;
  public PauseDialog pauseDialog;
  public GameObject finishDialog;

  public float agility = 5f;
  public float moveSpeed = 15f;
  public float boostSpeed = 3f;
  public int energyRechargeRate = 25;
  public int boostEnergyCost = 100;
  public int maxHealth = 3;
  public int currentHealth = 3;
  public int invulnerableTimeAfterHit = 2;

  public int maxEnergy = 100;
  public float currentEnergy = 100f;
  private bool consumingEnergy = false;

  private bool upDown = false;
  private bool downDown = false;
  private int carrotsPicked = 0;
  private int collisions = 0;
  private bool invulnerable = false;

  void Start() {
    currentEnergy = maxEnergy;
    currentHealth = maxHealth;
    energyBar.SetMaxEnergy(maxEnergy);
    healthBar.SetMaxHealth(maxHealth);
  }

  // Update is called once per frame
  void Update()
  {
    RechargeEnergy();

    if (Input.touchCount > 0) {
      Touch touch = Input.GetTouch(0);
      if (touch.phase == TouchPhase.Moved) {
        Vector2 delta = touch.deltaPosition * agility * Time.deltaTime;
        Vector2 touchPos = touch.position;
        Vector2 camPosition = Camera.main.WorldToScreenPoint(transform.position);
        float currentAngle = Mathf.Atan2(touchPos.x - camPosition.x, touchPos.y - camPosition.y) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(touchPos.x + delta.x - camPosition.x, touchPos.y + delta.y - camPosition.y) * Mathf.Rad2Deg;
        Vector3 rotation = transform.eulerAngles;
        rotation.z -= (currentAngle - deltaAngle);
        transform.eulerAngles = rotation;
      } else if (touch.tapCount == 2) {
        Boost();
      }
    } else if (upDown || Input.GetKeyDown(KeyCode.UpArrow)) {
      upDown = true;
      Vector3 rotation = transform.eulerAngles;
      rotation.z += 10f * agility * Time.deltaTime;
      transform.eulerAngles = rotation;
    } else if (downDown || Input.GetKeyDown(KeyCode.DownArrow)) {
      downDown = true;
      Vector3 rotation = transform.eulerAngles;
      rotation.z -= 10f * agility * Time.deltaTime;
      transform.eulerAngles = rotation;
    } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
      Boost();
    } else if (Input.GetKeyDown(KeyCode.Escape)) {
      if (!pauseDialog.IsPaused()) {
        pauseDialog.PauseGame();
      } else{
        pauseDialog.ContinueGame();
      }
    }

    if (Input.GetKeyUp(KeyCode.UpArrow)) {
      upDown = false;
    }
    if (Input.GetKeyUp(KeyCode.DownArrow)) {
      downDown = false;
    }
  }

  void FixedUpdate() {
    body.AddForce(transform.right * moveSpeed);
    var main = dustParticles.main;
    main.simulationSpeed = body.velocity.magnitude + 0.1f;
  }

  void OnCollisionEnter2D(Collision2D other) {
    animator.SetBool("IsHit", true);
    TakeDamage();
  }
  void OnCollisionExit2D(Collision2D other) {
    animator.SetBool("IsHit", false);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Carrot")) {
      carrotsPicked += 1;
    }
  }

  private IEnumerator InvulnerableTimeout(int seconds) {
    invulnerable = true;
    healthBar.Disabled();
    yield return new WaitForSeconds(seconds);
    invulnerable = false;
    healthBar.Enabled();
  }

  public int GetCarrotsPicked() {
    return carrotsPicked;
  }

  public int GetCollisionCount() {
    return collisions;
  }

  private void TakeDamage() {
    if (!invulnerable && currentHealth > 0) {
      currentHealth -= 1;
      healthBar.SetHealth(currentHealth);
      StartCoroutine(InvulnerableTimeout(invulnerableTimeAfterHit));
    } else if (!invulnerable) {
      if (finishDialog != null) {
        finishDialog.SetActive(true);
      }
      collisions += 1;
    }
  }

  private void Boost() {
    if (currentEnergy >= boostEnergyCost) {
      body.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
      currentEnergy -= boostEnergyCost;
      energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
      if (currentEnergy < boostEnergyCost) {
        energyBar.Disabled();
      }
    }
  }

  private void RechargeEnergy() {
    if (!consumingEnergy && currentEnergy < maxEnergy) {
      currentEnergy += energyRechargeRate * Time.deltaTime;
      currentEnergy = Mathf.Min(maxEnergy, currentEnergy);
      energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
      if (currentEnergy >= boostEnergyCost) {
        energyBar.Enabled();
      }
    }
  }
}

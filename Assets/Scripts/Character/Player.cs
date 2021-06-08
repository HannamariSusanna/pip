using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
  public Rigidbody2D body;
  public Animator animator;
  public HealthBar healthBar;
  public Ability ability;

  public float agility = 5f;
  public float moveSpeed = 15f;
  public int energyRechargeRate = 25;
  
  public int maxHealth = 3;
  public int currentHealth = 3;
  public int invulnerableTimeAfterHit = 2;
  public int maxEnergy = 100;

  private GameMode gameMode;
  private int carrotsPicked = 0;
  private int collisions = 0;
  private bool invulnerable = false;
  private bool upDown = false;
  private bool downDown = false;
  private float sqrMaxVelocity;

  protected void Start() {
    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    sqrMaxVelocity = (transform.right * moveSpeed).sqrMagnitude;
  }

  void Update() {
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
          ability.Use();
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
      ability.Use();
    }

    if (Input.GetKeyUp(KeyCode.UpArrow)) {
      upDown = false;
    }
    if (Input.GetKeyUp(KeyCode.DownArrow)) {
      downDown = false;
    }
  }

  void FixedUpdate() {
    if (body.velocity.sqrMagnitude < sqrMaxVelocity) {
      body.velocity = transform.right * moveSpeed;
    }
  }

  void OnCollisionEnter2D(Collision2D other) {
    animator.SetBool("IsHit", true);
    TakeDamage();
  }
  void OnCollisionExit2D(Collision2D other) {
    animator.SetBool("IsHit", false);
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Carrot")) {
      gameMode.IncreaseCarrotsPicked(1);
    }
  }

  private IEnumerator InvulnerableTimeout(int seconds) {
    invulnerable = true;
    healthBar.Disabled();
    yield return new WaitForSeconds(seconds);
    invulnerable = false;
    healthBar.Enabled();
  }

  public void SetGameMode(GameMode gameMode) {
    this.gameMode = gameMode;
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
      gameMode.IncreaseCollisions(1);
    }
  }
}

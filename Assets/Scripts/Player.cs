using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public Rigidbody2D body;
  public Animator animator;
  public ParticleSystem dustParticles;
  public EnergyBar energyBar;

  public float agility = 5f;
  public float moveSpeed = 15f;
  public float boostSpeed = 3f;
  public int energyRechargeRate = 25;
  public int boostEnergyCost = 100;

  public int maxEnergy = 100;
  public float currentEnergy = 100f;
  private bool consumingEnergy = false;

  private bool upDown = false;
  private bool downDown = false;

  void Start() {
    currentEnergy = maxEnergy;
    energyBar.SetMaxEnergy(maxEnergy);
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

  void OnCollisionEnter2D(Collision2D col) {
    animator.SetBool("IsHit", true);
  }
  void OnCollisionExit2D(Collision2D other) {
    animator.SetBool("IsHit", false);
  }

  private void Boost() {
    if (currentEnergy >= boostEnergyCost) {
      body.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
      currentEnergy -= boostEnergyCost;
      energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
    }
  }

  private void RechargeEnergy() {
    if (!consumingEnergy && currentEnergy < maxEnergy) {
      currentEnergy += energyRechargeRate * Time.deltaTime;
      currentEnergy = Mathf.Min(maxEnergy, currentEnergy);
      energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
    }
  }
}

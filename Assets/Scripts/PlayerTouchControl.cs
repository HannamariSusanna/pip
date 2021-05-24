using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchControl : MonoBehaviour
{
  public Rigidbody2D body;
  public Animator animator;
  public ParticleSystem dustParticles;

  public float agility = 5f;
  public float moveSpeed = 15f;
  public float boostSpeed = 3f;

  private bool boost = false;
  private bool upDown = false;
  private bool downDown = false;

  // Update is called once per frame
  void Update()
  {
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
        boost = true;
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
      boost = true;
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
    if (boost) {
      body.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
      boost = false;
    }
    var main = dustParticles.main;
    main.simulationSpeed = body.velocity.magnitude + 0.1f;
  }

  void OnCollisionEnter2D(Collision2D col) {
    animator.SetBool("IsHit", true);
  }
  void OnCollisionExit2D(Collision2D other) {
    animator.SetBool("IsHit", false);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{

    public Transform player;
    public Transform target;
    public RectTransform compass;
    public Camera uiCamera;
    
    private CanvasRenderer canvasRenderer;
    private Vector3 originalPos;

    void Start() {
        canvasRenderer = GetComponent<CanvasRenderer>();
        originalPos = compass.position;
    }

    // Update is called once per frame
    void Update()
    {
        float compassOffset = 120f;
        Vector2 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(target.position);
        bool isOffScreen = targetPositionScreenPoint.x < compassOffset ||
            targetPositionScreenPoint.x > Screen.width - compassOffset ||
            targetPositionScreenPoint.y < compassOffset ||
            targetPositionScreenPoint.y > Screen.height -compassOffset;
        
        if (isOffScreen) {
            // Show the needle
            canvasRenderer.SetAlpha(255f);
            RotateTowardsTarget();
            compass.position = originalPos;
        } else {
            // Hide the needle
            canvasRenderer.SetAlpha(0f);
            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
            compass.position = pointerWorldPosition;
            compass.localPosition = new Vector3(compass.localPosition.x, compass.localPosition.y, 0f);
        }
    }

    private void RotateTowardsTarget() {
        Vector3 targetDir = target.position - player.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg);
        Vector3 playerAngle = player.eulerAngles;
        playerAngle.z *= -1;
        playerAngle.z += angle;
        transform.eulerAngles = playerAngle;
    }
}

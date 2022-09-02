using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    
    private Rigidbody2D currentBallRB;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true;
        currentBallRB.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRB.position = worldPosition;

    }

    private void LaunchBall()
    {
        currentBallRB.isKinematic = false;
        currentBallRB = null;

        Invoke(nameof(DetachBall), detachDelay);

    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRB = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }
}

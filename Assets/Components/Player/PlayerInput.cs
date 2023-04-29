using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public Animator animator;
    public GameObject player;
    public Rigidbody2D playerRB;
    public SpriteRenderer spriteRenderer;
    bool faceLeft = true;
    public float movementSpeed = 1000f;

    public float jumpHeight = 400f;

    //bool allowJump = true;
    bool isJumping = false;
    bool isMoving = true;

    float moveHorizontal = 0f;
    float moveVertical = 0f;

    void Start() {
        playerRB = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        Debug.Log("Started");
    }

    void Update() {
        HandleAnimator();

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        isMoving = Mathf.Abs(moveHorizontal) > 0.1f;
        if (isMoving) {
            faceLeft = moveHorizontal < 0f;
        }
    }

    void FixedUpdate() {
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f) {
            playerRB.AddForce(new Vector2(moveHorizontal * movementSpeed, 0f), ForceMode2D.Impulse);
        }
        if (!isJumping) {
            if (moveVertical > 0.1f || moveVertical < -0.1f) {
                playerRB.AddForce(new Vector2(0f, moveVertical * jumpHeight), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        isJumping = false;
    }

    private void HandleAnimator() {
        float moveX = Input.GetAxis("Horizontal");

        animator.SetBool("faceLeft", moveX <= 0);
        animator.SetFloat("moveX", moveX);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", isJumping);
        spriteRenderer.flipX = !faceLeft;

    }
}

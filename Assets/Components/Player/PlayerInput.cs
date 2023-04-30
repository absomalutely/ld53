using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public Animator animator;
    public GameObject player;
    public Rigidbody2D playerRB;
    public SpriteRenderer spriteRenderer;
    public bool FaceLeft { get; private set; } = true;
    public bool IsCarrying { get; set; } = false;


    public float movementSpeed = 1000f;

    public float jumpHeight = 400f;


    //bool allowJump = true;
    bool isJumping = false;
    bool canJump = true;
    bool isMoving = true;
    float moveX;

    float moveHorizontal = 0f;
    float moveVertical = 0f;

    float jumpDebounceLimit = 0.33f;
    float jumpDebounceCounter = 0f;

    void Start() {
        playerRB = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        //Debug.Log("Started");
    }

    void Update() {
        JumpDebounceTick();
        HandleAnimator();

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        isMoving = Mathf.Abs(moveHorizontal) > 0.1f;
        moveX = Input.GetAxis("Horizontal");
        if (isMoving) {
            FaceLeft = moveX <= 0;
        }
    }

    void FixedUpdate() {
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f) {
            playerRB.AddForce(new Vector2(moveHorizontal * movementSpeed, 0f), ForceMode2D.Impulse);
        }
        if (!isJumping && canJump) {
            if (moveVertical > 0.1f || moveVertical < -0.1f) {
                playerRB.AddForce(new Vector2(0f, moveVertical * jumpHeight), ForceMode2D.Impulse);
                isJumping = true;
                canJump = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        isJumping = false;
    }

    private void JumpDebounceTick() {
        if (!canJump) {
            if (jumpDebounceCounter < jumpDebounceLimit) {
                jumpDebounceCounter += Time.deltaTime;
                return;
            }
            jumpDebounceCounter = 0;
            canJump = true;
        }
    }
    private void HandleAnimator() {
        

        animator.SetBool("faceLeft", FaceLeft);
        animator.SetFloat("moveX", moveX);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isCarrying", IsCarrying);
        spriteRenderer.flipX = !FaceLeft;

    }
}

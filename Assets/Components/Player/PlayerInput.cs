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

    public float jumpHeight = 100f;

    //bool allowJump = true;
    bool isJumping = false;
    bool isMoving = true;

    Vector2 move;

    void Start() {
        playerRB = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        Debug.Log("Started");
    }

    void Update() {
        HandleAnimator();

        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            faceLeft = false;
            isMoving = true;

        } else if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.right * -movementSpeed * Time.deltaTime;
            faceLeft = true;
            isMoving = true;
        } else {
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.Space)) {
            if (!isJumping) {
                playerRB.AddForce(new Vector2(0, movementSpeed * jumpHeight));
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
        spriteRenderer.flipX = !faceLeft;

    }
}

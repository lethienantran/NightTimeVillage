using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHomeController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private BoxCollider2D checkGrounded;
    public float jumpPower, speed;
    private Animator playerAnim;
    public bool jump;
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        checkGrounded = GetComponent<BoxCollider2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("lastMoveX", 1);
    }

    // Update is called once per frame

    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void Jump()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpPower);

        }
        if (isGrounded())
        {
            jump = false;
        }
        else
        {
            jump = true;
        }
        if (jump)
        {
            playerAnim.SetBool("Jump", true);
        }
        else
        {
            playerAnim.SetBool("Jump", false);
        }
    }

    void PlayerMovement()
    {
        float dirX = Input.GetAxisRaw("Horizontal");

        playerRB.velocity = new Vector2(dirX * speed, playerRB.velocity.y);

        playerAnim.SetFloat("MoveX", playerRB.velocity.x);

        if (dirX == 1 || dirX == -1)
        {
            playerAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
        }
    }

    private bool isGrounded()
    {
        playerAnim.ResetTrigger("Jump");
        return Physics2D.BoxCast(checkGrounded.bounds.center, checkGrounded.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

}

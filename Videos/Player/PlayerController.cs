using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private BoxCollider2D checkGrounded;
    public float jumpPower, speed;
    private Animator playerAnim;
    public bool jump;
    [SerializeField] private LayerMask jumpableGround;

    PhotonView view;

    new public GameObject camera;

    public static bool isKidnap = false;

    public Text roleText;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        checkGrounded = GetComponent<BoxCollider2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("lastMoveX", 1);

        view = GetComponent<PhotonView>();

        if (!view.IsMine)
        {
            Destroy(camera);
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (view.IsMine)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            PlayerMovement();
        }

    }

    private void Jump()
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
        turnOnPlayerLight();

        if (Input.GetKey(KeyCode.T))
        {
            if (isKidnap)
            {
                Debug.Log("hi");
            }
            else
            {
                Debug.Log("no");
            }
        }
    }

    void playerGameplayInfoUI()
    {
        StartCoroutine(WaitForInfoWindowClose());
    }

    private IEnumerator WaitForInfoWindowClose()
    {
        yield return new WaitForSeconds(2.5f);
        roleText = GameObject.Find("RoleTextInfo").GetComponent<Text>();
        if (view.IsMine && isKidnap)
        {
            roleText.text = "Role: The Kidnapper";
        }
        else if(view.IsMine && !isKidnap)
        {
            roleText.text = "Role: The Villager";
        }
    }

    private bool isGrounded()
    {
        playerAnim.ResetTrigger("Jump");
        return Physics2D.BoxCast(checkGrounded.bounds.center, checkGrounded.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    [PunRPC]
    void RPC_SetKidnap()
    {
        isKidnap = true;
    }

    public void turnOnPlayerLight()
    {
        if (DayNightCycle.activateLights)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Light"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else if (!DayNightCycle.activateLights)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Light"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}

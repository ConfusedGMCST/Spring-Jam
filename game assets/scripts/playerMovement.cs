using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public charSwitch charSwitchScript;
    private bool character;
    public AudioSource[] jumps = { null, null}; //0 -> earthBoy, 1 -> airGirl};
    private float horizontal;
    private float speed = 8f;
    public float jumpingPower = 8f;
    private float jumpMod = 1f;
    private float airMod = 1.3f;
    private int charSwitchIndex = 0;
    private Vector2[] initPos = {new Vector2(0, 0), new Vector2(0, 0)}; //0 -> earthBoy, 1 -> airGirl
    public GameObject earthBoy;
    public GameObject airGirl;
    private Rigidbody2D rb;
    private Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask deathLayer;

    void Start()
    {
        setRb();
        initPos[0] = new Vector2(earthBoy.transform.position.x, earthBoy.transform.position.y);
        initPos[1] = new Vector2(airGirl.transform.position.x, airGirl.transform.position.y);
    }

    void setRb()
    {
        character = charSwitchScript.character;
        if (!character)
        {
            jumpMod = 1f;
            charSwitchIndex = 0;
            rb = earthBoy.GetComponent<Rigidbody2D>();
            groundCheck = earthBoy.transform.Find("Ground Check");
        }
        else
        {
            jumpMod = airMod;
            charSwitchIndex = 1;
            rb = airGirl.GetComponent<Rigidbody2D>();
            groundCheck = airGirl.transform.Find("Ground Check");
        }
    }

    void resetChars()
    {
        earthBoy.transform.position = initPos[0];
        airGirl.transform.position = initPos[1];
    }

    void Update()
    {
        setRb();

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * jumpMod);
            jumps[charSwitchIndex].Play();
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f * jumpMod);
        }

        if (IsDead())
        {
            resetChars();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsDead()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, deathLayer);
    }

    private void FixedUpdate()
    {
        setRb();
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }


}

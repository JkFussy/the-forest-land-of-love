using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public float Speed;
    private bool faceRight = true;

    private PlayerInput playerinput;

    private void Awake()
    {
        playerinput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        Reflect();
        Walk();
        Dash();
    }

    void Reflect()
    {

        if (!IsDashing) // ���� � ������� ������ ���� � ��������� �����, �� ��������� ��������
        {
            if ((movement.x > 0 && !faceRight) || (movement.x < 0 && faceRight))
            {
                transform.localScale *= new Vector2(-1, 1);
                faceRight = !faceRight;
            }
        }
    }

    void Walk()
    {
        
        Vector2 moveInput = InputManager.GetInstance().GetMoveDirection();
        movement.x = moveInput.x;
        movement.y = moveInput.y;
        if (IsDashing)
        {
            anim.SetFloat("x", 0);
        }
        else
        {


            if (movement.x != 0 || movement.y != 0)
            {

                anim.SetFloat("x", movement.x);
                anim.SetFloat("y", movement.y);

                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("trigger_rest_instantly", true);
                StopMoving();
            }
            rb.velocity = new Vector2(movement.x * Speed, movement.y * Speed);
        }
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    private float dashTimer = 0f;
    private bool IsDashing = false;
    

    public float dashReloadTime = 2.5f; // ����� ����������� ������ "�����"
    private bool dashReloaded = true; // ��������� ���������� � �����
    void Dash()
    {
        bool dashPressed = InputManager.GetInstance().GetDashPressed();
        if (dashPressed && dashReloaded)
        {
            dashReloaded = false;
            Invoke("DashReload", dashReloadTime);

            rb.velocity = Vector2.zero;
            IsDashing = true; // ��������� ��������� "� �����" - ��������� ����������, �������� � ������

        }

        if (IsDashing && dashTimer < dashTime) // ���� ���� � ����� � ����� ����� ��� �� �����
        {
            if (!faceRight) { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.y); }
            else { rb.velocity = new Vector2(dashSpeed, rb.velocity.y); }
            anim.SetBool("IsDashing", true);
            dashTimer += Time.deltaTime;
        }
        else { dashTimer = 0; IsDashing = false; anim.SetBool("IsIdle", true); } // ��������� ������� ����� � ���������� ��������� "� �����"
    }


    void DashReload()
    {
        dashReloaded = true; // ��������� ���������� � ���������� �����
    }

}



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public float Speed;
    private bool faceRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Reflect();
        Walk();
        Dash();
    }

    void Reflect()
    {
        if (!dashNow) // ���� � ������� ������ ���� � ��������� �����, �� ��������� ��������
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
        if (dashNow)
        {
            anim.SetFloat("x", 0);
        }
        else
       {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if(movement.x !=0 || movement.y !=0)
            {
                anim.SetFloat("x", movement.x);
                anim.SetFloat("y", movement.y);
                anim.SetTrigger("trigger_walk");
            }       
            rb.velocity = new Vector2(movement.x * Speed, movement.y * Speed);
        }
    }

    private bool dashNow = false;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    private float dashTimer = 0f;



    public float dashReloadTime = 2.5f; // ����� ����������� ������ "�����"
    private bool dashReloaded = true; // ��������� ���������� � �����
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K) && dashReloaded)
        {
            dashReloaded = false;
            Invoke("DashReload", dashReloadTime);

            anim.SetTrigger("trigger_dashing"); ;
            rb.velocity = Vector2.zero;

            dashNow = true; // ��������� ��������� "� �����" - ��������� ����������, �������� � ������
        }

        if (dashNow && dashTimer < dashTime) // ���� ���� � ����� � ����� ����� ��� �� �����
        {
            if (!faceRight) { rb.velocity = new Vector2(dashSpeed, rb.velocity.y); }
            else  { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.y); }
            if (!faceRight) { rb.velocity = new Vector2(dashSpeed, rb.velocity.x); }
            else { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.x); }

            dashTimer += Time.deltaTime;
        }
        else { dashTimer = 0; dashNow = false; anim.SetTrigger("trigger_idle"); } // ��������� ������� ����� � ���������� ��������� "� �����"
    }


    void DashReload()
    {
        dashReloaded = true; // ��������� ���������� � ���������� �����
    }

}


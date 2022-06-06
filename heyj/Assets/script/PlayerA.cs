using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerA : MonoBehaviour
{


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public float Speed;
    private bool faceRight = true;
    public FixedJoystick joystick;
    private bool IsWalking = false;
    private bool IsRest = false;
    private bool IsDashing = false;


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
        if (!dashNow) // если в текущий момент перс в состоянии Рывка, то блокируем повороты
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
            movement.x = joystick.Horizontal;
            movement.y = joystick.Vertical;

            if(movement.x !=0 || movement.y !=0)
            {
               
                anim.SetFloat("x", movement.x);
                anim.SetFloat("y", movement.y);
                if(!IsWalking)
                {
                    IsRest = false;
                    IsWalking = true;
                    anim.SetBool("IsWalking", IsWalking);
                    anim.SetBool(" IsRest", IsRest);
                }
                else
                {
                    IsWalking = false;
                    IsRest = true;
                    anim.SetBool("IsWalking", IsWalking);
                    anim.SetBool(" IsRest", IsRest);
                    StopMoving();
                }
            }       
            rb.velocity = new Vector2(movement.x * Speed, movement.y * Speed);
        }
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }


    private bool dashNow = false;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    private float dashTimer = 0f;



    public float dashReloadTime = 2.5f; // время перезарядки навыка "Рывок"
    private bool dashReloaded = true; // состояние готовности к Рывку
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K) && dashReloaded)
        {
            dashReloaded = false;
            Invoke("DashReload", dashReloadTime);

            anim.SetTrigger("trigger_dashing"); ;
            rb.velocity = Vector2.zero;

            dashNow = true; // включение состояния "в Рывке" - БЛОКИРУЕТ УПРАВЛЕНИЕ, ПОВОРОТЫ И ПРЫЖКИ
        }

        if (dashNow && dashTimer < dashTime) // если перс в Рывке и время Рывка ещё не вышло
        {
            if (!faceRight) { rb.velocity = new Vector2(dashSpeed, rb.velocity.y); }
            else  { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.y); }
            if (!faceRight) { rb.velocity = new Vector2(dashSpeed, rb.velocity.x); }
            else { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.x); }

            dashTimer += Time.deltaTime;
        }
        else { dashTimer = 0; dashNow = false; } // обнуление таймера рывка и отключение состояния "в Рывке"
    }


    void DashReload()
    {
        dashReloaded = true; // включение готовности к следующему Рывку
    }

}


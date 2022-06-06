using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.InputSystem;

public class PlayerA1 : MonoBehaviour
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

    private void OnEnable()
    {
        playerinput.Enable();
    }
    private void OnDisable()
    {
        playerinput.Disable();
    }


    private void Update()
    {
        Reflect();
        Walk();
        Dash();
    }

    void Reflect()
    {
        if (!IsDashing) // если в текущий момент перс в состоянии Рывка, то блокируем повороты
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
        if (IsDashing)
        {
            anim.SetFloat("x", 0);
        }
        else
       {
            Vector2 moveInput = playerinput.Player.Move.ReadValue<Vector2>();
            movement.x = moveInput.x;
            movement.y = moveInput.y;

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
            rb.velocity = moveInput * Speed;
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

    public float dashReloadTime = 2.5f; // время перезарядки навыка "Рывок"
    private bool dashReloaded = true; // состояние готовности к Рывку
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K) && dashReloaded)
        {
            dashReloaded = false;
            Invoke("DashReload", dashReloadTime);

            rb.velocity = Vector2.zero;
            IsDashing = true; // включение состояния "в Рывке" - БЛОКИРУЕТ УПРАВЛЕНИЕ, ПОВОРОТЫ И ПРЫЖКИ
            
        }

        if (IsDashing && dashTimer < dashTime) // если перс в Рывке и время Рывка ещё не вышло
        {
            if (!faceRight) { rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.y); }
            else  { rb.velocity = new Vector2(dashSpeed, rb.velocity.y); }
            anim.SetBool("IsDashing", true);
            dashTimer += Time.deltaTime;
        }
        else { dashTimer = 0; IsDashing = false; anim.SetBool("IsIdle", true); } // обнуление таймера рывка и отключение состояния "в Рывке"
    }


    void DashReload()
    {
        dashReloaded = true; // включение готовности к следующему Рывку
    }

}


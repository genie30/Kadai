using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private JoyStick stick = null;
    [SerializeField]
    private float speed;
    [SerializeField]
    private CharacterController cc = null;
    [SerializeField]
    Animator anim = null;

    [SerializeField]
    float stepspeed = 800;

    private void FixedUpdate()
    {
        if (anim.GetBool("KnockBack")) return;

        if (GuardButton.Guard)
        {
            if (stick.Position.x > 0.5f || stick.Position.x < -0.5f || Input.GetAxis("Horizontal") != 0)
            {
                PlayerStep();
            }
        }
        else
        {
            if (stick.Position.x > 0.2f || stick.Position.x < -0.2f || Input.GetAxis("Horizontal") != 0)
            {
                PlayerRotation();
            }

            if (stick.Position.y > 0.2f || stick.Position.y < -0.2f || Input.GetAxis("Vertical") != 0)
            {
                PlayerMove();
            }

            MoveAnimSelector();
        }
    }

    private void PlayerStep()
    {
        var step = stick.Position.x > 0 ? 1 : -1;
        if(Input.GetAxis("Horizontal") < 0) step = -1;
        if (Input.GetAxis("Horizontal") > 0) step = 1;

        var move = (transform.right * step) * stepspeed * Time.deltaTime;
        cc.SimpleMove(move);
    }

    private void PlayerRotation()
    {
        var inp = stick.Position.x;
        if (Input.GetAxis("Horizontal") != 0) inp = Input.GetAxis("Horizontal");

        var step = (speed * inp * Time.deltaTime) / 4f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(0f, 90f, 0f), step);
    }

    private void PlayerMove()
    {
        var inp = stick.Position.y;
        if (Input.GetAxis("Vertical") != 0) inp = Input.GetAxis("Vertical");

        var curSpeed = inp * speed * Time.deltaTime;
        if (inp < 0f) curSpeed /= 2;
        var move = transform.forward * curSpeed;
        cc.SimpleMove(move);
    }

    private void MoveAnimSelector()
    {
        bool front, back;
        front = stick.Position.y > 0.2f || Input.GetKey("w");
        back = stick.Position.y < -0.2f || Input.GetKey("s");
        anim.SetBool("Move", front);
        anim.SetBool("LockMoveB", back);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (anim.GetBool("KnockBack") || anim.GetBool("Die")) return;
        if(other.tag == "EnemyAttack")
        {
            anim.SetTrigger("Damage");
        }
    }
}

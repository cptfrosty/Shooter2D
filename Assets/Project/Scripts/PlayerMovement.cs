using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float idle;
    public float run;

    [HideInInspector]
    public Rigidbody2D rb2d;
    private PlayerView playerView;
    public Vector2 VectorMove;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerView = GetComponent<PlayerView>();
    }

    private void Update()
    {
        Move();
    }

    //Движение за мышкой
    /*void MoveLookMouse()
    {
        

        if (Input.GetKey(KeyCode.LeftShift)) speed = idle;
        else speed = run;

        //rb2d.AddForce(transform.right * VectorMove * speed * Time.deltaTime);

        rb2d.AddForce(transform.up * VectorMove.x * idle);
        rb2d.AddForce(transform.right * VectorMove.y * idle);

        if (VectorMove.x == 0 && VectorMove.y == 0)
        {
            rb2d.velocity = Vector2.zero;
        }
    }*/

    void Move()
    {
        VectorMove.x = Input.GetAxis("Horizontal");
        VectorMove.y = Input.GetAxis("Vertical");
        rb2d.MovePosition(rb2d.position + VectorMove * speed * Time.deltaTime);
    }
}

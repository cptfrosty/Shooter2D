using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementSystem : NetworkBehaviour
{
    public float speed;
    public float idle;
    public float run;

    [HideInInspector]
    public Rigidbody2D rb2d;
    public Vector2 VectorMove;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (!isLocalPlayer) return;

        VectorMove.x = Input.GetAxis("Horizontal");
        VectorMove.y = Input.GetAxis("Vertical");
        rb2d.MovePosition(rb2d.position + VectorMove * speed * Time.deltaTime);
    }
}

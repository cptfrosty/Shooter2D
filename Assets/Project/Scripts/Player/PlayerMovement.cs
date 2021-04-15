using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float idle;
    [SerializeField] private float run;

    [SerializeField] private bool _isCanMove = true;
    public bool SetCanMove
    {
        set
        {
            _isCanMove = value;
        }
    }

    [HideInInspector]
    public Rigidbody2D rb2d;
    public Vector2 VectorMove;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isLocalPlayer && !_isCanMove) return;

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

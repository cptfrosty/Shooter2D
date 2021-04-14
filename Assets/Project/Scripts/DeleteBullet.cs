using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeleteBullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerCombat : MonoBehaviour
{
    public Gun gunPlayer;
    public Transform spawnPos;
    private PlayerView playerView;
    [SerializeField] private GameObject bullet;

    public void Start()
    {
        playerView = GetComponent<PlayerView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        Debug.DrawRay(transform.position, playerView.mousePos, Color.red);
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, spawnPos.position, spawnPos.rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(spawnPos.up * gunPlayer.speedBullet, ForceMode2D.Impulse);
        
    }

    private void Damage(PlayerManager player)
    {
        player.DealDamage(gunPlayer.damage);
    }
}

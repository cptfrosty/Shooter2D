using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum TypeTeam
{
    CT, T
}

[RequireComponent(typeof(PlayerView))]
public class PlayerCombat : NetworkBehaviour
{
    public DataPlayerCombat dataPlayerCombat;

    public int hp = 100;
    public int armor = 100;
    public TypeTeam typeTeam;

    [Header("GUNS")]
    public Gun gunPistol;
    public Gun gunSilencer;
    public Gun gunAuthomatic;
    private Gun _gun;

    [Header("SFX/VFX")]
    public ParticleSystem effectShoot;
    public AudioSource audioGun;
    public AudioSource audioDamage;

    [Space]
    public Transform spawnPosBullet;
    
    private PlayerView playerView;

    [SerializeField] 
    private GameObject bullet;

    public override void OnStartClient()
    {
        base.OnStartLocalPlayer();
        playerView = GetComponent<PlayerView>();
        _gun = gunPistol;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }


    private void Shoot()
    { 
        CMDShoot(spawnPosBullet.position, spawnPosBullet.rotation, spawnPosBullet.up);
        PlayEffectShoot();
    }

    #region Shot
    [Command(requiresAuthority = false)]
    private void CMDShoot(Vector3 pos, Quaternion rot, Vector3 dir)
    {
        ServerShot(pos, rot, dir);
    }

    [Server]
    private void ServerShot(Vector3 pos, Quaternion rot, Vector3 dir)
    {
        GameObject newBullet = Instantiate(gunPistol.prefabBullet, pos, rot);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * gunPistol.speedBullet, ForceMode2D.Impulse);
        NetworkServer.Spawn(newBullet);
    }
    #endregion

    [Command(requiresAuthority = false)]
    private void Damage()
    {
        DealDamage(_gun.damage);
        audioDamage.PlayOneShot(dataPlayerCombat.get_damage);
    }

    //Получение урона
    [Server]
    public void DealDamage(int damage)
    {
        //TODO: РАССЧИТАТЬ УРОН АРМОРА И ЖИЗНЕЙ
        hp -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.collider.tag == "Bullet")
        {
            Damage();
        }
    }

    #region Effects
    private void PlayEffectShoot()
    {
        effectShoot.Play();
        audioGun.PlayOneShot(_gun.shot_pistol);
    }
    #endregion
}

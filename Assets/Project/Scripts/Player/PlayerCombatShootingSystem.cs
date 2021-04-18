using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCombatShootingSystem : NetworkBehaviour
{
    [Header("GUNS")]
    public Gun gunPistol;
    public Gun gunSilencer;
    public Gun gunAuthomatic;
    public Gun _gun;

    [Header("SFX/VFX")]
    public ParticleSystem effectShoot;
    public AudioSource audioGun;

    [Space]
    public Transform spawnPosBullet;

    public override void OnStartClient()
    {
        base.OnStartClient();
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

    #region Effects
    private void PlayEffectShoot()
    {
        effectShoot.Play();
        audioGun.PlayOneShot(_gun.shot_pistol);
    }
    #endregion
}

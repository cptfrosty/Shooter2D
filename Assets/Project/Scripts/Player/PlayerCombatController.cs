using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(HudHealth))]
[RequireComponent(typeof(PlayerArmor))]
[RequireComponent(typeof(HudArmor))]
[RequireComponent(typeof(PlayerCombatShootingSystem))]
[RequireComponent(typeof(PlayerViewSystem))]
[RequireComponent(typeof(PlayerMovementSystem))]
public class PlayerCombatController : NetworkBehaviour
{
    //Events
    public delegate void ChangeDeadHandler(bool isDead);
    public ChangeDeadHandler OnDead;

    //Data
    [SerializeField] private DataPlayerCombat dataPlayerCombat;
    public DataPlayerCombat SetDataPlayerCombat
    {
        set
        {
            dataPlayerCombat = value;
        }
    }

    //Components
    [SerializeField] [ReadOnly] private PlayerHealth health;
    [SerializeField] [ReadOnly] private HudHealth hudHealth;
    [SerializeField] [ReadOnly] private PlayerArmor armor;
    [SerializeField] [ReadOnly] private HudArmor hudArmor;
    [SerializeField]            private SpriteRenderer sprite;

    [SerializeField] [ReadOnly] private PlayerCombatShootingSystem shoot;
    [SerializeField] [ReadOnly] private PlayerViewSystem view;
    [SerializeField] [ReadOnly] private PlayerMovementSystem movement;

    //Varable
    [SerializeField] [ReadOnly] private bool isDead;
    [SerializeField] private AudioSource damageSound;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            SceneController.instance.AddClient(netIdentity);
        }
    }

    private void OnEnable()
    {
        health = GetComponent<PlayerHealth>();
        hudHealth = GetComponent<HudHealth>();
        armor = GetComponent<PlayerArmor>();
        hudArmor = GetComponent<HudArmor>();
        view = GetComponent<PlayerViewSystem>();
        shoot = GetComponent<PlayerCombatShootingSystem>();
        movement = GetComponent<PlayerMovementSystem>();

        //Events
        OnDead += StatusDead;
        health.ChangedHP += hudHealth.ChangeHealth;
        armor.ChangedArmor += hudArmor.ChangeArmor;

        StatusDead(true);
    }
    public void StatusDead(bool isDead)
    {
        if (!isDead)
        {
            CmdChoiseSpritePlayerSkinPistol();

            health.Value = 100;
            armor.Value = 100;

            shoot.enabled = true;
            view.enabled = true;
            movement.enabled = true;

            view.StartLook();
        }
        else
        {
            if (isLocalPlayer)
            {
                CmdChoiseSpritePlayerDead();
                CmdDead();
            }

            health.Value = 0;
            armor.Value = 0;

            shoot.enabled = false;
            view.enabled = false;
            movement.enabled = false;
        }
    }

    [Command (requiresAuthority = false)]
    public void CmdDead()
    {
        SceneController.instance.DeadPlayer();
    }

    #region Поменять скин на красный крест
    [Command(requiresAuthority = false)]
    public void CmdChoiseSpritePlayerDead()
    {
        RpcChoiseSpritePlayerDead();
    }
    [ClientRpc]
    public void RpcChoiseSpritePlayerDead()
    {
        sprite.sprite = dataPlayerCombat.dead_mark;
    }
    #endregion

    #region Поменять на скин с пистолетом
    [Command(requiresAuthority = false)]
    public void CmdChoiseSpritePlayerSkinPistol()
    {
        RpcChoiseSpritePlayerSkinPistol();
    }
    [ClientRpc]
    public void RpcChoiseSpritePlayerSkinPistol()
    {
        sprite.sprite = dataPlayerCombat.skin_pistol;
    }
    #endregion

    #region health and armor

    private void Damage(int damage)
    {
        CmdDealDamage(damage);
        damageSound.PlayOneShot(dataPlayerCombat.get_damage);
    }

    //Получение урона
    [Command(requiresAuthority = false)]
    public void CmdDealDamage(int damage)
    {
        PlayerArmor cArmor = GetComponent<PlayerArmor>();
        PlayerHealth cHealth = GetComponent<PlayerHealth>();

        //Если броня и жизни включены
        if (cArmor.enabled && cHealth.enabled)
        {
            //Если броня равняется 0
            if (cArmor.Value == 0)
            {
                //... то вычесть из жизней
                cHealth.Value -= damage;
            }
            else
            {
                //Если броня больше чем урон
                if (cArmor.Value > damage)
                {
                    //... то вычесть из брони
                    cArmor.Value -= damage;
                }
                else
                {
                    //Иначе расчитать остаток урона на жизни
                    int remains = damage - cArmor.Value;
                    cArmor.Value = 0;
                    cHealth.Value -= remains;
                }
            }
        }
        //Если жизни включены, то нанести урон
        else if (cHealth.enabled)
        {
            cHealth.Value -= damage;
        }

        //Если жизни включены
        if (cHealth.enabled)
        {
            //И они равны 0
            if (cHealth.Value == 0)
            {
                //То игрок умирает
                isDead = true;
                OnDead(isDead);
            }
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            StatusDead(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClient)
        {
            #region Если попала пуля
            if (collision.collider.tag == "Bullet")
            {
                if (!armor.enabled && !health.enabled) return;
                Damage(shoot._gun.damage);
                NetworkServer.Destroy(collision.gameObject);
            }
            #endregion
        }
    }
}

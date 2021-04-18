using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    public delegate void ChangedHPHandler(int hp);
    public event ChangedHPHandler ChangedHP;

    [SerializeField] [SyncVar] private int hp;

    public int Value
    {
        get 
        {
            return hp;
        }

        set
        {
            hp = value;
            if (hp < 0)
                hp = 0;
            ChangedHP(hp);
        }
    }
}

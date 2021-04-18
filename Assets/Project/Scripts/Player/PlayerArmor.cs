using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerArmor : NetworkBehaviour
{
    public delegate void ChangedArmorHandler(int armor);
    public event ChangedArmorHandler ChangedArmor;

    [SerializeField] private int armor;

    public int Value
    {
        get
        {
            return armor;
        }

        set
        {
            armor = value;
            ChangedArmor(armor);
        }
    }
}

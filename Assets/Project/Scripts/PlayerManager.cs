using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeTeam
{
    CT, T
}

public class PlayerManager : MonoBehaviour
{
    public int hp = 100;
    public int armor = 100;
    public TypeTeam typeTeam;

    //Получение урона
    public void DealDamage(int damage)
    {
        //TODO: РАССЧИТАТЬ УРОН АРМОРА И ЖИЗНЕЙ

        if (armor == 0)
        {
            hp -= damage;
        }
    }
}

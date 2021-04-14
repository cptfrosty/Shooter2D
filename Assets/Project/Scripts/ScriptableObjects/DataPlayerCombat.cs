using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CS2D/Data Player combat")]
public class DataPlayerCombat : ScriptableObject
{
    public Sprite skin_pistol;
    public Sprite skin_machine;
    public Sprite skin_silencer;
    public Sprite skin_reload;
    public Sprite skin_stand;

    public AudioClip get_damage;
}

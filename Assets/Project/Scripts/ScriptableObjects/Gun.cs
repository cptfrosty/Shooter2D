using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CS2D/New gun")]
public class Gun : ScriptableObject
{
    public string nameGun;
    public int countAmmo;
    public GameObject prefabBullet;
    public int damage;
    public float speedBullet;

    [Header("SFX")]
    public AudioClip shot_pistol;
    public AudioClip reload_pistol;
}

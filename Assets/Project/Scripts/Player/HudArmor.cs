using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HudArmor : NetworkBehaviour
{
    [SerializeField] private GameObject armor;

    //Subscribed to the event
    public void ChangeArmor(int value)
    {
        if (isLocalPlayer)
        {
            Change(value);
        }
    }

    [Command(requiresAuthority = false)]
    private void Change(int value)
    {
        float size = value / 100f;
        RpcChange(size);
    }

    [ClientRpc]
    private void RpcChange(float value)
    {
        Vector3 scale = new Vector3(value, 0.1f, 0);
        armor.transform.localScale = scale;
    }
}

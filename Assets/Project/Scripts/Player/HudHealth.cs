using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HudHealth : NetworkBehaviour
{
    [SerializeField] private GameObject health;

    ///Subscribed to the event
    public void ChangeHealth(int value)
    {
        if (isClient)
        {
            CmdChange(value);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdChange(int value)
    {
        float res = value / 100f;
        RpcChange(res);
    }

    [ClientRpc]
    private void RpcChange(float value)
    {
        Vector3 scale = new Vector3(value, 0.1f, 0);
        health.transform.localScale = scale;
    }
}

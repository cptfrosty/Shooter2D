using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(PlayerTeamController))]
public class PlayerManager : NetworkBehaviour
{
    public string namePlayer;

    private PlayerCombatController playerCombat;
    private PlayerTeamController playerTeam;

    public PlayerCombatController GetPlayerCombat { 
        get
        {
            return playerCombat;
        } 
    }

    public PlayerTeamController GetPlayerTeam
    {
        get
        {
            return playerTeam;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        playerCombat    = GetComponent<PlayerCombatController>();
        playerTeam      = GetComponent<PlayerTeamController>();
    }
}

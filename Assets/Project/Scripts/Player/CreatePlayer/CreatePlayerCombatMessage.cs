using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CreatePlayerCombatMessage : NetworkMessage
{
    public string namePlayer;
    public PlayerTeamController.TypeTeam team;
}

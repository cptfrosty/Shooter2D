using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CS2DNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreatePlayerCombatMessage>(OnCreatePlayerCombat);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        UIController.instance.ShowSelectTeam();

        CreatePlayerCombatMessage createMessage = new CreatePlayerCombatMessage()
        {
            namePlayer = "Player303",
            team = PlayerTeamController.TypeTeam.None
            
        };

        conn.Send(createMessage);
    }

    public void OnCreatePlayerCombat(NetworkConnection conn, CreatePlayerCombatMessage message)
    {
        GameObject gameObject = Instantiate(playerPrefab);

        //gameObject.GetComponent<PlayerTeamController>().team = message.team;
        gameObject.GetComponent<PlayerManager>().namePlayer = message.namePlayer;
        //gameObject.GetComponent<PlayerManager>().GetPlayerTeam.team = message.team;
        gameObject.transform.position = new Vector3(-777, -777, 0);
        //gameObject.GetComponent<PlayerManager>().GetPlayerCombat.StatusDead(true);
        NetworkServer.AddPlayerForConnection(conn, gameObject);
    }
}

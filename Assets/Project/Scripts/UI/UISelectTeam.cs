using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectTeam : MonoBehaviour
{
    private Coroutine messageDisplayTime;

    [Header("Team CT")]
    public Button selectTeamCT;
    [SerializeField] private Text countPlayersCT;

    [Header("Team CT")]
    public Button selectTeamT;
    [SerializeField] private Text countPlayersT;

    [Header("General")]
    [SerializeField] private Text message;
    [SerializeField] private Text allCountPlayers;

    bool isStartCoroutine = false;

    private void Start()
    {
        message.text = "";
    }

    public void PrintMessage(string message)
    {
        if(isStartCoroutine)
            StopCoroutine(messageDisplayTime);

        this.message.text = message;
        messageDisplayTime = StartCoroutine(MessageDisplayTime());
    }

    public void InfoAboutTeams(int countPlayers, int countPlayersCT, int countPlayersT, int maxPlayerInTeam)
    {
        this.countPlayersCT.text = $"{countPlayersCT}/{maxPlayerInTeam}";
        this.countPlayersT.text = $"{countPlayersT}/{maxPlayerInTeam}";
        this.allCountPlayers.text = $"На сервере {countPlayers}";
    }

    IEnumerator MessageDisplayTime()
    {
        isStartCoroutine = true;
        yield return new WaitForSeconds(1.5f);
        message.text = "";
        isStartCoroutine = false;
    }
}

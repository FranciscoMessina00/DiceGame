using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsVisualizer : MonoBehaviour
{
    private Player currentPlayer;
    private int indexPlayer;
    private List<Player> allPlayers;
    private GameObject playerNameObject;
    private GameObject pointsObj;
    private void Awake()
    {
        allPlayers = PlayerPoints.players;
        currentPlayer = PlayerPoints.CurrentPlayer();
        indexPlayer = PlayerPoints.currentPlayer;
        playerNameObject = GameObject.Find("Player name");
        pointsObj = GameObject.Find("Points");
    }
    // Start is called before the first frame update
    void Start()
    {
        viewPlayerInfo();
        GameObject lanciaDadiNomeObj = GameObject.Find("Lancia nome");
        TMP_Text lanciaDadiNome = lanciaDadiNomeObj.GetComponent<TMP_Text>();
        lanciaDadiNome.text = "Tocca a " + currentPlayer.PlayerName;
    }
    private string getPointsString(Player player)
    {
        string points = "";
        foreach(int point in player.points.Values)
        {
            points += point.ToString() + "\n";
        }
        points += player.TotalPoints().ToString();
        return points;
    }
    public void viewDifferentPlayer(int direction)
    {
        indexPlayer += direction;
        if (indexPlayer >= allPlayers.Count)
        {
            indexPlayer = 0;
        }
        if (indexPlayer < 0 )
        {
            indexPlayer = allPlayers.Count - 1;
        }
        viewPlayerInfo();
    }
    public void viewPlayerInfo()
    {
        TMP_Text playerName = playerNameObject.GetComponent<TMP_Text>();
        TMP_Text points = pointsObj.GetComponent<TMP_Text>();
        playerName.text = allPlayers[indexPlayer].PlayerName;
        string pointsString = getPointsString(allPlayers[indexPlayer]);
        points.text = pointsString;
    }
    public void Lancia()
    {
        SceneManager.LoadScene("LancioDadi");
    }
}

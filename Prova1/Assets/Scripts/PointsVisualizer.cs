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
        if ( PointsManager.inPunteggio)
        {
            lanciaDadiNome.text = "Vai a scegliere il punteggio";
        }
        else
        {
            if (PlayerPoints.tiriTotali >= 11 * PlayerPoints.players.Count)
            {
                lanciaDadiNome.text = "Dichiara vincitore!";
            }
            else
            {
                lanciaDadiNome.text = "Tocca a " + currentPlayer.PlayerName + ". Clicca qui!";
            }
        }
        
        
    }
    private string getPointsString(Player player)
    {
        string points = "";
        foreach(int point in player.points.Values)
        {
            if (point == -1)
            {
                points += "-\n";
            }
            else
            {
                points += point.ToString() + "\n";
            }
            
        }
        points += player.TotalPoints().ToString();
        return points;
    }
    public void viewDifferentPlayer(int direction)
    {
        indexPlayer += direction;
        /*if (indexPlayer >= allPlayers.Count)
        {
            indexPlayer = 0;
        }
        if (indexPlayer < 0 )
        {
            indexPlayer = allPlayers.Count - 1;
        }*/
        
        // lo traslo della lunghezza della lista per non avere problemi con il modulo
        // di numeri negativi se mi sposto a sinistra con le frecce
        indexPlayer = Mathf.Abs( (indexPlayer + allPlayers.Count) % allPlayers.Count);
        viewPlayerInfo();
    }
    private void viewPlayerInfo()
    {
        TMP_Text playerName = playerNameObject.GetComponent<TMP_Text>();
        TMP_Text points = pointsObj.GetComponent<TMP_Text>();
        playerName.text = allPlayers[indexPlayer].PlayerName;
        points.text = getPointsString(allPlayers[indexPlayer]);
    }
    public void Lancia()
    {
        if (PointsManager.inPunteggio)
        {
            SceneManager.LoadScene("SceltaPunteggio");
        }
        else
        {
            if (PlayerPoints.tiriTotali >= 11 * PlayerPoints.players.Count)
            {
                SceneManager.LoadScene("Vincitore");
            }
            else
            {
                SceneManager.LoadScene("LancioDadi");
            }
        }
    }
}

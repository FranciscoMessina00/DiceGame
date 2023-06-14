using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void Gioca()
    {
        SceneManager.LoadScene("LancioDadi");
        List<string> names = new List<string>
        {
            "Giocatore 1",
            "Giocatore 2"
        };
        PlayerPoints.CreatePlayers(names);
        GameController.contatore = 3;
    }
}

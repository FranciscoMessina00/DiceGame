using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void Gioca()
    {
        /*List<string> names = new List<string>
        {
            "Frisco",
            "Filippo"
        };
        PlayerPoints.CreatePlayers(names);
        GameController.contatore = 3;*/
        SceneManager.LoadScene("IniziaPartita");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("soundtrack");
    }
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

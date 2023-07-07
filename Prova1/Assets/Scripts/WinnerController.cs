using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<KeyValuePair<string, int>> classifica = Classifica();
        // set testo per dichiarare vincitore
        GameObject textVincitore = GameObject.Find("Vincitore");
        TMP_Text meshVincitore = textVincitore.GetComponent<TMP_Text>();
        meshVincitore.SetText("Il vincitore è...\n" + classifica[0].Key);
        // set testo per mostrare la classiica generale
        GameObject textClassifica = GameObject.Find("Classifica");
        TMP_Text meshClassifica = textClassifica.GetComponent<TMP_Text>();
        int i = 1;
        string classificaTesto = "";
        foreach(KeyValuePair<string, int> player in classifica)
        {
            classificaTesto += i.ToString() + ". " + player.Key + " - " + player.Value + "\n";
            i++;
        }
        meshClassifica.SetText(classificaTesto);
    }
    private List<KeyValuePair<string, int>> Classifica()
    {
        List<Player> players = PlayerPoints.players;
        Dictionary<string, int> classifica = new Dictionary<string, int>();
        foreach (Player player in players)
        {
            classifica.Add(player.PlayerName, player.TotalPoints());
        }
        List<KeyValuePair<string, int>> sortedClassifica = classifica.OrderBy(x => x.Value).ToList();
        sortedClassifica.Reverse();
        return sortedClassifica;
    }
    public void MenuPrincipale()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

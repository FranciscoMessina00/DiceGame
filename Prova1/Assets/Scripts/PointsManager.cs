using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.ParticleSystem;

public class PointsManager : MonoBehaviour
{
    public GameObject text;
    public int[] conteggioNumeri;
    public int[] dadiUsciti;
    public GameObject bottonManagerObj;
    public ButtonsManagerPoints bmp;
    Player currentPlayer;
    public bool sacrifica;
    public static bool inPunteggio = false;
    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = PlayerPoints.CurrentPlayer();
        bmp = bottonManagerObj.GetComponent<ButtonsManagerPoints>();
        conteggioNumeri = new int[] { 0, 0, 0, 0, 0, 0 };
        dadiUsciti = new List<int>(GameController.numeri).ToArray();
        System.Array.Sort(dadiUsciti);
        //dadiUsciti = new int[] { 6, 6, 6, 6, 6 };
        string testo = GenerateText(dadiUsciti);
        TMP_Text mesh = text.GetComponent<TMP_Text>();
        mesh.SetText("I dadi usciti sono:\n"+testo+"\nCosa scegli?");
        //popola conteggioNumeri
        foreach (int numero in dadiUsciti)
        {
            PopolaConteggioNumeri(numero);
        }
        CalcolaGiochi();
        sacrifica = false;
        inPunteggio = false;
        GameObject pulsanteSacrifica = GameObject.Find("Sacrifica");
        pulsanteSacrifica.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    private string GenerateText(int[] numeri)
    {
        string stampa = "";
        foreach (int numero in numeri)
        {
            stampa = stampa + numero.ToString() + ", ";
        }
        return stampa;
    }
    private void CalcolaGiochi()
    {
        //disabilitare tutti i pulsanti
        for(int i = 0; i < 11; i++)
        {
            bmp.DisableButtons(i);
        }

        //check giocate dei numeri
        for (int i=0; i<conteggioNumeri.Length; i++)
        {
            int numero = i + 1;
            if (GiocataNumeri(conteggioNumeri[i]) && currentPlayer.points[numero.ToString()] == 0)
            {
                //abilita giocata del numero
                bmp.SetButtons(i);
                //Debug.Log(numero + " c'è");
            }
        }
        //check giocate scala, full, poker e generala
        if ( Scala() && currentPlayer.points["Scala"] == 0 )
        {
            //abilita scala
            bmp.SetButtons(6);
            //Debug.Log("Scala");
        }
        if ( Full() && currentPlayer.points["Full"] == 0 )
        {
            //abilita full
            bmp.SetButtons(7);
            //Debug.Log("Full");
        }
        if ( Poker() && currentPlayer.points["Poker"] == 0 )
        {
            //abilita poker
            bmp.SetButtons(8);
            //Debug.Log("Poker");
        }
        if ( Generala() && (currentPlayer.points["Doppia"] == 0 || currentPlayer.points["Generala"] == 0))
        {
            if (currentPlayer.GeneralaFatta)
            {
                bmp.SetButtons(10);
                //Debug.Log("Doppia");
            }
            else
            {
                // se ho cliccato sacrifica, sono qui perché non ho fatto la generala
                if (sacrifica)
                {
                    if (currentPlayer.points["Doppia"] == 0)
                    {
                        bmp.SetButtons(10);
                        //Debug.Log("Doppia");
                    }
                    else
                    {
                        bmp.SetButtons(9);
                        //Debug.Log("Generala");
                    }
                }
                else
                {
                    bmp.SetButtons(9);
                    //
                    //
                    //
                    //
                    //
                    //
                    //Debug.Log("Generala");
                }
                
            }
            
        }
    }
    private void PopolaConteggioNumeri(int numero)
    {
        switch (numero)
        {
            case 1:
                conteggioNumeri[0]++;
                break;
            case 2:
                conteggioNumeri[1]++;
                break;
            case 3:
                conteggioNumeri[2]++;
                break;
            case 4:
                conteggioNumeri[3]++;
                break;
            case 5:
                conteggioNumeri[4]++;
                break;
            case 6:
                conteggioNumeri[5]++;
                break;
        }
    }
    private bool GiocataNumeri(int uscita)
    {
        if (uscita != 0 || sacrifica)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool Scala()
    {
        if (sacrifica || Enumerable.SequenceEqual(conteggioNumeri, new int[] { 1, 1, 1, 1, 1, 0 })  || Enumerable.SequenceEqual(conteggioNumeri, new int[] { 0, 1, 1, 1, 1, 1 }))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    private bool Full()
    {
        if ( sacrifica || (conteggioNumeri.Contains(2) && conteggioNumeri.Contains(3)) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool Poker()
    {
        if ( sacrifica || conteggioNumeri.Contains(4) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool Generala()
    {
        if ( conteggioNumeri.Contains(5) || sacrifica )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Segn i punti per i numeri
    public void SetPointsNumber( int facciaDado )
    {
        int index = facciaDado - 1;
        int puntiMoltiplicati = facciaDado * conteggioNumeri[index];
        //manda puntiMoltiplicati e puntoSingolo in formato stringa per segnare punti
        if (sacrifica)
        {
            PlayerPoints.SetPointsToPlayer(facciaDado.ToString());
            audioManager.Play("sacrifica");
        }
        else
        {
            PlayerPoints.SetPointsToPlayer(facciaDado.ToString(), puntiMoltiplicati);
            audioManager.Play(conteggioNumeri[index].ToString());
        }
        
        VaiASchermataPunti();
    }
    public void SetPointsScala()
    {
        int points = 20;
        points = CheckFirstTry(points);
        if (sacrifica)
        {
            PlayerPoints.SetPointsToPlayer("Scala");
            audioManager.Play("sacrifica");
        }
        else
        {
            PlayerPoints.SetPointsToPlayer("Scala", points);
            audioManager.Play("scala");
        }
        VaiASchermataPunti();
    }
    public void SetPointsFull()
    {
        int points = 30;
        points = CheckFirstTry(points);
        if (sacrifica)
        {
            PlayerPoints.SetPointsToPlayer("Full");
            audioManager.Play("sacrifica");
        }
        else
        {
            PlayerPoints.SetPointsToPlayer("Full", points);
            audioManager.Play("full");
        }
        VaiASchermataPunti();
    }
    public void SetPointsPoker()
    {
        int points = 40;
        points = CheckFirstTry(points);
        if (sacrifica)
        {
            PlayerPoints.SetPointsToPlayer("Poker");
            audioManager.Play("sacrifica");
        }
        else
        {
            PlayerPoints.SetPointsToPlayer("Poker", points);
            audioManager.Play("poker");
        }
        VaiASchermataPunti();
    }
    public void SetPointsGenerala()
    {
        int points = 50;
        points = CheckFirstTry(points);
        if (currentPlayer.GeneralaFatta)
        {
            if (sacrifica)
            {
                PlayerPoints.SetPointsToPlayer("Doppia");
                audioManager.Play("sacrifica");
            }
            else
            {
                PlayerPoints.SetPointsToPlayer("Doppia", points);
                audioManager.Play("doppia");
            }
        }
        else
        {
            if (sacrifica)
            {
                if (currentPlayer.points["Doppia"] == 0)
                {
                    PlayerPoints.SetPointsToPlayer("Doppia");
                }
                else
                {
                    PlayerPoints.SetPointsToPlayer("Generala");
                }
                audioManager.Play("sacrifica");
            }
            else
            {
                PlayerPoints.SetPointsToPlayer("Generala", points);
                currentPlayer.GeneralaFatta = true;
                audioManager.Play("generala");
            }
        }
        VaiASchermataPunti();
    }
    public void VaiASchermataPunti()
    {
        GameController.midTurno = false;
        GameController.contatore = 3;
        PlayerPoints.NextPlayer();
        print(currentPlayer);
        SceneManager.LoadScene("Punteggio");
        // vai alla schermata dei punti oppure rilancia i dadi per ora
    }
    public void MidTurnoSchermataPunti()
    {
        inPunteggio = true;
        SceneManager.LoadScene("Punteggio");
    }
    public void IndietroSchermata()
    {
        SceneManager.LoadScene("LancioDadi");
    }
    public void SwitchPulsantiSacrifica()
    {
        sacrifica = !sacrifica;
        if (sacrifica)
        {
            GameObject pulsanteSacrifica = GameObject.Find("Sacrifica");
            pulsanteSacrifica.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            GameObject pulsanteSacrifica = GameObject.Find("Sacrifica");
            pulsanteSacrifica.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        CalcolaGiochi();
    }
    /*public void Sacrifica(string gioco)
    {
        PlayerPoints.SetPointsToPlayer(gioco);
    }*/
    private int CheckFirstTry(int points)
    {
        if (GameController.contatore == 2)
        {
            points += 5;
        }
        return points;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = PlayerPoints.CurrentPlayer();
        bmp = bottonManagerObj.GetComponent<ButtonsManagerPoints>();
        conteggioNumeri = new int[] { 0, 0, 0, 0, 0, 0 };
        dadiUsciti = GameController.numeri;
        //dadiUsciti = new int[] { 6, 6, 6, 6, 6 };
        string testo = GenerateText(dadiUsciti);
        TMP_Text mesh = text.GetComponent<TMP_Text>();
        mesh.SetText("I dadi usciti sono:\n"+testo+"\nCosa scegli?");
        CalcolaGiochi();
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
        //popola conteggioNumeri
        foreach(int numero in dadiUsciti)
        {
            PopolaConteggioNumeri(numero);
        }
        //check giocate dei numeri
        for (int i=0; i<conteggioNumeri.Length; i++)
        {
            int numero = i + 1;
            if (GiocataNumeri(conteggioNumeri[i]) && currentPlayer.points[numero.ToString()] == 0)
            {
                //abilita giocata del numero
                bmp.SetButtons(i);
                Debug.Log(numero + " c'è");
            }
        }
        //check giocate scala, full, poker e generala
        if ( Scala() && currentPlayer.points["Scala"] == 0 )
        {
            //abilita scala
            bmp.SetButtons(6);
            Debug.Log("Scala");
        }
        else if ( Full() && currentPlayer.points["Full"] == 0 )
        {
            //abilita full
            bmp.SetButtons(7);
            Debug.Log("Full");
        }
        else if ( Poker() && currentPlayer.points["Poker"] == 0 )
        {
            //abilita poker
            bmp.SetButtons(8);
            Debug.Log("Poker");
        }
        else if ( Generala() )
        {
            if (currentPlayer.GeneralaFatta)
            {
                bmp.SetButtons(10);
                Debug.Log("Doppia");
            }
            else
            {
                bmp.SetButtons(9);
                Debug.Log("Generala");
            }
            // if(generalaFatta) {abilita doppia generala}
            // else{abilita generala}
            
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
        if (uscita != 0)
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
        if (Enumerable.SequenceEqual(conteggioNumeri, new int[] { 1, 1, 1, 1, 1, 0 })  || Enumerable.SequenceEqual(conteggioNumeri, new int[] { 0, 1, 1, 1, 1, 1 }))
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
        if (conteggioNumeri.Contains(2) && conteggioNumeri.Contains(3))
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
        if (conteggioNumeri.Contains(4))
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
        if (conteggioNumeri.Contains(5))
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
        PlayerPoints.SetPointsToPlayer(facciaDado.ToString(), puntiMoltiplicati);
        VaiASchermataPunti();
    }
    public void SetPointsScala()
    {
        int points = 20;
        points = CheckFirstTry(points);
        PlayerPoints.SetPointsToPlayer("Scala", points);
        VaiASchermataPunti();
    }
    public void SetPointsFull()
    {
        int points = 30;
        points = CheckFirstTry(points);
        PlayerPoints.SetPointsToPlayer("Full", points);
        VaiASchermataPunti();
    }
    public void SetPointsPoker()
    {
        int points = 40;
        points = CheckFirstTry(points);
        PlayerPoints.SetPointsToPlayer("Poker", points);
        VaiASchermataPunti();
    }
    public void SetPointsGenerala()
    {
        int points = 50;
        points = CheckFirstTry(points);
        if (currentPlayer.GeneralaFatta)
        {
            PlayerPoints.SetPointsToPlayer("Doppia", points);
        }
        else
        {
            PlayerPoints.SetPointsToPlayer("Generala", points);
            currentPlayer.GeneralaFatta = true;
        }
        VaiASchermataPunti();
    }
    public void VaiASchermataPunti()
    {
        GameController.contatore = 3;
        PlayerPoints.NextPlayer();
        print(currentPlayer);
        SceneManager.LoadScene("Punteggio");
        // vai alla schermata dei punti oppure rilancia i dadi per ora
    }
    public void VaiASchermataSacrifica()
    {
        // carica la stessa schermata dei pulsanti ma tutti abilitati tranne
        // quelli che hanno un punteggio diverso da 0
    }
    public void Sacrifica(string gioco)
    {
        PlayerPoints.SetPointsToPlayer(gioco);
    }
    private int CheckFirstTry(int points)
    {
        if (GameController.contatore == 2)
        {
            points += 5;
        }
        return points;
    }
}

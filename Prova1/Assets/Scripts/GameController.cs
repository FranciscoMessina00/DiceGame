using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject[] dadiArray;
    public static int contatore = 3;
    private SpriteRenderer m_renderer;
    private Color colore;
    [SerializeField] private bool dadiAbilitati = true;
    [SerializeField] private GameObject inputController;
    [SerializeField] private GameObject lanciaBotton;
    [SerializeField] private GameObject avantiBotton;
    [SerializeField] private GameObject testo;
    [SerializeField] private GameObject tentativiRimasti;
    [SerializeField] private GameObject vediPunteggioButton;
    private bool avanti;
    public static int[] numeri = new int[] { 1, 1, 1, 1, 1 };
    // public static int[] tempDiceFaces;
    [SerializeField] public static bool midTurno = false;
    public PlayerInput inputPlayer;
    UnityEngine.UI.Button bottoneLanciare;
    public AudioManager audioManager;
    private void Awake()
    {
        inputPlayer = new PlayerInput();
        inputController.GetComponent<DiceSelected>().enabled = false;
        inputPlayer.Disable();
        MostraBottoni(false);
        // vediPunteggioButton = GameObject.Find("Vedi punteggio");
        avanti = false;
        bottoneLanciare = lanciaBotton.GetComponent<UnityEngine.UI.Button>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (contatore == 3)
        {
            midTurno = false;
        }
        else
        {
            midTurno = true;
        }
        TMP_Text mesh = tentativiRimasti.GetComponent<TMP_Text>();
        mesh.SetText( "Tiri rimasti: " + contatore );
        mesh = testo.GetComponent<TMP_Text>();
        mesh.SetText(PlayerPoints.CurrentPlayer().PlayerName + ", tieni premuto per lanciare i dadi");
        // Debug.Log(PlayerPoints.CurrentPlayer().PlayerName);
        
        if (midTurno)
        {
            EnableAllDice(false);
            foreach (GameObject dado in dadiArray)
            {
                dado.GetComponent<DiceRandomizer>().enabled = false;
                Debug.Log("enable false");
            }
            DisableDice();
            if (contatore != 0)
            {
                inputPlayer.Enable();
                inputController.GetComponent<DiceSelected>().enabled = true;
            }
            // else è uguale a 0
            else
            {
                inputPlayer.Disable();
                inputController.GetComponent<DiceSelected>().enabled = false;
            }
            RecoverFaces();
        }
        else
        {
            EnableAllDice(true);
        }
        
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dadiAbilitati)
        {
            audioManager.Play("roll");
        }
        if (Input.GetMouseButtonUp(0) && dadiAbilitati)
        {
            Randomize();
            audioManager.Stop("roll");
            audioManager.Play("throw");
            TogliTentativo();
            SaveNumbersArray();
            DisableDice();
            if (contatore != 0)
            {
                inputPlayer.Enable();
                inputController.GetComponent<DiceSelected>().enabled = true;
            }
            else
            {
                inputPlayer.Disable();
                inputController.GetComponent<DiceSelected>().enabled = false;
            }
        }
        // quando clicco lancia, se non c'è questo if sotto mi lancia i dadi automaticamente perchè
        // mi prende il fatto che alzo il dito e nel frame successivo mi lancia i dadi questo serve come
        // separatore tra la pressione del pulsante e l'abilitazione del lancio di dadi
        if (avanti)
        {
            dadiAbilitati = true;
            avanti = false;
        }
    }

    public void DisableDice()
    {
        MostraBottoni(true);
        bottoneLanciare.interactable = false;
        foreach (GameObject dado in dadiArray)
        {
            dado.GetComponent<DiceRandomizer>().StopCo();
            Debug.Log("STOPPED COROUTINE");
            m_renderer = dado.GetComponent<SpriteRenderer>();
            colore = m_renderer.color;
            colore.a = contatore != 0 ? .5f : 1f;
            dado.GetComponent<SpriteRenderer>().color = colore;
            dado.GetComponent<Dice>().IsSelected(false);
        }
        dadiAbilitati = false;
    }
    public void Randomize()
    {
        foreach (GameObject dadoObj in dadiArray)
        {
            DiceRandomizer randomizer = dadoObj.GetComponent<DiceRandomizer>();
            Dice dado = dadoObj.GetComponent<Dice>();
            if (dado.selected)
            {
                randomizer.ChangeSprite(dado.RandomFace());
            }
        }
    }
    public void EnableDice()
    {
        MostraBottoni(false);
        //bottoneLanciare.interactable = true;
        Dice dado;
        foreach (GameObject dadoObj in dadiArray)
        {
            dado = dadoObj.GetComponent<Dice>();
            if (dado.selected)
            {
                dadoObj.GetComponent<DiceRandomizer>().enabled = true;
                dadoObj.GetComponent<DiceRandomizer>().StartCo();
            }
        }
        inputPlayer.Disable();
        inputController.GetComponent<DiceSelected>().enabled = false;
        avanti = true;
    }
    public void SaveNumbersArray()
    {
        Dice dado;
        for (int i = 0; i < dadiArray.Length; i++)
        {
            dado = dadiArray[i].GetComponent<Dice>();
            numeri[i] = dado.face;
        }
    }
    public void NextScene()
    {
        SceneManager.LoadScene("SceltaPunteggio");
    }
    public void TogliTentativo()
    {
        contatore--;
        TMP_Text mesh = tentativiRimasti.GetComponent<TMP_Text>();
        mesh.SetText("Tiri rimasti: " + contatore);
        midTurno = true;
    }
    /*public void ControlloBottoneLancia()
    {
        bool abilitaLancia = false;
        foreach (GameObject dadoObj in dadiArray)
        {
            Dice dado = dadoObj.GetComponent<Dice>();
            if (dado.selected) { bottoneLanciare.interactable = true; }
        }
        if (abilitaLancia)
        {
            bottoneLanciare.interactable = true;
        }
        else
        {
            bottoneLanciare.interactable = false;
        }
    }*/
    public void MostraBottoni(bool mostra)
    {
        testo.SetActive(!mostra);
        lanciaBotton.SetActive(mostra);
        avantiBotton.SetActive(mostra);
        vediPunteggioButton.SetActive(mostra);
    }
    public void VediPunteggio()
    {
        SceneManager.LoadScene("Punteggio");
    }
    private void RecoverFaces()
    {
        DiceRandomizer diceRandomizer;
        Dice dadoFaccia;
        string dadoName;
        for (int i = 1; i <= 5; i++)
        {
            // Debug.Log(numeri[i - 1]);
            dadoName = "Dice_" + i.ToString();
            GameObject dado = GameObject.Find(dadoName);
            diceRandomizer = dado.GetComponent<DiceRandomizer>();
            diceRandomizer.ChangeSprite(numeri[i - 1] - 1);
            dadoFaccia = dado.GetComponent<Dice>();
            dadoFaccia.SetParam(numeri[i - 1] - 1);
        }
    }
    public void EnableAllDice(bool enable)
    {
        foreach (GameObject dadoObj in dadiArray)
        {
            Dice dado = dadoObj.GetComponent<Dice>();
            dado.IsSelected(enable);
        }
    }
}
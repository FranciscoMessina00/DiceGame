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
        lanciaBotton.SetActive(false);
        avantiBotton.SetActive(false);
        vediPunteggioButton = GameObject.Find("Vedi punteggio");
        vediPunteggioButton.SetActive(false);
        testo.SetActive(true);
        avanti = false;
        bottoneLanciare = lanciaBotton.GetComponent<UnityEngine.UI.Button>();
        if (midTurno)
        {
            DisableDice();
            if (contatore != 0)
            {
                inputPlayer.Enable();
                inputController.GetComponent<DiceSelected>().enabled = true;
            }
            if (contatore == 0)
            {
                inputPlayer.Disable();
                inputController.GetComponent<DiceSelected>().enabled = false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text mesh = tentativiRimasti.GetComponent<TMP_Text>();
        mesh.SetText( "Tiri rimasti: " + contatore );
        mesh = testo.GetComponent<TMP_Text>();
        mesh.SetText(PlayerPoints.CurrentPlayer().PlayerName + ", tieni premuto per lanciare i dadi");
        Debug.Log(PlayerPoints.CurrentPlayer().PlayerName);
        RecoverFaces();
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
            if (contatore == 0)
            {
                inputPlayer.Disable();
                inputController.GetComponent<DiceSelected>().enabled = false;
            }
        }
        if (avanti)
        {
            dadiAbilitati = true;
            avanti = false;
        }
        if (!dadiAbilitati)
        {
            ControlloBottoneLancia();
        }
    }

    public void DisableDice()
    {
        lanciaBotton.SetActive(true);
        avantiBotton.SetActive(true);
        vediPunteggioButton.SetActive(true);
        testo.SetActive(false);
        bottoneLanciare.interactable = false;
        for (int i = 0; i < dadiArray.Length; i++)
        {
            dadiArray[i].GetComponent<DiceRandomizer>().enabled = false;
            if (contatore != 0)
            {
                m_renderer = dadiArray[i].GetComponent<SpriteRenderer>();
                colore = m_renderer.color;
                colore.a = .5f;
                dadiArray[i].GetComponent<SpriteRenderer>().color = colore;
                dadiArray[i].GetComponent<Dice>().Deselected();
            }
            else
            {
                m_renderer = dadiArray[i].GetComponent<SpriteRenderer>();
                colore = m_renderer.color;
                colore.a = 1f;
                dadiArray[i].GetComponent<SpriteRenderer>().color = colore;
                dadiArray[i].GetComponent<Dice>().Deselected();
            }
        }
        dadiAbilitati = false;
    }
    public void Randomize()
    {
        foreach (GameObject dadoObj in dadiArray)
        {
            DiceRandomizer randomizer = dadoObj.GetComponent<DiceRandomizer>();
            if (randomizer.enabled)
            {
                Dice dado = dadoObj.GetComponent<Dice>();
                randomizer.ChangeSprite(dado.RandomFace());
            }
        }
    }
    public void EnableDice()
    {
        testo.SetActive(true);
        lanciaBotton.SetActive(false);
        avantiBotton.SetActive(false);
        vediPunteggioButton.SetActive(false);
        //bottoneLanciare.interactable = true;
        Dice dado;
        for (int i = 0; i < dadiArray.Length; i++)
        {
            dado = dadiArray[i].GetComponent<Dice>();
            if (dado.selected)
            {
                dadiArray[i].GetComponent<DiceRandomizer>().enabled = true;
            }
        }
        inputPlayer.Disable();
        inputController.GetComponent<DiceSelected>().enabled = false;
        avanti = true;
    }
    public void SaveNumbersArray()
    {
        for (int i = 0; i < dadiArray.Length; i++)
        {
            Dice dado = dadiArray[i].GetComponent<Dice>();
            numeri[i] = dado.face;
        }
    }
    public void NextScene()
    {
        if (contatore != 0)
        {
            midTurno = true;
        }
        SceneManager.LoadScene("SceltaPunteggio");
    }
    public void TogliTentativo()
    {
        contatore--;
        TMP_Text mesh = tentativiRimasti.GetComponent<TMP_Text>();
        mesh.SetText("Tiri rimasti: " + contatore);
    }
    public void ControlloBottoneLancia()
    {
        bool abilitaLancia = false;
        foreach (GameObject dadoObj in dadiArray)
        {
            Dice dado = dadoObj.GetComponent<Dice>();
            if (dado.selected) { abilitaLancia = true; }
        }
        if (abilitaLancia)
        {
            bottoneLanciare.interactable = true;
        }
        else
        {
            bottoneLanciare.interactable = false;
        }
    }
    public void VediPunteggio()
    {
        midTurno = true;
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log(numeri[i - 1]);
        }
        SceneManager.LoadScene("Punteggio");
    }
    private void RecoverFaces()
    {
        if (midTurno)
        {
            DiceRandomizer diceRandomizer;
            Dice dadoFaccia;
            midTurno = false;
            string dadoName;
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log(numeri[i - 1]);
                dadoName = "Dice_" + i.ToString();
                GameObject dado = GameObject.Find(dadoName);
                diceRandomizer = dado.GetComponent<DiceRandomizer>();
                diceRandomizer.ChangeSprite(numeri[i - 1] - 1);
                dadoFaccia = dado.GetComponent<Dice>();
                dadoFaccia.SetParam(numeri[i - 1] - 1);
            }
        }
    }
}
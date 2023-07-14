using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class NamePlayerController : MonoBehaviour
{
    public GameObject selectorObj;
    public GameObject inputBase;
    public GameObject[] inputs;
    public List<string> names;
    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        selectorObj = GameObject.Find("SelezionaGiocatori");
        SetNumberOfPlayers();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    public void SetNumberOfPlayers()
    {
        TMP_Dropdown drop = selectorObj.GetComponent<TMP_Dropdown>(); 
        int numberOfPlayers = drop.value + 1;
        // Debug.Log(numberOfPlayers);
        CreateListNamesDefault(numberOfPlayers);
        InstantiateInputs(numberOfPlayers);
    }
    public void InstantiateInputs(int n)
    {
        if (inputs != null && inputs.Length != 0)
        {
            foreach (GameObject input in inputs)
            {
                Destroy(input);
                Debug.Log("distrutto");
            }
        }
        inputs = new GameObject[n];
        CreateInputs(n);
    }
    private void CreateInputs(int n)
    {
        for (int i = 0; i < n; i++)
        {
            inputs[i] = Instantiate(inputBase, GameObject.Find("Canvas").transform);
            inputs[i].SetActive(true);
            inputs[i].transform.localPosition = new Vector3(0, -i * 150, 0);
            inputs[i].name = "Player " + (i + 1);
            foreach (Transform transform in inputs[i].transform)
            {
                foreach (Transform children in transform)
                {
                    Debug.Log(children.name);
                }
            }
            GameObject child = inputs[i].transform.Find("Text Area").transform.Find("Placeholder").gameObject;
            TMP_Text mesh = child.GetComponent<TMP_Text>();
            mesh.SetText("Player " + (i + 1));
        }
    }
    private void CreateListNamesDefault(int n)
    {
        names = new List<string>();
        for (int i = 1; i <= n; i++)
        {
            names.Add("PL" + i);
        }
    }
    public void NewGame()
    {
        for(int i = 0; i < names.Count; i++)
        {
            TMP_InputField field = inputs[i].GetComponent<TMP_InputField>();
            if (field.text != null && field.text.Length != 0)
            {
                names[i] = field.text.ToUpper();
            }
            else
            {
                names[i] = "PL" + (i + 1) ;
            }
        }
        PlayerPoints.CreatePlayers(names);
        GameController.contatore = 3;
        audioManager.Stop("soundtrack");
        SceneManager.LoadScene("Punteggio");
    }
    public void Indietro()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

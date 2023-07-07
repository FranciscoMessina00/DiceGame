using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManagerPoints : MonoBehaviour
{
    public GameObject[] bottoni;
    private void Awake()
    {
        Button button;
        for (int i = 0; i < 11; i++)
        {
            button = bottoni[i].GetComponent<Button>();
            button.interactable = false;
        }
        button = bottoni[12].GetComponent<Button>();
        button.interactable = GameController.midTurno;
    }
    public void SetButtons(int index)
    {
        GameObject bottoneIesimo = bottoni[index];
        Button button = bottoneIesimo.GetComponent<Button>();
        button.interactable = true;
    }
    public void DisableButtons(int index)
    {
        GameObject bottoneIesimo = bottoni[index];
        Button button = bottoneIesimo.GetComponent<Button>();
        button.interactable = false;
    }
}

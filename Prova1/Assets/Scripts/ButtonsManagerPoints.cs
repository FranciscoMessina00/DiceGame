using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManagerPoints : MonoBehaviour
{
    public GameObject[] bottoni;
    private void Awake()
    {
        for (int i = 0; i < bottoni.Length - 1; i++)
        {
            Button button = bottoni[i].GetComponent<Button>();
            button.interactable = false;
        }
    }
    public void SetButtons(int index)
    {
        GameObject bottoneIesimo = bottoni[index];
        Button button = bottoneIesimo.GetComponent<Button>();
        button.interactable = true;
    }
}

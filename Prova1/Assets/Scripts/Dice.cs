using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int face = 1;
    public string nameImage = "Dadi_1";
    private readonly System.Random rand = new System.Random();
    public bool selected = false;
    public int RandomFace()
    {
        int setFace = rand.Next(0, 6);
        SetParam(setFace);
        return setFace;
    }
    private void SetParam(int setFace)
    {
        this.face = setFace + 1;
        this.nameImage = "Dice_" + setFace.ToString();
    }
    public void Selected()
    {
        selected = true;
    }
    public void Deselected()
    {
        selected = false;
    }
}
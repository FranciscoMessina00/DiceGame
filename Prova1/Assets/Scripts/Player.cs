using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}

public class Player
{
    private static int playerNumber = 0;
    private int _idPlayer;
    public int IdPlayer
    {
        get => _idPlayer;
        init => _idPlayer = playerNumber;
    }
    private string _playerName;
    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }
    private bool _isCurrent;
    public bool IsCurrent
    {
        get => _isCurrent;
        set => _isCurrent = value;
    }
    private bool _generalaFatta;
    public bool GeneralaFatta
    {
        get => _generalaFatta;
        set => _generalaFatta = value;
    }
    public Dictionary<string, int> points = new Dictionary<string, int>()
    {
        {"1", 0},
        {"2", 0},
        {"3", 0},
        {"4", 0},
        {"5", 0},
        {"6", 0},
        {"Scala", 0},
        {"Full", 0},
        {"Poker", 0},
        {"Generala", 0},
        {"Doppia", 0}
    };
    public Player(string name)
    {
        this.IdPlayer = playerNumber;
        this.PlayerName = name;
        this.GeneralaFatta = false;
        this.IsCurrent = false;
        playerNumber++;
    }
    public override string ToString()
    {
        string printValue = this.PlayerName + "\n";
        foreach (KeyValuePair<string, int> kvp in points)
        {
            printValue += "Gioco = " + kvp.Key + ", Punti = " + kvp.Value + "\n";
        }
        return printValue;
    }
    public int TotalPoints()
    {
        int tot = 0;
        foreach (int game in points.Values)
        {
            if (game >= 0)
            {
                tot += game;
            }
        }
        return tot;
    }
}

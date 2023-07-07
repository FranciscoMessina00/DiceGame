using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public static int tiriTotali = 0;
    public static int currentPlayer = 0;
    public static List<Player> players = new List<Player>();
    public static void CreatePlayers(List<string> names)
    {
        foreach (string name in names)
        {
            Player player = new Player(name);
            players.Add(player);
        }
        SetInitPlayer();
    }
    public static void SetPointsToPlayer(string gioco, int points = -1)
    {
        Player player = CurrentPlayer();
        player.points[gioco] = points;
    }
    public static void SetInitPlayer()
    {
        System.Random rand = new System.Random();
        currentPlayer = rand.Next(players.Count);
        CurrentPlayer().IsCurrent = true;
    }
    public static void NextPlayer()
    {
        foreach(Player player in players)
        {
            player.IsCurrent = false;
        }
        currentPlayer++;
        if (currentPlayer >= players.Count)
        {
            currentPlayer = 0;
        }
        CurrentPlayer().IsCurrent = true;
        tiriTotali++;
    }
    public static Player CurrentPlayer()
    {
        try
        {
            return players[currentPlayer];
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("Player Not Found");
            return null;
        }
    }
}

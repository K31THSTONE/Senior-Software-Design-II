﻿using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public RoundSettings roundSettings;
    public static GameManager gameInstance;

    private void Awake()
    {
        if (gameInstance != null)
        {
            Debug.LogError("more than 1 game in scene");
        }else
        {
            gameInstance = this;
        }
    }

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer (string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer ( string _playerID)
    {
        return players[_playerID];
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(250, 250, 250, 500));
        GUILayout.BeginVertical();

        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

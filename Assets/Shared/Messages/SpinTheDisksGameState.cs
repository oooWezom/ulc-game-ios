using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SpinTheDisksGameState : GameState
{
    public SpinTheDisksPlayerData[] players_data;
    public int auto_finalization;
}
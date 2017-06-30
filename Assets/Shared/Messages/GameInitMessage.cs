using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameInitMessage : Message {
    public int user_id;
    public int game_id;
    public int left_player_id;
    public String base_url;
}
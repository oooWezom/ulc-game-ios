using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class RockSpockRoundResultMessage : Message
{
    public int[] moves;
    public int winner_id;
    public bool final;
}
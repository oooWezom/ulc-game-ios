using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
class RockSpockMessage
{
    public int move;

    public RockSpockMessage(int move)
    {
        this.move = move;
    }
}
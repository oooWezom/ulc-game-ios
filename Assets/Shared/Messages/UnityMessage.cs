using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class UnityMessage {

    public int type;
    public string message;

    public UnityMessage(int type, string message) {
        this.type = type;
        this.message = message;
    }
}

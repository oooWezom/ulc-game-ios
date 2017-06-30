using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SessionStateMessage : Message {
	public SpinTheDisksGameState session_state;
}
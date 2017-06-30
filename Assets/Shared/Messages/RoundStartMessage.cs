using System;

[Serializable]
public class RoundStartMessage<T> : Message {

    public T p1; //playerData
    public T p2;
}
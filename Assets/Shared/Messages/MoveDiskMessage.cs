using System;

[Serializable]
public class MoveDiskMessage : Message {

    public int disk;
    public int angle;
    public int id;
}
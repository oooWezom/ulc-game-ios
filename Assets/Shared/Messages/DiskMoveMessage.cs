using System;

[Serializable]
public class DiskMoveMessage {

    public int angle;
    public int disk;


    public DiskMoveMessage(int angle, int disk) {
        this.angle = angle;
        this.disk = disk;
    }
}
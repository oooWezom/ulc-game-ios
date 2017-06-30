public class Utils {

    public static float To360Angle(float angle) {
        while (angle < 0.0f) angle += 360.0f;
        while (angle >= 360.0f) angle -= 360.0f;
        return angle;
    }
}
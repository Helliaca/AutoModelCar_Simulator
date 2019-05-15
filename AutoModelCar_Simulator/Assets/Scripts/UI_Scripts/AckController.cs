using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AckController : MonoBehaviour
{
    public RectTransform flw, frw, fvw;
    public Image flw_angle, frw_angle, fvw_angle;
    public Text flw_at, frw_at, fvw_at;
    public Text L_t, R_t, C_t, Angvel_t;
    public Text steering_t, speed_t, speed_r_t, pos_t;

    public void setValues(float mid_angle, float left_angle, float right_angle, float L, float R, Vector3 C, float angvel) {
        frw.rotation = Quaternion.Euler(0, 0, -right_angle);
        frw_angle.fillAmount = Mathf.Abs(right_angle / 360f);
        frw_angle.fillClockwise = right_angle > 0;
        frw_at.text = "ϕᵣ: " + right_angle.ToString("F3") + "°";

        flw.rotation = Quaternion.Euler(0, 0, -left_angle);
        flw_angle.fillAmount = Mathf.Abs(left_angle / 360f);
        flw_angle.fillClockwise = left_angle > 0;
        flw_at.text = "ϕᵢ: " + left_angle.ToString("F3") + "°";

        fvw.rotation = Quaternion.Euler(0, 0, -mid_angle);
        fvw_angle.fillAmount = Mathf.Abs(mid_angle / 360f);
        fvw_angle.fillClockwise = mid_angle > 0;
        fvw_at.text = "ϕ: " + mid_angle.ToString("F3") + "°";

        L_t.text = "L: " + L.ToString("F3") + "m";
        R_t.text = "R: " + R.ToString("F3") + "m";
        C_t.text = "C: " + C.ToString();
        Angvel_t.text = "Angular Velocity: " + angvel.ToString("F3") + "°/s";

        steering_t.text = "/steering: " + Globals.Instance.CurrentCar.steering.ToString();
        speed_t.text = "/speed: " + Globals.Instance.CurrentCar.speed.ToString();
        speed_r_t.text = "Speed: " + Globals.Instance.CurrentCar.speed_real.ToString("F3") + "m/s";
        pos_t.text = "Position: " + Globals.Instance.CurrentCar.transform.position.ToString();

    }
}

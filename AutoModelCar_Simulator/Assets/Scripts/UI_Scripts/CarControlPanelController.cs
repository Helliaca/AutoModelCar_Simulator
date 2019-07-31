using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControlPanelController : MonoBehaviour
{
    public Text CurrentCarLabel;

    public Text rotSlider_leftlabel, rotSlider_rightlabel;
    public Text speedSlider_leftlabel, speedSlider_rightlabel;
    public Text steeringSlider_leftlabel, steeringSlider_rightlabel;
    

    public Slider rotationSlider;
    public Slider speedSlider;
    public Slider steeringSlider;
    
    
    void Start()
    {
        rotSlider_leftlabel.text = rotationSlider.minValue.ToString("F1");
        rotSlider_rightlabel.text = rotationSlider.maxValue.ToString("F1");

        speedSlider_leftlabel.text = speedSlider.minValue.ToString("F1");
        speedSlider_rightlabel.text = speedSlider.maxValue.ToString("F1");

        steeringSlider_leftlabel.text = steeringSlider.minValue.ToString("F1");
        steeringSlider_rightlabel.text = steeringSlider.maxValue.ToString("F1");
    }

    
    void Update()
    {
        if(CurrentCarLabel.text != Globals.Instance.CurrentCar.gameObject.name) CurrentCarLabel.text = Globals.Instance.CurrentCar.gameObject.name;
        rotationSlider.value = mod(Globals.Instance.CurrentCar.transform.eulerAngles.y, 360f);
        speedSlider.value = Globals.Instance.CurrentCar.backAxle.speed_real;
        steeringSlider.value = Globals.Instance.CurrentCar.frontAxle.steering_real_deg;
    }

    public void set_rotation() {
        Vector3 old_rot = Globals.Instance.CurrentCar.transform.eulerAngles;
        Globals.Instance.CurrentCar.transform.eulerAngles = new Vector3(old_rot.x, rotationSlider.value, old_rot.z);
    }

    public void set_speed() {
        Globals.Instance.CurrentCar.backAxle.set_speed_override(speedSlider.value);
    }

    public void set_steering() {
        Globals.Instance.CurrentCar.frontAxle.set_steering_override(steeringSlider.value);
    }

    

    public void next_car() {
        List<UIListEntry_Prop> cars = new List<UIListEntry_Prop>();
        List<UIListEntry> proplist = Globals.Instance.PorpList.entries;

        foreach(UIListEntry p in proplist) {
            if(((UIListEntry_Prop)p).reference.GetComponent<CarController>() != null) {
                cars.Add((UIListEntry_Prop)p);
            }
        }
        if(cars.Count<2) return;

        int i = 0;
        while(cars[i].reference != Globals.Instance.CurrentCar.gameObject && i<cars.Count) i++;
        cars[i].DeSelect();
        i = mod((i+1),cars.Count);
        cars[i].Select();
    }

    public void prev_car() {
        List<UIListEntry_Prop> cars = new List<UIListEntry_Prop>();
        List<UIListEntry> proplist = Globals.Instance.PorpList.entries;

        foreach(UIListEntry p in proplist) {
            if(((UIListEntry_Prop)p).reference.GetComponent<CarController>() != null) {
                cars.Add((UIListEntry_Prop)p);
            }
        }
        if(cars.Count<2) return;

        int i = 0;
        while(cars[i].reference != Globals.Instance.CurrentCar.gameObject && i<cars.Count) i++;
        cars[i].DeSelect();
        i = mod((i-1),cars.Count);
        cars[i].Select();
    }

    public void move_to_handle() {
        Globals.Instance.CurrentCar.transform.position = Globals.Instance.spaceHandle.handleObject.position;
    }

    // Default modulo behaves strangely with negative numbers
    private int mod(int x, int m) {
        return (x%m + m)%m;
    }

    private float mod(float x, float m) {
        return (x%m + m)%m;
    }
}

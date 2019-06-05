using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    [System.Serializable]
    public enum AxisMode {STATIC, SCROLL, ADAPT};
    public enum AxisValue {CAR_XPOS, CAR_YPOS, CAR_SPEED, CAR_STEERING, SYS_FPS, SYS_CPU, SYS_TIME };

    [System.Serializable]
    public class Axis {
        public float min;
        public float max;
        public AxisMode mode;
        public AxisValue type;
        public string label;

        public float getValue() {
            switch(type) {
                case AxisValue.CAR_XPOS : {
                    return Globals.Instance.CurrentCar.transform.position.x;
                }
                case AxisValue.CAR_YPOS : {
                    return Globals.Instance.CurrentCar.transform.position.z;
                }
                case AxisValue.CAR_SPEED : {
                    return Globals.Instance.CurrentCar.backAxle.speed_topic;
                }
                case AxisValue.CAR_STEERING : {
                    return Globals.Instance.CurrentCar.frontAxle.steering_topic;
                }
                case AxisValue.SYS_FPS : {
                    return 1.0f / Time.deltaTime;
                }
                case AxisValue.SYS_CPU : {
                    return 0.0f; // TODO
                }
                case AxisValue.SYS_TIME : {
                    return Time.time;
                }
            }
            Globals.Instance.DevConsole.print("ERR: Invalid Graph Axis value.");
            return 0.0f;
        }

        public bool isValid(float val) {
            switch(mode) {
                case AxisMode.STATIC : {
                    return val>=min && val <=max;
                }
                case AxisMode.SCROLL : {
                    return val>=min;
                }
                case AxisMode.ADAPT : {
                    return true;
                }
            }
            return false;
        }
        public void UpdateAxis(float val) {
            switch(mode) {
                case AxisMode.STATIC : {
                    break;
                }
                case AxisMode.SCROLL : {
                    if(val > max) {
                        min += val-max;
                        max = val;
                    }
                    else if(val<min) {
                        max += val-min;
                        min = val;
                    }
                    break;
                }
                case AxisMode.ADAPT : {
                    if(val > max) max = val;
                    else if(val<min) min = val;
                    break;
                }
            }
        }
    }

    [Header("Internal References")]
    public LineRenderer line;
    public RectTransform canvas;
    public RawImage grid;
    public Text xAxis_label, yAxis_label;
    public Text xAxis_min, xAxis_mid, xAxis_max;
    public Text yAxis_mid, yAxis_max;

    [Header("Settings")]
    public Axis xAxis = new Axis();
    public Axis yAxis = new Axis();
    public float updateFrequency;
    public bool showGrid;
    public int capacity = 100;

    //Private variables
    private float stime = 0.0f;
    List<Vector3> data = new List<Vector3>();

    void Start()
    {
        grid.enabled = showGrid;
        xAxis_label.text = xAxis.label;
        yAxis_label.text = yAxis.label;
    }

    void Update() {
        //Reached updatefrequency threshhold, read new value  
        if(Time.time - stime > 1.0f / updateFrequency) {
            float new_x = xAxis.getValue();
            float new_y = yAxis.getValue();

            if(xAxis.isValid(new_x) && yAxis.isValid(new_y)) { 
                data.Add(new Vector3(new_x, new_y, 0.0f));
                xAxis.UpdateAxis(new_x);
                yAxis.UpdateAxis(new_y);
            }
            
            while(data.Count>0 && (!xAxis.isValid(data[0].x) || !yAxis.isValid(data[0].y))) data.RemoveAt(0);
            while(data.Count>capacity) data.RemoveAt(0);

            line.positionCount=data.Count;
            for(int i=0; i<line.positionCount; i++) {
                line.SetPosition(i, toCanvasSpace(data[i]));
            }

            stime = Time.time;
        }

    }

    void OnGUI()
    {
        grid.uvRect = new Rect(xAxis.min, yAxis.min, xAxis.max - xAxis.min, yAxis.max - yAxis.min);
        xAxis_min.text = xAxis.min.ToString("F2");
        xAxis_mid.text = (xAxis.min + 0.5f*(xAxis.max - xAxis.min)).ToString("F2");
        xAxis_max.text = xAxis.max.ToString("F2");
        yAxis_mid.text = (yAxis.min + 0.5f*(yAxis.max - yAxis.min)).ToString("F2");
        yAxis_max.text = yAxis.max.ToString("F2");
    }

    Vector3 toCanvasSpace(Vector3 v) {
        return new Vector3(canvas.rect.width*(v.x-xAxis.min)/(xAxis.max-xAxis.min), canvas.rect.height*(v.y-yAxis.min)/(yAxis.max-yAxis.min), 0.0f);
    }
}

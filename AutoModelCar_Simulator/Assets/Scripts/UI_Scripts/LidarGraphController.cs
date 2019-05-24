using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LidarGraphController : MonoBehaviour
{
    [System.Serializable]
    public enum AxisMode {STATIC, SCROLL, ADAPT};
    public enum AxisValue {CAR_XPOS, CAR_YPOS, CAR_SPEED, CAR_STEERING, SYS_FPS, SYS_CPU, SYS_TIME, NONE };

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
                    return Globals.Instance.CurrentCar.speed;
                }
                case AxisValue.CAR_STEERING : {
                    return Globals.Instance.CurrentCar.steering;
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
                case AxisValue.NONE : {
                    return 0.0f;
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
    public RosSharp.RosBridgeClient.LaserScanReader lidar;

    public RectTransform canvas;
    public RawImage grid;
    public Text xAxis_label, yAxis_label;
    public Text xAxis_min, xAxis_mid, xAxis_max;
    public Text yAxis_mid, yAxis_max;
    public RectTransform HitPrefab;

    [Header("Settings")]
    private bool relativePosition = true;
    public Axis xAxis = new Axis();
    public Axis yAxis = new Axis();
    public float updateFrequency;
    public bool showGrid;

    //Private variables
    private float stime = 0.0f;
    List<Vector3> data = new List<Vector3>();
    List<RectTransform> hits = new List<RectTransform>();

    void Start()
    {
        grid.enabled = showGrid;
        xAxis_label.text = xAxis.label;
        yAxis_label.text = yAxis.label;
        balanceHits();
        toggleRelativePos(relativePosition);
    }

    void Update() {
        //Reached updatefrequency threshhold, read new value  
        if(Time.time - stime > 1.0f / updateFrequency) {

            balanceHits();

            for(int i=0; i<lidar.samples; i++) {
                float range = lidar.ranges[i];
                if(range>=lidar.range_min && range<=lidar.range_max) {
                    float angle = lidar.angle_min + lidar.angle_increment*i;
                    Vector3 pos = new Vector3(range*Mathf.Cos(angle), range*Mathf.Sin(angle), 0.0f);
                    if(!relativePosition) pos += new Vector3(lidar.transform.position.x, lidar.transform.position.z, 0.0f); //x and y of transform are different than on HUD

                    hits[i].anchoredPosition3D = toCanvasSpace(pos);
                    if(xAxis.isValid(pos.x) && yAxis.isValid(pos.y)) hits[i].gameObject.SetActive(true);
                    else hits[i].gameObject.SetActive(false);
                }
                else hits[i].gameObject.SetActive(false);
            }
            stime = Time.time;
        }

    }

    void balanceHits() {
        while(hits.Count<lidar.samples) hits.Add(Instantiate(HitPrefab, Vector3.zero, Quaternion.identity, canvas));
        while(hits.Count>lidar.samples) {
            GameObject o = hits[hits.Count-1].gameObject;
            hits.RemoveAt(hits.Count-1);
            GameObject.Destroy(o);
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

    public void toggleRelativePos(bool val) {
        this.relativePosition = val;
        if(val) {
            xAxis.mode = yAxis.mode = AxisMode.STATIC;
            xAxis.min = yAxis.min = -lidar.range_max;
            xAxis.max = yAxis.max = lidar.range_max;
        }
        else {
            xAxis.mode = yAxis.mode = AxisMode.ADAPT;
            xAxis.min = -5f;
            xAxis.max = 0f;
            yAxis.min = 0f;
            yAxis.max = 5f;
        }
    }
}

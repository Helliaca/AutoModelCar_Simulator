using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GraphEditor : MonoBehaviour
{
    private AnimationCurve curve;
    public int bez_resolution = 15;
    public LineRenderer line;
    public RectTransform canvas;
    public bool scaleWithDelta = true;
    public RectTransform KeyframePrefab;
    private List<KeyFrameController> keyFrameObjs = new List<KeyFrameController>();
    private float min_x=0.0f, min_y=0.0f, max_x=1.0f, max_y=1.0f;
    void Start()
    {
        UpdateKeyframeObjects();
    }

    public void setCurve(AnimationCurve c) {
        curve = c;
        UpdateAxesIntervals();
        UpdateKeyframeObjects();
    }

    void UpdateAxesIntervals() {
        foreach(Keyframe k in curve.keys) {
            if(k.time<min_x) min_x=k.time;
            if(k.time>max_x) max_x=k.time;
            if(k.value<min_y) min_y=k.value;
            if(k.value>max_y) max_y=k.value;
        }
    }

    void UpdateKeyframeObjects() {
        foreach(KeyFrameController k in keyFrameObjs) {
            Object.Destroy(k.gameObject);
        }

        for(int i=0; i<curve.length; i++) {
            Keyframe f = curve[i];
            RectTransform o = Instantiate(KeyframePrefab, Vector3.zero, Quaternion.identity, canvas);
            KeyFrameController kfc = o.transform.GetComponent<KeyFrameController>();
            kfc.init(canvas, curve, i, min_x, max_x, min_y, max_y);
            keyFrameObjs.Add(kfc);
        }
    }

    void Update()
    {
        // Add new keyframe with right-click
        if(Input.GetMouseButtonDown(1)) { 
            //Get local mouse position
            Vector3 offset;
            if(gameObject.layer == 9) offset = Globals.Instance.UI_GRAPH_CAMERA.WorldToScreenPoint(canvas.position); //Layer 9 is GRAPH_UI, requires special treatment
            else offset = canvas.position;

            Vector3 np = Input.mousePosition - offset;
            curve.AddKey(Mathf.Abs(max_x-min_x)*(np.x/canvas.rect.width), Mathf.Abs(max_y-min_y)*curve.Evaluate(np.x / canvas.rect.width));
            UpdateKeyframeObjects();
        }

        for(int i=0; i<curve.length; i++) {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, keyFrameObjs[i].leftTangent_mode);
            AnimationUtility.SetKeyRightTangentMode(curve, i, keyFrameObjs[i].rightTangent_mode);
        }



        // Update Line Renderer each Frame
        List<Vector3> linePoints = new List<Vector3>();

        for(int i=0; i<curve.length-1; i++) {
            // Linear Interpolation
            if(false && AnimationUtility.GetKeyRightTangentMode(curve, i)==AnimationUtility.TangentMode.Linear && AnimationUtility.GetKeyLeftTangentMode(curve, i+1)==AnimationUtility.TangentMode.Linear) {
                linePoints.Add(toCanvasSpace(new Vector3(curve.keys[i].time, curve.keys[i].value, 0.0f)));
            }
            // Constant Interpolation
            else if(false && AnimationUtility.GetKeyRightTangentMode(curve, i)==AnimationUtility.TangentMode.Constant) {
                linePoints.Add(toCanvasSpace(new Vector3(curve.keys[i].time, curve.keys[i].value, 0.0f)));
                linePoints.Add(toCanvasSpace(new Vector3(curve.keys[i+1].time, curve.keys[i].value, 0.0f)));
            }
            // Bezier Interpolation
            else {
                float delta = curve[i+1].time - curve[i].time;
                for(int j=0; j<bez_resolution; j++) {
                    float cur = curve.keys[i].time + ((float)j/bez_resolution)*delta;
                    linePoints.Add(toCanvasSpace(new Vector3(cur, curve.Evaluate(cur), 0.0f)));
                }
            }
        }
        linePoints.Add(toCanvasSpace(new Vector3(curve.keys[curve.length-1].time, curve.keys[curve.length-1].value, 0.0f)));

        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }

    Vector3 toCanvasSpace(Vector3 v) {
        return new Vector3(canvas.rect.width*(v.x)/(Mathf.Abs(max_x-min_x)), canvas.rect.height*(v.y)/(Mathf.Abs(max_y-min_y)), 0.0f);
    }
}

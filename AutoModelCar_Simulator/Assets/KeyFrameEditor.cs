using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class KeyFrameEditor : MonoBehaviour
{
    public static KeyFrameController keyframe;
    public static bool selected = false;

    public Text leftTangent;
    public Text rightTangent;
    public Text coords;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(keyframe!=null) {
            leftTangent.text = keyframe.leftTangent.ToString("F2");
            rightTangent.text = keyframe.rightTangent.ToString("F2");
            coords.text = "Pos: " + keyframe.key.time.ToString("F2") + ", " + keyframe.key.value.ToString("F2");
        }
    }

    public void changeLeftTangentMode(int tangentmode) {
        if(tangentmode==0) keyframe.leftTangent_mode = AnimationUtility.TangentMode.Free;
        else if(tangentmode==1) keyframe.leftTangent_mode = AnimationUtility.TangentMode.Linear;
        else if(tangentmode==2) keyframe.leftTangent_mode = AnimationUtility.TangentMode.Constant;
        keyframe.forceUpdate();
    }

    public void changeRightTangentMode(int tangentmode) {
        if(tangentmode==0) keyframe.rightTangent_mode = AnimationUtility.TangentMode.Free;
        else if(tangentmode==1) keyframe.rightTangent_mode = AnimationUtility.TangentMode.Linear;
        else if(tangentmode==2) keyframe.rightTangent_mode = AnimationUtility.TangentMode.Constant;
        keyframe.forceUpdate();
    }

    public void changeLeftTangent(string slope) {
        keyframe.leftTangent = float.Parse(slope);
        keyframe.forceUpdate();
    }

    public void changeRightTangent(string slope) {
        keyframe.rightTangent = float.Parse(slope);
        keyframe.forceUpdate();
    }
}

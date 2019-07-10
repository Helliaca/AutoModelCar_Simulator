using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using autominy_msgs = RosSharp.RosBridgeClient.Messages.Autominy;

public class InputTopic_Float<T> : InputTopic<T>
{
    public AnimationCurve interp;
    public void callback(T data) {
        
    }
}

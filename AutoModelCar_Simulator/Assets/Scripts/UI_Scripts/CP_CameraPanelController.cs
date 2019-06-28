using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_CameraPanelController : CP_PanelController
{

    public InputField pos_x, pos_y, pos_z;
    public InputField rot_x, rot_y, rot_z;
    public InputField sensorsize_x, sensorsize_y;
    public InputField lensshift_x, lensshift_y;
    public InputField focallength;
    public Slider fov;

    public InputField topicname, pubfreq, resolution_x, resolution_y;
    public Slider compression;

    public RenderTexture footage_tex;
    private Camera cam;
    private RosSharp.RosBridgeClient.ImagePublisher cam_topic;
    
    void Update()
    {
        if(!pos_x.isFocused) { pos_x.text = reference.localPosition.x.ToString(); }
        if(!pos_y.isFocused) { pos_y.text = reference.localPosition.y.ToString(); }
        if(!pos_z.isFocused) { pos_z.text = reference.localPosition.z.ToString(); }

        if(!rot_x.isFocused) { rot_x.text = reference.localEulerAngles.x.ToString(); }
        if(!rot_y.isFocused) { rot_y.text = reference.localEulerAngles.y.ToString(); }
        if(!rot_z.isFocused) { rot_z.text = reference.localEulerAngles.z.ToString(); }

        if(!sensorsize_x.isFocused) { sensorsize_x.text = cam.sensorSize.x.ToString(); }
        if(!sensorsize_y.isFocused) { sensorsize_y.text = cam.sensorSize.y.ToString(); }

        if(!lensshift_x.isFocused) { lensshift_x.text = cam.lensShift.x.ToString(); }
        if(!lensshift_y.isFocused) { lensshift_y.text = cam.lensShift.y.ToString(); }

        if(!focallength.isFocused) { focallength.text = cam.focalLength.ToString(); }

        if(!topicname.isFocused) { topicname.text = cam_topic.Topic; }
        if(!pubfreq.isFocused) { pubfreq.text = cam_topic.frequency.ToString(); }
        if(!resolution_x.isFocused) { resolution_x.text = cam_topic.resolutionWidth.ToString(); }
        if(!resolution_y.isFocused) { resolution_y.text = cam_topic.resolutionHeight.ToString(); }

        fov.value = cam.fieldOfView;
        compression.value = cam_topic.qualityLevel;
    }

    public void forceUpdate() {
        try {
            if(pos_x.text!="") reference.localPosition = new Vector3(float.Parse(pos_x.text), reference.localPosition.y, reference.localPosition.z);
            if(pos_y.text!="") reference.localPosition = new Vector3(reference.localPosition.x, float.Parse(pos_y.text), reference.localPosition.z);
            if(pos_z.text!="") reference.localPosition = new Vector3(reference.localPosition.x, reference.localPosition.y, float.Parse(pos_z.text));

            if(rot_x.text!="") reference.localEulerAngles = new Vector3(float.Parse(rot_x.text), reference.localEulerAngles.y, reference.localEulerAngles.z);
            if(rot_y.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, float.Parse(rot_y.text), reference.localEulerAngles.z);
            if(rot_z.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, reference.localEulerAngles.y, float.Parse(rot_z.text));

            if(sensorsize_x.text!="") cam.sensorSize = new Vector2(float.Parse(sensorsize_x.text), cam.sensorSize.y);
            if(sensorsize_y.text!="") cam.sensorSize = new Vector2(cam.sensorSize.x, float.Parse(sensorsize_y.text));

            if(lensshift_x.text!="") cam.lensShift = new Vector2(float.Parse(lensshift_x.text), cam.lensShift.y);
            if(lensshift_y.text!="") cam.lensShift = new Vector2(cam.lensShift.x, float.Parse(lensshift_y.text));

            if(focallength.text!="") cam.focalLength = float.Parse(focallength.text);

            if(topicname.text!="") cam_topic.Topic = topicname.text;
            if(pubfreq.text!="") cam_topic.frequency = float.Parse(pubfreq.text);
            if(resolution_x.text!="") cam_topic.resolutionWidth = int.Parse(resolution_x.text);
            if(resolution_y.text!="") cam_topic.resolutionHeight = int.Parse(resolution_y.text);
        }
        catch(System.Exception e) {Globals.Instance.DevConsole.error("Exception occurred while reading TransformPanel Components: " + e.ToString());}
    }

    public override void set_reference(Transform o) {
        OnDisable();    //We simulate turning the panel on and off when we switch refernce to keep cam.targetTexture up to speed.
        reference = o;
        cam_topic = o.GetComponent<RosSharp.RosBridgeClient.ImagePublisher>();
        cam = cam_topic.ImageCamera;
        OnEnable();
    }

    public void changeFov(float val) {
        cam.fieldOfView = val;
    }

    public void changeCompression(float val) {
        cam_topic.qualityLevel = (int)val;
    }

    void OnDisable()
    {
        if(cam!=null) cam.targetTexture = null;
        if(cam_topic!=null) cam_topic.enabled = true;
    }

    void OnEnable()
    {
        if(cam_topic!=null) cam_topic.enabled = false;
        if(cam!=null) cam.targetTexture = footage_tex;
    }
}

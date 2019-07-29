using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using autominy_msgs = RosSharp.RosBridgeClient.Messages.Autominy;

public class TicksPublisher : Publisher<autominy_msgs.Autominy_Tick>
{
    private void naming() {
        PropComponentGroup master = GetComponentInParent(typeof(PropComponentGroup)) as PropComponentGroup; 
        if(master==null) {Globals.Instance.DevConsole.error("Component without master group encountered!"); return;}
        //this.gameObject.name = master.gameObject.name + "_ticks"; //We wont be naming this, as it is the propulsionaxle
        Topic = Globals.Instance.normalize_from_settings("Default_TopicNames_Ticks", master.id.ToString(), master.gameObject.name, "ticks");
    }

    protected override void Start()
    {
        naming();
        base.Start();
    }

    public void publish_ticks(int amount) {
        autominy_msgs.Autominy_Tick tick_obj = new autominy_msgs.Autominy_Tick();
        tick_obj.value = amount;
        Publish(tick_obj);
    }
}

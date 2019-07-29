using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using autominy_msgs = RosSharp.RosBridgeClient.Messages.Autominy;

public class TicksPublisher : Publisher<autominy_msgs.Autominy_Tick>
{
    public void publish_ticks(int amount) {
        autominy_msgs.Autominy_Tick tick_obj = new autominy_msgs.Autominy_Tick();
        tick_obj.value = amount;
        Publish(tick_obj);
    }
}

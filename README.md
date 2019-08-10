# AutoModelCar Simulator

## What does it do?

The AutoModelCar-Simulator is a [Unity3D](https://unity.com/) and [RosSharp](https://github.com/siemens/ros-sharp)-based simulator of the [AutoModelCar](https://github.com/AutoModelCar/AutoModelCarWiki/wiki).

It allows you to easily run and test simple ROS nodes in a virtual environment without requiring access to an actual AutoModelCar. The environment provided closely resembles the conditions found at the [FU-Berlin](https://www.fu-berlin.de/) robotics laboratory.

## How to get started?

### TL;DR

- Download the simulator from the [github releases tab](https://github.com/Helliaca/AutoModelCar_Simulator/releases) and unpack it into a folder of your choosing.
- Make sure there is a [roscore](http://wiki.ros.org/roscore) running on your `ROS_MASTER_URI` machine.
- Make sure your ros-environment machine recognizes autominy_msgs message types. (`source devel/setup.bash`) (See [here](https://github.com/AutoMiny/AutoMiny))
- Launch rosbridge-server on your ros-environment machine: `roslaunch rosbridge_server rosbridge_websocket.launch`.
- In the unpacked folder, change the `RosBridgeServer_Url` in `/UserSettings/settings.txt` to the ip of your rosbridge server. (`RosBridgeServer_Url = ws://localhost:9090` for a single machine setup.)
- Run the simulation executable.

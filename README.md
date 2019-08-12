# AutoModelCar Simulator

## What does it do?

The AutoModelCar-Simulator is a [Unity3D](https://unity.com/) and [RosSharp](https://github.com/siemens/ros-sharp)-based simulator of the [AutoModelCar](https://github.com/AutoModelCar/AutoModelCarWiki/wiki).

It allows you to easily run and test simple ROS nodes in a virtual environment without requiring access to an actual AutoModelCar. The environment provided closely resembles the conditions found at the [FU-Berlin](https://www.fu-berlin.de/) robotics laboratory.

## How to get started?

### TL;DR

- Download the simulator from the [github releases tab](https://github.com/Helliaca/AutoModelCar_Simulator/releases) and unpack it into a folder of your choosing.
- Make sure there is a [roscore](http://wiki.ros.org/roscore) running on the `ROS_MASTER_URI` machine.
- Make sure your ros-environment machine recognizes autominy_msgs message types. (`source devel/setup.bash`) (See [here](https://github.com/AutoMiny/AutoMiny))
- Launch rosbridge-server on your ros-environment machine: `roslaunch rosbridge_server rosbridge_websocket.launch`.
- In the unpacked folder, change the `RosBridgeServer_Url` in `/UserSettings/settings.txt` to the ip of your rosbridge server. (`RosBridgeServer_Url = ws://localhost:9090` for a single machine setup.)
- Run the simulation executable.

### Basic Setup Tutorial (Single machine setup, Linux)

If you still haven't got a rudimentary ROS setup, follow the [ROS installation instructions](http://wiki.ros.org/melodic/Installation), clone the [autominy repository](https://github.com/AutoMiny/AutoMiny), then build and source it.

Otherwise skip this step and move on directly to the following:

1) Download the Linux version from the [releases tab](https://github.com/Helliaca/AutoModelCar_Simulator/releases) and unzip into `~/AutoModelCar_Simulator/`:

```
mkdir AutoModelCar_Simulator
unzip AMCS_v1.0_Lin.zip -d AutoModelCar_Simulator/
```

2) With downloading and unpacking completed, launch a roscore instance:

```
roscore
```

3) Unless you've launched roscore into a separate thread, open up a new terminal. There, reference the running roscore instance by setting the `ROS_MASTER_URI` environment variable:

```
export ROS_MASTER_URI=http://localhost:11311
```

4) Afterwards, launch a rosbrdige server:

```
roslaunch rosbrdige_server rosbrdige_websocket.launch
```

By default, this will start a server on port 9090. Reference the address of this server in the settings.txt of the simulator:

5) Open `AutoModelCar_Simulator/UserSettings/settings.txt` in a text-editor of your preference and replace the `RosBrdigeServer_Url` field with `ws://localhost:9090`.

6) Launch the simulator by running the executable:

```
./AutoModelCar_Simulator/AutoModelCar_Simulator.x86_64
```

Once the simulator is running, you may verify that everthing is working fine by running `rostopic list` in a new terminal. This should yield the following result:

![success](https://i.imgur.com/JLzxb0Q.png)

If you are missing the /actuators topics and your terminal running rosbrdige-server produces a bunch of errors, this means that your rosbrdige-server does not recognize the message-types used by the autominy framework.

![failure1](https://i.imgur.com/Mr8sKi0.png)
![failure1](https://i.imgur.com/dCJZtCR.png)

To rectify this, make sure you have the [autominy](https://github.com/AutoMiny/AutoMiny) repository cloned and built (`catkin build`) and run `source devel/setup.bash` in your catkin workspace before launching the rosbridge server (step 4).

You may also check whether if camera-images are being received by using [rviz](http://wiki.ros.org/rviz).

![cam_rviz](https://i.imgur.com/TL5Yypb.png)

## Usage

### Camera Movement

In order to navigate the scene with the oberving camera hold down *RMB* (Right-Click). Move your mouse to rotate and use the WASD keys to move around.

Alternatively, use the buttons of the main control panel, visible by default in the top right corner:

Left-Click anywhere in the scene to place a 3D handle. Then click on the "Look at Selected" button to rotate the camera towards it. "Reset Camera" moves the camera back to its original position.

![picture](https://i.imgur.com/xJRbv4n.png)

You may also have the camera track a specific car while it is driving: Open up the *Inspector* panel by clicking on "Toggle Inspector" and select a car from the list of props (left side). Once selected click on "Track Selected" to track the given car.

![picture](https://i.imgur.com/kptojNi.png)

### Moving / Rotating a Car

To move a car to a desired position, left-click in the scene where you want the car moved to to place the 3D handle there. Then, in the CarController panel click on "Move to Handle".

![picture](https://i.imgur.com/9TqokMG.png)

You may also use the speed and steering sliders of the CarController panel to drive the car to your desired position. Use the rotation-slider to set the cars rotation.

Alternatively, open up the inspector panel, select your car, select the *Transform* component, and edit the position, rotation and scale values to your liking.

![picture](https://i.imgur.com/yyu1Xuq.png)

### Adding/Removing Obstacles

To add, remove or modify obstacles place the 3D handle by clicking whereever you want to place the obstacle. Then open the inspector panel and click on "Add Prop" underneath the Prop-list. Choose "Obstacles/Misc." and select an object from a list of default props.

![picture](https://i.imgur.com/mup9k32.png)

You may then move, rotate or scale the object by selecting its *Transform* component and editing the given values.

The same principle applies for adding new cars or sensors to an existing prop.

### Topics

(...)

### Using the Developers Console

Press *Left-Ctrl*, *Tab* or click on "Toggle Console" to bring up the developers console. Error messages, warnings and other information will be displayed here.

You may also enter a command end execute it by pressing *Enter*. For a full list of available commands see below.

### Loading a Different Scene

If performance is important, switching to a less detailed scene should improve the simulations framerate.

Open up the developers console and run `load lab_detailed`, `load lab_standard` or `load lab_min` to open up the detailed, standard or minimalist scenes.

![picture](https://i.imgur.com/FQEv871.png)

### Editing Default Settings

## Console Commands

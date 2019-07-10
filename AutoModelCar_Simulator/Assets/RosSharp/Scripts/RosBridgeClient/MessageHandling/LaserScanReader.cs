﻿/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class LaserScanReader : MonoBehaviour
    {
        private Ray[] rays;
        private RaycastHit[] raycastHits;
        private Vector3[] directions;
        private LaserScanVisualizer[] laserScanVisualizers;

        public int samples = 360;
        public int update_rate = 1800;
        public float angle_min = 0;          
        public float angle_max = 6.28f;
        public float angle_increment = 0.0174533f;
        public float time_increment = 0;
        public float scan_time = 0;
        public float range_min = 0.12f;
        public float range_max = 3.5f;
        public float[] ranges ;
        public float[] intensities;
        public LaserScanVisualizer[] visualizers;

        public void Start()
        {
            directions = new Vector3[samples];
            ranges = new float[samples];
            intensities = new float[samples];
            rays = new Ray[samples];
            raycastHits = new RaycastHit[samples];
            
            visualizers = new LaserScanVisualizer[] {Globals.Instance.lsv_lines, Globals.Instance.lsv_mesh, Globals.Instance.lsv_spheres};
        }

        public float[] Scan()
        {
            // if sample number was changed we need to re-initialize all arrays to the appropriate length.
            if(samples!=directions.Length) {
                directions = new Vector3[samples];
                ranges = new float[samples];
                intensities = new float[samples];
                rays = new Ray[samples];
                raycastHits = new RaycastHit[samples];
            }

            MeasureDistance();

            foreach(LaserScanVisualizer laserScanVisualizer in visualizers)
                    laserScanVisualizer.SetSensorData(transform.position, directions, ranges, range_min, range_max);

            // laserScanVisualizers = GetComponents<LaserScanVisualizer>(); //old code, replaced by the bit above.
            // if (laserScanVisualizers != null)
            //     foreach(LaserScanVisualizer laserScanVisualizer in laserScanVisualizers)
            //         laserScanVisualizer.SetSensorData(transform.position, directions, ranges, range_min, range_max);

            return ranges;
        }

        private void MeasureDistance()
        {
            rays = new Ray[samples];
            raycastHits = new RaycastHit[samples];
            ranges = new float[samples];

            for (int i = 0; i < samples; i++)
            {
                rays[i] = new Ray(transform.position, transform.rotation * new Vector3(-Mathf.Cos(angle_min + angle_increment * i), 0, -Mathf.Sin(angle_min + angle_increment * i)));
                directions[i] = rays[i].direction;
                raycastHits[i] = new RaycastHit();
                if (Physics.Raycast(rays[i], out raycastHits[i], range_max))
                    if (raycastHits[i].distance >= range_min && raycastHits[i].distance <= range_max)
                        ranges[i] = raycastHits[i].distance;
            }
        }
    }
}
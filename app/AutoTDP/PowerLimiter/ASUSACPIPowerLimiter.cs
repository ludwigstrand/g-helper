﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GHelper.AutoTDP.PowerLimiter
{
    internal class ASUSACPIPowerLimiter : IPowerLimiter
    {

        private int DefaultA0;
        private int DefaultA3;
        private int DefaultB0 = 0;

        public ASUSACPIPowerLimiter()
        {
            DefaultA0 = Program.acpi.DeviceGet(AsusACPI.PPT_APUA0);
            DefaultA3 = Program.acpi.DeviceGet(AsusACPI.PPT_APUA3);
            if (Program.acpi.IsAllAmdPPT()) // CPU limit all amd models
            {
                DefaultB0 = Program.acpi.DeviceGet(AsusACPI.PPT_CPUB0);
            }
        }

        public void SetCPUPowerLimit(int watts)
        {
            if (Program.acpi.DeviceGet(AsusACPI.PPT_APUA0) >= 0)
            {
                Program.acpi.DeviceSet(AsusACPI.PPT_APUA3, watts, "PowerLimit A3");
                Program.acpi.DeviceSet(AsusACPI.PPT_APUA0, watts, "PowerLimit A0");
            }

            if (Program.acpi.IsAllAmdPPT()) // CPU limit all amd models
            {
                Program.acpi.DeviceSet(AsusACPI.PPT_CPUB0, watts, "PowerLimit B0");
            }
        }


        public int GetCPUPowerLimit()
        {
            return Program.acpi.DeviceGet(AsusACPI.PPT_APUA0);
        }

        public void Dispose()
        {
            //Nothing to dispose here
        }

        public void ResetPowerLimits()
        {
            //Load limits that were set before the limiter engaged
            if (DefaultA3 > 0)
                Program.acpi.DeviceSet(AsusACPI.PPT_APUA3, DefaultA3, "PowerLimit A0");

            if (DefaultA0 > 0)
                Program.acpi.DeviceSet(AsusACPI.PPT_APUA0, DefaultA0, "PowerLimit A3");

            if (Program.acpi.IsAllAmdPPT() && DefaultB0 > 0) // CPU limit all amd models
            {
                Program.acpi.DeviceSet(AsusACPI.PPT_CPUB0, DefaultB0, "PowerLimit B0");
            }
        }
    }
}

namespace UIDesign
{
    class TexcelCommand
    {
        public string torque;
        public string rpm;
        public string duration;
        public string ramp_time;
        public string inletValve;
        public string unitsCode;
        public string ign_Status;
        public string crank_Status;
        public string gplug_Status;

        public string command { get; set; }
        public TexcelCommand()
        {
        }

        //Set dynamometer and throttle demand
        public string TorqueThrottle(string torque, string rpm, string duration, string ramp_time)
        {
            return command = "C1,1," + rpm + "," + ramp_time + ",2," + torque + "," + ramp_time + "," + duration + ",";
        }

        //Set dynamometer, throttle, inlet valve demands
        public string TorqueThrottleInlet()
        {
            return command = "C2,2," + torque + ",6,1," + rpm + ",6,0," + inletValve + ",6," + duration + ",";
        }

        //Idle Command
        public string IdleCommand()
        {
            //C16,<dmode>,<ddem>,<dramp>,<tmode>,<tdem>,<tramp>,<duration>,<checksum><cr>
            return command = "C16,0,0,30,0,0,30,";
        }

        //Dump Load
        public string DumpLoad()
        {
            return command = "C3," + duration + ",";
        }

        //Restore Load
        public string RestoreLoad()
        {
            return command = "C4," + duration + ",";
        }

        //Clear demand queue
        public string clearDemandQueue()
        {
            return command = "C5,";
        }

        //Set torque units
        public string SetTorqueUnits()
        {
            return command = "C5," + unitsCode + ","; //please put unitsCode case in settings
        }

        //Reset Stop/Shutdown
        public string ResetStopShutdown()
        {
            return command = "C7,";
        }

        //Stop/shutdown system
        public string StopShutdownSystem()
        {
            return command = "C8,";
        }

        //Ignition Control
        public string IgnitionControl()
        {
            return command = "C9" + ign_Status + ","; //no need to add checksum
        }

        //Cranking control
        public string CrankingControl()
        {
            return command = "C10," + crank_Status + ",";
        }

        //Glow plug control. Engine glow plugs is a heating device used to aid starting diesel engines.
        public string GlowPlugControl()
        {
            return command = "C11," + gplug_Status + ","; // no need to add checksum
        }

        //Reset time stap to zero
        public string ResetTimeStamp()
        {
            return command = "C12," + gplug_Status + ","; // no need to add checksum
        }

        //This command alloes the host to request an immediate system shutdown,
        //even when the system is in manual control, or there is a queue of commands waiting to be executed
        public string ImmediateStopShutdown()
        {
            return command = "C15,";
        }

        //Immediate set torque and throttle demand
        public string ImmediateSetTorqueThrottle()
        {
            return command = "C16,2," + torque + ",6,1," + rpm + ",6," + duration + ",";
        }

        //Immediate set torque, throttle, and inlet demand
        public string ImmediateSetTorqueThrottleInlet()
        {
            return command = "C17,2," + torque + ",6,1," + rpm + ",6,0," + inletValve + ",6," + duration + ",";
        }

        //Switch to manual control and inhibit operator
        public string ManualControl()
        {
            return command = "C18,5,";
        }

        //Switch to host control
        public string HostControl()
        {
            return command = "C19,";
        }

        //Switch to remote repository changes

        //Request for repeated data packets.
        public string TorqueRpmRequest()
        {
            return command = "P2,0,1,";
        }

        //Request for repeated data packets.
        public string stopTorqueRpmRequest()
        {
            return command = "P2,0,0,";
        }

        //Turn off all Outstanding Data Request
        public string clearDataRequest()
        {
            return command = "P0!\r";
        }

        public string getTexcelStatus()
        {
            return command = "P7,";
        }

    }
}

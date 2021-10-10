using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Exiled.API.Enums;

namespace SCP096Rework
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with opened doors.")]
        public bool RestrictedOpenedDoorsAccess { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with elevators.")]
        public bool RestrictedElevatorAccess { get; set; } = false;

        [Description("Should SCP-096 be be prevented from interacting with lockers.")]
        public bool RestrictedLockerAccess { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with generators.")]
        public bool RestrictGeneratorsAccess { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with warhead.")]
        public bool RestrictWarheadAccess { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with SCP-914.")]
        public bool RestrictSCP914Access { get; set; } = true;

        [Description("Should SCP-096 be be prevented from interacting with workstation.")]
        public bool RestrictWorkstationAccess { get; set; } = true;

        [Description("Which doors will be prevented from opening by SCP-096")]
        public List<DoorType> SpecDoorAccess { get; set; } = new List<DoorType>()
        {
            DoorType.CheckpointLczA,
            DoorType.CheckpointLczB,
            DoorType.CheckpointEntrance,
        };
    }
}

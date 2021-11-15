using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using MapEvents = Exiled.Events.Handlers.Map;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace SCP096Rework
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "SCP096Rework";
        public override string Author { get; } = ".fkn_goose & Mydak";

        public override string Prefix => "SCP096Rework";
        public override Version Version => new Version(1, 1, 0);

        private static readonly Plugin InstanceValue = new Plugin();

        private readonly List<MEC.CoroutineHandle> coroutines = new List<MEC.CoroutineHandle>();

        public void AddCoroutine(MEC.CoroutineHandle coroutineHandle) => this.coroutines.Add(coroutineHandle);

        public void NewCoroutine(IEnumerator<float> coroutine, MEC.Segment segment = MEC.Segment.Update) => this.coroutines.Add(MEC.Timing.RunCoroutine(coroutine, segment));

        public override Version RequiredExiledVersion { get; } = new Version(3, 3, 1);
        private Plugin()
        {
        }

        public static Plugin Instance => InstanceValue;

        private Handlers handlers;

        public Doors Doors;

        public Generators Generators;

        public SCP914 Scp914;

        public Warhead Warhead;

        public Workstation Workstation;

        public override void OnEnabled()
        {
            this.Doors = new Doors(this);
            this.Generators = new Generators(this);
            this.Scp914 = new SCP914(this);
            this.Warhead = new Warhead(this);
            this.Workstation = new Workstation(this);

            handlers = new Handlers(this);

            Scp096Events.CalmingDown += handlers.OnCalmingDown;
            PlayerEvents.ChangingRole += handlers.OnChangingRole;

            PlayerEvents.Hurting += handlers.OnDamage;

            PlayerEvents.InteractingElevator += this.Doors.OnInteractingElevator;
            PlayerEvents.InteractingLocker += this.Doors.OnInteractingLocker;
            PlayerEvents.InteractingDoor += this.Doors.OnInteractingDoor;

            PlayerEvents.UnlockingGenerator += this.Generators.OnUnlockingGenerator;
            PlayerEvents.OpeningGenerator += this.Generators.OnOpeningGenerator;
            PlayerEvents.ActivatingGenerator += this.Generators.OnActivatingGenerator;
            PlayerEvents.StoppingGenerator += this.Generators.OnStoppingGenerator;
            PlayerEvents.ClosingGenerator += this.Generators.OnClosingGenerator;

            Scp914Events.Activating += this.Scp914.OnActivatingScp914;
            Scp914Events.ChangingKnobSetting += this.Scp914.OnKnobChangingScp914;

            PlayerEvents.ActivatingWarheadPanel += this.Warhead.OnActivatingWarheadPanel;
            WarheadEvents.ChangingLeverStatus += this.Warhead.OnChangingLeverStatus;
            WarheadEvents.Starting += this.Warhead.OnStarting;
            WarheadEvents.Stopping += this.Warhead.OnStopping;

            PlayerEvents.ActivatingWorkstation += this.Workstation.OnActivatingWorkstation;

            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Scp096Events.CalmingDown -= handlers.OnCalmingDown;
            PlayerEvents.ChangingRole -= handlers.OnChangingRole;

            PlayerEvents.Hurting += handlers.OnDamage;

            PlayerEvents.InteractingElevator -= this.Doors.OnInteractingElevator;
            PlayerEvents.InteractingLocker -= this.Doors.OnInteractingLocker;
            PlayerEvents.InteractingDoor -= this.Doors.OnInteractingDoor;

            PlayerEvents.UnlockingGenerator -= this.Generators.OnUnlockingGenerator;
            PlayerEvents.OpeningGenerator -= this.Generators.OnOpeningGenerator;
            PlayerEvents.ActivatingGenerator -= this.Generators.OnActivatingGenerator;
            PlayerEvents.StoppingGenerator -= this.Generators.OnStoppingGenerator;
            PlayerEvents.ClosingGenerator -= this.Generators.OnClosingGenerator;

            Scp914Events.Activating -= this.Scp914.OnActivatingScp914;
            Scp914Events.ChangingKnobSetting -= this.Scp914.OnKnobChangingScp914;

            PlayerEvents.ActivatingWarheadPanel -= this.Warhead.OnActivatingWarheadPanel;
            WarheadEvents.ChangingLeverStatus -= this.Warhead.OnChangingLeverStatus;
            WarheadEvents.Starting -= this.Warhead.OnStarting;
            WarheadEvents.Stopping -= this.Warhead.OnStopping;

            PlayerEvents.ActivatingWorkstation -= this.Workstation.OnActivatingWorkstation;

            this.Doors = null;
            this.Generators = null;
            this.Scp914 = null;
            this.Warhead = null;
            this.Workstation = null;
            handlers = null;
            base.OnDisabled();
        }
    }
}

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp096 = Exiled.Events.Handlers.Scp096;
using SvEvents = Exiled.Events.Handlers.Server;
using ScpEvents = Exiled.Events.Handlers.Scp106;
using MapEvents = Exiled.Events.Handlers.Map;
using WarhEvents = Exiled.Events.Handlers.Warhead;
using Scp914Events = Exiled.Events.Handlers.Scp914;

namespace SCP096Rework
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "SCP096Rework";
        public override string Author { get; } = ".fkn_goose & Mydak";
        public override Version Version => new Version(0, 3, 1);
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private Handlers handlers;
        public override void OnEnabled()
        {
            handlers = new Handlers(this);
            Scp096.Enraging += handlers.OnEnraging;
            Scp096.CalmingDown += handlers.OnCalmingDown;

            PlayerEvents.Hurting += handlers.OnDamage;
            Scp914Events.Activating += handlers.OnActivatingScp914;
            Scp914Events.ChangingKnobSetting += handlers.OnKnobChangingScp914;
            WarhEvents.ChangingLeverStatus += handlers.OnChangingWarheadStatus;
            PlayerEvents.ClosingGenerator += handlers.OnClosingGenerator;
            PlayerEvents.EjectingGeneratorTablet += handlers.OnEjectingTabletGenerator;
            PlayerEvents.InteractingLocker += handlers.OnLockerInteract;
            WarhEvents.Starting += handlers.OnStartingWarhead;
            WarhEvents.Stopping += handlers.OnStoppingWarhead;

            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Scp096.Enraging -= handlers.OnEnraging;
            Scp096.CalmingDown -= handlers.OnCalmingDown;

            PlayerEvents.Hurting += handlers.OnDamage;
            Scp914Events.Activating -= handlers.OnActivatingScp914;
            Scp914Events.ChangingKnobSetting -= handlers.OnKnobChangingScp914;
            WarhEvents.ChangingLeverStatus -= handlers.OnChangingWarheadStatus;
            PlayerEvents.ClosingGenerator -= handlers.OnClosingGenerator;
            PlayerEvents.EjectingGeneratorTablet -= handlers.OnEjectingTabletGenerator;
            PlayerEvents.InteractingLocker -= handlers.OnLockerInteract;
            WarhEvents.Starting -= handlers.OnStartingWarhead;
            WarhEvents.Stopping -= handlers.OnStoppingWarhead;

            handlers = null;
            base.OnDisabled();
        }
    }
}

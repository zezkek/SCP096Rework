using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PlayerEv = Exiled.Events.Handlers.Player;
using Scp096 = Exiled.Events.Handlers.Scp096;

namespace SCP096Rework
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "SCP096Rework";
        public override string Author { get; } = ".fkn_goose";
        public override Version Version => new Version(0, 3, 0);
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private Handlers handlers;
        public override void OnEnabled()
        {
            handlers = new Handlers();
            Scp096.Enraging += handlers.OnEnraging;
            Scp096.CalmingDown += handlers.OnCalmingDown;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Scp096.Enraging -= handlers.OnEnraging;
            Scp096.CalmingDown -= handlers.OnCalmingDown;
            handlers = null;
            base.OnDisabled();
        }
    }
}

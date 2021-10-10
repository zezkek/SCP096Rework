namespace SCP096Rework
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class Workstation
    {
        private readonly Plugin plugin;

        public Workstation(Plugin plugin) => this.plugin = plugin;

        public void OnActivatingWorkstation(ActivatingWorkstationEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictWorkstationAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }
    }
}

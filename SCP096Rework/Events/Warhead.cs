namespace SCP096Rework
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class Warhead
    {
        private readonly Plugin plugin;

        public Warhead(Plugin plugin) => this.plugin = plugin;

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictWarheadAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnChangingLeverStatus(ChangingLeverStatusEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictWarheadAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnStarting(StartingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictWarheadAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnStopping(StoppingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictWarheadAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }
    }
}

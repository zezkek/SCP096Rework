namespace SCP096Rework
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class SCP914
    {
        private readonly Plugin plugin;

        public SCP914(Plugin plugin) => this.plugin = plugin;

        public void OnActivatingScp914(ActivatingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictSCP914Access && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnKnobChangingScp914(ChangingKnobSettingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictSCP914Access && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }
    }
}

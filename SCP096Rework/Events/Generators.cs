namespace SCP096Rework
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.API.Enums;

    public class Generators
    {
        private readonly Plugin plugin;

        public Generators(Plugin plugin) => this.plugin = plugin;

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictGeneratorsAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictGeneratorsAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictGeneratorsAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnStoppingGenerator(StoppingGeneratorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictGeneratorsAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }
            else if (Plugin.Instance.Config.RestrictGeneratorsAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }
    }
}

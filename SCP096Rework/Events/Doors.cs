namespace SCP096Rework
{
    using System;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class Doors
    {
        private readonly Plugin plugin;

        public Doors(Plugin plugin) => this.plugin = plugin;

        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }

            else if (Plugin.Instance.Config.RestrictedElevatorAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled)
            {
                return;
            }

            else if (Plugin.Instance.Config.RestrictedLockerAccess && ev.Player.Role == RoleType.Scp096)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsBypassModeEnabled || ev.Player.Role != RoleType.Scp096)
            {
                return;
            }

            if (ev.Door.IsOpen && Plugin.Instance.Config.RestrictedOpenedDoorsAccess)
            {
                ev.IsAllowed = false;
            }
            else if (Plugin.Instance.Config.SpecDoorAccess.Contains(ev.Door.Type))
            {
                if (!Handlers.angryScp096s.ContainsKey(ev.Player))
                    ev.IsAllowed = false;
            }
        }
    }
}

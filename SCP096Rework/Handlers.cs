using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PlayableScps;
using Scp096 = PlayableScps.Scp096;
using Exiled.API.Enums;
using UnityEngine;
using HarmonyLib;
using Assets;
using System.Reflection.Emit;

namespace SCP096Rework
{
    public class Handlers
    {
        List<Player> allPlayers = new List<Player>();
        public void OnCalmingDown(CalmingDownEventArgs ev)
        {
            if (ev.Scp096._targets.Count != 0)
                ev.IsAllowed = false;
            else
            {
                ev.IsAllowed = true;
                Timing.KillCoroutines();
            }
        }
        public void OnEnraging(EnragingEventArgs ev)
        {
            Targets(ev.Scp096, ev.Player);
        }
        private IEnumerator<float> Targets(Scp096 scp,Player player)
        {
            allPlayers = Player.List.ToList();
            int lightZoneAmount=0;
            int heavyZoneAmount = 0;
            int officeZoneAmount = 0;
            int surfaceAmount = 0;
            for(; ; )
            {
                if (scp._targets.Count == 0)
                {
                    scp.PlayerState = PlayableScps.Scp096PlayerState.Calming;
                    Timing.KillCoroutines();
                }
                yield return Timing.WaitForSeconds(5f);
                foreach (var target in scp._targets)
                {
                    //foreach (var rndPlayer in allPlayers.Where(x => x.Id == target.playerId))
                        switch (Player.Get(target.playerId).Zone)
                        {
                            case ZoneType.LightContainment:
                                lightZoneAmount++;
                                break;
                            case ZoneType.HeavyContainment:
                                heavyZoneAmount++;
                                break;
                            case ZoneType.Entrance:
                                officeZoneAmount++;
                                break;
                            case ZoneType.Surface:
                                surfaceAmount++;
                                break;
                        }
                }
                player.ShowHint("Целей в легкой зоне " + lightZoneAmount + "\nЦелей в тяжелой зоне " + heavyZoneAmount + "\nЦелей в офисной зоне " + officeZoneAmount + "\nЦелей  на поверхности " + surfaceAmount, 5f);
            }
        }
    }
}

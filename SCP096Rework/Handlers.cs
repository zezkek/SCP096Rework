using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using Scp096 = PlayableScps.Scp096;
using Exiled.API.Enums;
using UnityEngine;

namespace SCP096Rework
{
    public class Handlers
    {
        List<Player> targetplayers = new List<Player>();
        List<Player> AllPlayers = new List<Player>();
        Player Scp096;
        public void OnEnraging(EnragingEventArgs ev)
        {
            Scp096 = ev.Player;
            Timing.RunCoroutine(CheckForTargets(ev));
            Timing.RunCoroutine(RefreshPosition());
            ListOfTargets();
        }
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            targetplayers.Add(ev.Target);
        }
        public void OnDied(DiedEventArgs ev)
        {
            if (targetplayers.Contains(ev.Target))
                targetplayers.Remove(ev.Target);

        }
        private IEnumerator<float> CheckForTargets(EnragingEventArgs ev)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(10f);
                if (targetplayers.Count == 0)
                {
                    ev.Scp096.PlayerState = PlayableScps.Scp096PlayerState.Calming;
                    break;
                }
                else
                    ev.Scp096.PlayerState = PlayableScps.Scp096PlayerState.Enraging;
            }
        }
        private IEnumerator<float> RefreshPosition()
        {
            for(; ; )
            {
                yield return Timing.WaitForSeconds(1f);
                AllPlayers = Player.List.ToList();
                foreach (Player RandomPlayer in AllPlayers.Where(x => x.IsHuman))
                {
                    foreach (Player Target in targetplayers.Where(y => y.Id == RandomPlayer.Id))
                    {
                        Target.Position = RandomPlayer.Position;
                    }
                }
                foreach (Player targetscp in AllPlayers.Where(x => x.Role == RoleType.Scp096 && x.Id==Scp096.Id))
                    Scp096 = targetscp;
            }
        }
        private void ListOfTargets()
        {
            float mindistance = Vector3.Distance(targetplayers[0].Position, Scp096.Position);
            int countoutside = 0;
            int countoffice = 0;
            int countheavy = 0;
            int countlight = 0;
            bool sameZone = false;
            foreach (Player target in targetplayers)
            {
                if (target.Zone == Scp096.Zone)
                {
                    if (Vector3.Distance(target.Position, Scp096.Position) < mindistance)
                        mindistance = Vector3.Distance(target.Position, Scp096.Position);
                    sameZone = true;
                }
                else
                {
                    switch (target.Zone)
                    {
                        case ZoneType.Entrance:
                            countoffice++;
                            break;
                        case ZoneType.Surface:
                            countoutside++;
                            break;
                        case ZoneType.HeavyContainment:
                            countheavy++;
                            break;
                        case ZoneType.LightContainment:
                            countlight++;
                            break;
                    }
                }
            }
            if(sameZone)
                if (mindistance >= 30)
                    Scp096.ShowHint("Расстояние до ближайшей цели: " + mindistance.ToString() + "\n", 1);
                else
                { }
            else
                Scp096.ShowHint("Целей на поверхности: " + countoutside + " Целей в офисной зоне: " + countoffice + "\n" 
                    + "Целей в тяжелой зоне: " + countheavy + " Целей в легкой зоне: " + countlight);
        }
    }
}

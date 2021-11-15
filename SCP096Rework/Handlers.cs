using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PlayableScps;
using Exiled.API.Enums;
using UnityEngine;
using HarmonyLib;
using Assets;
using System.Reflection.Emit;

namespace SCP096Rework
{
    public class Handlers
    {
        private readonly Plugin plugin;
        public Handlers(Plugin plugin)
        {
            this.plugin = plugin;
        }
        public void OnCalmingDown(CalmingDownEventArgs ev)
        {
            if (ev.Scp096._targets.Count != 0)
                ev.IsAllowed = false;
            else
            {
                ev.IsAllowed = true;
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Scp096)
            {
                Plugin.Instance.NewCoroutine(StatsMsg(ev.Player));
            }
            Timing.CallDelayed(0.1f, () => {
                if (Player.List.Where(x => x.Role == RoleType.Scp096).Count() > 0)
                {
                    PlayableScps.Scp096 scp = (PlayableScps.Scp096)Player.List.Where(x => x.Role == RoleType.Scp096).First()?.CurrentScp;
                    if (scp != null)
                    {
                        scp._targets.Remove(ev.Player.ReferenceHub);
                    }
                }
            });

        }

        /// <summary>
        /// GUI with the systems parameters.
        /// </summary>
        /// <param name="player">Player should be SCP-079.</param>
        /// <returns>Coroutine.</returns>
        /// 
        internal ZoneType GetZone(Player player, out ZoneType result)
        {
            string zone = $"{player.CurrentRoom.Type}".Remove(2);
            result = ZoneType.Unspecified;
            switch (zone.ToLower())
            {
                case "hc":
                    result = ZoneType.HeavyContainment;
                    break;
                case "lc":
                    result = ZoneType.LightContainment;
                    break;
                case "ez":
                    result = ZoneType.Entrance;
                    break;
                case "su":
                    result = ZoneType.Surface;
                    break;
                default:
                    break;
            }
            return result;
        }
        internal IEnumerator<float> StatsMsg(Player player)
        {
            yield return MEC.Timing.WaitForSeconds(1f);
            int showName = 7;

            Log.Debug("Coroutine statsMsg has started.", Plugin.Instance.Config.Debug);
            short enraseDelay = 1; 
            while (player != null)
            {
                try
                {
                    int lines = 11;
                    string response = string.Empty;
                    if (player.Role != RoleType.Scp096)
                    {
                        yield break;
                    }
                    PlayableScps.Scp096 scp = (PlayableScps.Scp096)player.CurrentScp;
                    if (scp._targets.Count > 0)
                    {
                        enraseDelay = 3;
                        player.CanSendInputs = true;
                        Dictionary<ZoneType, HashSet<Player>> targetsZones = new Dictionary<ZoneType, HashSet<Player>> { };
                        response = "<align=left><pos=-21%><size=25><color=#C1B5B5><b>МЕСТОНАХОЖДЕНИЕ ЦЕЛЕЙ</b></color></pos></align>\n";

                        string msg = "<align=left><b><size=20><pos=-21%><color=#C1B5B5>%name</color></pos><pos=-7%> :  <color=#C1B5B5>%count %curzone</color></pos></size></b></align>\n";
                        Player closest = Player.Get(scp._targets.Where(t => GetZone(Player.Get(t.playerId), out ZoneType temp) != ZoneType.Unspecified).First().playerId);
                        foreach (var targetHub in scp._targets)
                        {
                            Player target = Player.Get(targetHub.playerId);
                            if (!targetsZones.ContainsKey(GetZone(target, out ZoneType zone)))
                            {
                                targetsZones.Add(zone, new HashSet<Player> { target });
                            }
                            else
                            {
                                targetsZones[zone].Add(target);
                            }
                            if(closest == null)
                            {
                                continue;
                            }
                            else if (Vector3.Distance(player.Position, closest.Position) > Vector3.Distance(player.Position, target.Position) && GetZone(target, out ZoneType temp) != ZoneType.Unspecified)
                            {
                                closest = target;
                            }
                        }

                        foreach (var zone in targetsZones.Keys.ToList())
                        {
                            switch (zone)
                            {
                                case ZoneType.Surface:
                                    response += msg.
                                        Replace("%name", "ПОВЕРХНОСТЬ").
                                        Replace("%count", $"{targetsZones[zone].Count}");
                                    break;
                                case ZoneType.Entrance:
                                    response += msg.
                                        Replace("%name", "ОФИСНАЯ").
                                        Replace("%count", $"{targetsZones[zone].Count}");
                                    break;
                                case ZoneType.HeavyContainment:
                                    response += msg.
                                        Replace("%name", "ТЯЖЁЛАЯ").
                                        Replace("%count", $"{targetsZones[zone].Count}");
                                    break;
                                case ZoneType.LightContainment:
                                    response += msg.
                                        Replace("%name", "ЛЁГКАЯ").
                                        Replace("%count", $"{targetsZones[zone].Count}");
                                    break;
                                case ZoneType.Unspecified:
                                    response += msg.
                                        Replace("%name", "НЕИЗВЕСТНО").
                                        Replace("%count", $"{targetsZones[zone].Count}");
                                    break;
                                default:
                                    break;
                            }
                            lines--;
                            if(zone == GetZone(player, out ZoneType temp))
                            {
                                response = response.Replace(" %curzone", " </color><color=#990000>[ТЕКУЩЕЕ]");
                            }
                            else
                            {
                                response = response.Replace(" %curzone", string.Empty);
                            }
                        }
                        if (closest != null)
                        {
                            if (GetZone(closest, out ZoneType targetZone) == GetZone(player, out ZoneType playerZone))
                            {
                                string exceptions = string.Empty;
                                if(playerZone == ZoneType.HeavyContainment)
                                {
                                    if(player.Position.y > -600)
                                    {
                                        exceptions = "<color=#990000>[БОЕГОЛОВКА]</color>";
                                    }
                                    if(player.Position.y > -750)
                                    {
                                        exceptions = "<color=#990000>[КОМНАТА СОД. SCP-049]</color>";
                                    }
                                }
                                response += $"\n<align=left><b><size=20><pos=-21%><color=#990000>ДИСТАНЦИЯ</color></pos><pos=-7%> :  <color=#C1B5B5>{Vector3.Distance(closest.Position, player.Position)} м.</color> {exceptions}</pos></size></b></align>\n";
                            }

                            lines -= 2;
                        }
                    }
                    else if(scp._targets.Count == 0 && scp.EnragedOrEnraging)
                    {
                        if(enraseDelay <= 0) 
                        {
                            player.EnableEffect(EffectType.Ensnared, 1.5f);
                            player.CanSendInputs = false;
                        }
                        else
                        {
                            enraseDelay--;
                        }

                    }
                    else
                    {
                        player.DisableEffect(EffectType.Ensnared);
                    }
                    if (lines != 11)
                    {
                        response += "\n";
                    }

                    for (int i = 0; i < lines; i++)
                    {
                        response = "\n" + response;
                    }

                    response += "\n";
                    if (showName > 0)
                    {
                        response += "<align=left><pos=-21%><b><size=20><color=#C1B5B5>ПОЛЬЗОВАТЕЛЬ</pos><pos=-7%>  :  </color>" +
                        $"<color=#990000>{player.Nickname}</color></b></size></pos></align>\n";
                        showName--;
                    }
                    else
                    {
                        response += "\n";
                    }
                    response += "<align=left><pos=-21%><b><size=20>" +
                        "<color=#C1B5B5>РОЛЬ" +
                        "<pos=-7%>  :  </color><color=#990000>[ДАННЫЕ УДАЛЕНЫ]</color></b></size></pos></align>\n";

                    player.ShowHint(response, 1.5f);
                    response = string.Empty;

                }
                catch (Exception e)
                {
                    Log.Error($"Catched error while trying to display message: {e}");
                    break;
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public void OnDamage(HurtingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Target.Role != RoleType.Scp096) return;
            Log.Debug($"OnDamage event has been taken.\nTarget: {ev.Target.Nickname}\nRole: {ev.Target.Role}" +
                      $"\nAmount of damage: {ev.Amount}\nDamageType: {ev.DamageType.Name}", plugin.Config.Debug);
            if (ev.DamageType == DamageTypes.Nuke || ev.DamageType == DamageTypes.Wall || ev.DamageType == DamageTypes.Decont)
            {
                return;
            }
            else
            {
                ev.IsAllowed = false;
            }
        }
    }
}

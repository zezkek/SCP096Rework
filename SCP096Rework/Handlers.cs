using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using Scp096 = PlayableScps.Scp096;

namespace SCP096Rework
{
    public class Handlers
    {
        List<Player> targetplayers=new List<Player>();
        public void OnEnraging(EnragingEventArgs ev)
        {
            Log.Info("Старт рейджа");
            Timing.RunCoroutine(CheckForTargets(ev));
            Log.Info("Время рейджа "+ev.Scp096.AddedTimeThisRage.ToString());
        }
        public void OnDied(DiedEventArgs ev)
        {
            Log.Info("Смэрть");
            if (targetplayers.Contains(ev.Target))
                targetplayers.Remove(ev.Target);

        }
        private IEnumerator<float> CheckForTargets(EnragingEventArgs ev)
        {
            Log.Info("Начало цикла проверок");
            for (; ; )
            {
                yield return Timing.WaitForSeconds(10f);
                Log.Info("Проверка на количество целей");
                Log.Info("Количество целей " + targetplayers.Count);
                if (targetplayers.Count == 0)
                {
                    ev.Scp096.PlayerState = PlayableScps.Scp096PlayerState.Calming;
                    Log.Info("Заканчиваем рейдж");
                    break;
                }
                else
                    ev.Scp096.PlayerState = PlayableScps.Scp096PlayerState.Enraging;
            }
        }
    }
}

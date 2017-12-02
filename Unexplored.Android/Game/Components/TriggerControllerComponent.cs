using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;
using Unexplored.Core.Attributes;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Unexplored.Core.Types;
using Unexplored.Android.Core.Types;

namespace Unexplored.Game.Components
{
    public struct TriggerMap
    {
        public int Id;
        public string Type;
        public bool Once;
        public bool Used;
        public bool NotifyParent, NotifyTrigger, NotifyChild;
        public GameObject Object;
        public GameObject ParentObject;

        public bool PlaySound;
        public SoundEffectInstance Sound;

        public TriggerMap CopyWithObject(GameObject gameObject)
        {
            this.Object = gameObject;
            return this;
        }
    }

    public class TriggerControllerComponent : BehaviorComponent
    {
        [CustomProperty]
        public string Children;
        
        public TriggerMap[] Triggers;
        private int triggersCount;

        public TriggerControllerComponent()
        {
            Drawable = false;
            Children = "";
        }

        public override void SingleInitialize()
        {
            Triggers = Children.Split('\n').Select(line => {
                string[] lineParts = line.Split(':');
                TriggerMap map = new TriggerMap();
                if (lineParts[0] == "sound")
                {
                    map.Id = -1;
                    map.PlaySound = true;
                    map.Sound = lineParts.Length > 1 ? StaticResources.Sounds[lineParts[1].Trim()].CreateInstance() : null;
                }
                else
                {
                    int.TryParse(lineParts[0], out map.Id);
                    map.Type = lineParts.Length > 1 ? lineParts[1].Trim() : null;
                    if (lineParts.Length > 3)
                    {
                        // Тот кто вошел в триггер узнает об указанном объекте.
                        map.NotifyParent = lineParts[3].Trim() == "parent";
                        // Указанный в скрипте объект узнает о хозяине данного компонента.
                        map.NotifyTrigger = lineParts[3].Trim() == "trigger";
                        // Хозяин данного компонента узнает о том кто указан в скрипте.
                        map.NotifyChild = lineParts[3].Trim() == "child";
                        // Иначе тот кто указан в скрипте узнает о том кто вошел в тригер.
                    }
                }
                // Выполняем единожды (вошли, стоим, вышли и войти уже не сможем (не будет триггера))
                map.Once = lineParts.Length > 2 ? lineParts[2].Trim() == "once" : false;
                return map;
            }).ToArray();
        }

        public override void Initialize()
        {
            triggersCount = Triggers.Length;
        }

        public override void OnTriggerEnter(Trigger rawTrigger)
        {
            int triggerIndex = triggersCount;
            while (--triggerIndex >= 0)
            {
                var trigger = Triggers[triggerIndex];
                if (trigger.Once)
                {
                    if (trigger.Used)
                        continue;
                    Triggers[triggerIndex].Used = true;
                }

                // Грязный хак
                if (trigger.PlaySound)
                {
                    if (trigger.Sound is SoundEffectInstance sound)
                    {
                        if (sound.State == SoundState.Paused ||
                            sound.State == SoundState.Stopped)
                        {
                            sound.Play();
                        }
                    }
                    continue;
                }

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnEventBegin(new GameEvent(trigger.Object, trigger.Type));
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnEventBegin(new GameEvent(GameObject, trigger.Type));
                else if (trigger.NotifyChild)
                    GameObject.OnEventBegin(new GameEvent(trigger.Object, trigger.Type));
                else
                    trigger.Object.OnEventBegin(new GameEvent(rawTrigger.GameObject, trigger.Type));

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }

        public override void OnTriggerStay(Trigger rawTrigger)
        {
            int triggerIndex = triggersCount;
            while (--triggerIndex >= 0)
            {
                var trigger = Triggers[triggerIndex];
                if (trigger.Once && trigger.Used || trigger.PlaySound)
                    continue;

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnEventStay(new GameEvent(trigger.Object, trigger.Type));
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnEventStay(new GameEvent(GameObject, trigger.Type));
                else if (trigger.NotifyChild)
                    GameObject.OnEventStay(new GameEvent(trigger.Object, trigger.Type));
                else
                    trigger.Object.OnEventStay(new GameEvent(rawTrigger.GameObject, trigger.Type));

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }

        public override void OnTriggerExit(Trigger rawTrigger)
        {
            int triggerIndex = triggersCount;
            while (--triggerIndex >= 0)
            {
                var trigger = Triggers[triggerIndex];
                if (trigger.Once && trigger.Used || trigger.PlaySound)
                    continue;

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnEventEnd(new GameEvent(trigger.Object, trigger.Type));
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnEventEnd(new GameEvent(GameObject, trigger.Type));
                else if (trigger.NotifyChild)
                    GameObject.OnEventEnd(new GameEvent(trigger.Object, trigger.Type));
                else
                    trigger.Object.OnEventEnd(new GameEvent(rawTrigger.GameObject, trigger.Type));

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }
    }
}

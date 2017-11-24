﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;
using Unexplored.Core.Attributes;

namespace Unexplored.Game.Components
{
    public struct TriggerMap
    {
        public int Id;
        public string Type;
        public bool Once;
        public bool Used;
        public bool NotifyParent, NotifyTrigger;
        public GameObject Object;
        public GameObject ParentObject;

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
        }

        public override void SingleInitialize()
        {
            Triggers = Children.Split('\n').Select(line => {
                string[] lineParts = line.Split(':');
                TriggerMap map = new TriggerMap();
                int.TryParse(lineParts[0], out map.Id);
                map.Type = lineParts.Length > 1 ? lineParts[1] : null;
                map.Once = lineParts.Length > 2 ? lineParts[2] == "once" : false;
                map.NotifyParent = lineParts.Length > 3 ? lineParts[3] == "parent" : false;
                map.NotifyTrigger = lineParts.Length > 3 ? lineParts[3] == "trigger" : false;
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

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnTriggerEnter(new Trigger(trigger.Type, trigger.Object), true);
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnTriggerEnter(new Trigger(trigger.Type, GameObject), true);
                else
                    trigger.Object.OnTriggerEnter(new Trigger(trigger.Type, rawTrigger.GameObject), true);

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }

        public override void OnTriggerStay(Trigger rawTrigger)
        {
            if (rawTrigger.Type != null)
                return;

            int triggerIndex = triggersCount;
            while (--triggerIndex >= 0)
            {
                var trigger = Triggers[triggerIndex];
                if (trigger.Once && trigger.Used)
                    continue;

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnTriggerStay(new Trigger(trigger.Type, trigger.Object));
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnTriggerStay(new Trigger(trigger.Type, GameObject));
                else
                    trigger.Object.OnTriggerStay(new Trigger(trigger.Type, rawTrigger.GameObject));

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }

        public override void OnTriggerExit(Trigger rawTrigger)
        {
            int triggerIndex = triggersCount;
            while (--triggerIndex >= 0)
            {
                var trigger = Triggers[triggerIndex];
                if (trigger.Once && trigger.Used)
                    continue;

                if (trigger.NotifyParent)
                    rawTrigger.GameObject.OnTriggerExit(new Trigger(trigger.Type, trigger.Object));
                else if (trigger.NotifyTrigger)
                    trigger.Object.OnTriggerExit(new Trigger(trigger.Type, GameObject));
                else
                    trigger.Object.OnTriggerExit(new Trigger(trigger.Type, rawTrigger.GameObject));

                Triggers[triggerIndex].ParentObject = rawTrigger.GameObject;
            }
        }
    }
}

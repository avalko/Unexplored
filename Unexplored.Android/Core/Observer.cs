using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core
{
    public class Observer
    {
        class NotifyItem
        {
            public Action action;
            public bool once = false;
        }

        private static Dictionary<string, List<NotifyItem>> observers = new Dictionary<string, List<NotifyItem>>();

        public static void Subscribe(string property, Action action, bool once = false)
        {
            if (property == null || action == null)
                return;

            if (!observers.ContainsKey(property))
                observers[property] = new List<NotifyItem>();

            observers[property].Add(new NotifyItem { action = action, once = once });
        }

        public static void UnSubscribe(string property, Action action)
        {
            if (property == null || action == null)
                return;

            if (!observers.ContainsKey(property))
                return;

            observers[property].RemoveAll((item) => item.action == action);
        }

        public static void SubscribeAll(Action action, bool once = false, params string[] properties)
        {
            if (properties == null || action == null || properties.Length == 0)
                return;

            for (int i = 0; i < properties.Length; i++)
            {
                string property = properties[i];
                if (!observers.ContainsKey(property))
                    observers[property] = new List<NotifyItem>();
                observers[property].Add(new NotifyItem { action = action, once = once });
            }

        }

        private string objectName;

        public Observer(string objectName)
        {
            this.objectName = objectName;
        }

        public void Notify([CallerMemberName] string property = null)
        {
            if (property == null)
                return;

            property = objectName + "_" + property;
            NotifyAll(property);
        }

        public static void NotifyAll(string property)
        {
            if (property == null)
                return;

            if (observers.ContainsKey(property))
            {
                for (int i = 0; i < observers[property].Count; i++)
                {
                    var observer = observers[property][i];
                    observer.action.Invoke();
                    if (observer.once)
                        observers[property].RemoveAt(i--);
                }
            }
        }
    }
}

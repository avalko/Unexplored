using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Unexplored.Core.Base;

namespace Unexplored.Android.Core.Types
{
    public struct GameEvent
    {
        public string Type;
        public GameObject GameObject;

        public GameEvent(GameObject gameObject, string type)
        {
            GameObject = gameObject;
            Type = type;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Common.Core
{
    public sealed class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        private Action<object> defaultAction = null;

        private TypeSwitch() { }

        public static TypeSwitch Create()
        {
            return new TypeSwitch();
        }

        public TypeSwitch Case<T>(Action<T> action)
        {
            matches.Add(typeof(T), (x) => action((T)x));
            return this;
        }

        public TypeSwitch Default(Action<object> action)
        {
            if (defaultAction != null)
                throw new InvalidOperationException("The default action on TypeSwitch can only be set to one action. TypeSwitch.Default() should only be called once per TypeSwitch object.");

            defaultAction = action;
            return this;
        }

        public void Switch(object x)
        {
            Action<object> action;
            if (matches.TryGetValue(x.GetType(), out action))
                action(x);
            else if (defaultAction != null)
                defaultAction(x);
        }
    }
}

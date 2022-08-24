using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Utils
{
    public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
    {
        Func<TValue> _default;

        public TValue DefaultValue
        {
            get { return _default(); }
        }
        public DictionaryWithDefault() : base() {
            _default = () =>
            {
                return default(TValue);
            };
        }
        public DictionaryWithDefault(Func<TValue> defaultValue) : base()
        {
            _default = defaultValue;
        }

        public new TValue this[TKey key]
        {
            get
            {
                var worked = base.TryGetValue(key, out var v);
                if (!worked) return DefaultValue;
                return v;
            }
            set { 
                base[key] = value;
            }
        }
    }
}

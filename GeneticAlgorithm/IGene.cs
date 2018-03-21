using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public interface IGene<Key, Value>
    {
        void SetValue(Value value);
        Value GetValue();
        Key GetKey();
    }
}

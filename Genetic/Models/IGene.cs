namespace Genetic.Models
{
    public interface IGene<Key, Value>
    {
        void SetValue(Value value);
        Value GetValue();
        Key GetKey();
    }
}

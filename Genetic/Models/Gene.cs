namespace Genetic.Models
{
    class Gene<Key, Value> : IGene<Key, Value>
    {
        private Key place;
        private Value factory;

        public Gene(Key place, Value factory)
        {
            this.place = place;
            this.factory = factory;
        }

        public Key GetKey()
        {
            return place;
        }

        public Value GetValue()
        {
            return factory;
        }

        public void SetValue(Value factory)
        {
            this.factory = factory;
        }
    }
}
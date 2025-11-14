namespace FootballSimulator.Core.Domain
{
    public class Country : FSDataEntity
    {
        protected Country() { }
        public Country(string name, string code)
        {
            Name = name;
            Code = code;
        }
        public string? Name { get; private set; }
        public string? Code { get; private set; }
    }
}

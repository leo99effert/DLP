namespace DLP.DTO
{
    internal class Country
    {
        public CountryName Name { get; set; }
        public List<string> Capital { get; set; }
    }
    internal class CountryName
    {
        public string Common { get; set; }
    }
}
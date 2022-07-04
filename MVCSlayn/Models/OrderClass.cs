namespace MVCSlayn.Models
{
    public class OrderClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateСreation { get; set; }
        public string Status { get; set; }
        public List<PositionClass> positions { get; set; }
        public int sum { get; set; }

    }
}

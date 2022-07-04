namespace MVCSlayn.Models
{
    public class PositionClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string priceOldOrder { get; set; }
        public List<PriceTypeClass> PriceTypes { get; set; }
    }
}

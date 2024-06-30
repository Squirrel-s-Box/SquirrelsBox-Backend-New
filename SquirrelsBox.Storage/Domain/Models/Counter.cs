namespace SquirrelsBox.Storage.Domain.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public string UserCodeOwner { get; set; }
        public int Boxes { get; set; }
        public int Sections { get; set; }
        public int Items { get; set; }
    }
}

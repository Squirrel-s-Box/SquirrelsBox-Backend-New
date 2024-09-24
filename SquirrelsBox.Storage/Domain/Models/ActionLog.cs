namespace SquirrelsBox.Storage.Domain.Models
{
    public class ActionLog
    {
        public int Id { get; set; }
        public string Usercode{ get; set; }
        public int BoxId { get; set; }
        public int SectionId { get; set; }
        public int ItemId { get; set; }
        public int SpecId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
}

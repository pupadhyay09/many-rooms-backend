namespace ManyRoomStudio.Boundary.Responses
{
    public class MasterDetailsResponse
    {

        public int ID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string? OtherName { get; set; }
        public string? Description { get; set; }
        public int? ParentID { get; set; }

    }
}

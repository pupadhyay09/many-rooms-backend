﻿namespace ManyRoomStudio.Models.Entities
{
    public class Auditable
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

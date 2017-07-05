using AisProjectCore.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    [Table("AIS_Reserve_Information")]
    public class ReserveModel
    {
        [Key]
        public int Code { get; set; }
        public DateTime Datetime { get; set; }
        public int MeetingRoomCode { get; set; }
        public int PurposeCode { get; set; }
        public int EmployeeCode { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime ReserveStart { get; set; }
        public DateTime ReserveEnd { get; set; }
    }
}

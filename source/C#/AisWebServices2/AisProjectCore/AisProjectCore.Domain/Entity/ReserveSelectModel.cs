using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    public class ReserveSelectModel
    {
        public int MeetRoomCode { get; set; }
        public DateTime ReservedDate { get; set; }
    }
}

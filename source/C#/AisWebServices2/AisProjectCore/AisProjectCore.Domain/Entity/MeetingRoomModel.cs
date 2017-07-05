using AisProjectCore.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    public class MeetingRoomModel
    {
        [Key]
        public int Code { get; set; }
        public string Name { get;set; }
    }
}

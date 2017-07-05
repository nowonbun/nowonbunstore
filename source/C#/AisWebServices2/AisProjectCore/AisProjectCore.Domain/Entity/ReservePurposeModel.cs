using AisProjectCore.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    public class ReservePurposeModel
    {
        [Key]
        public int Code { get; set; }
        public string Content { get; set; }
    }
}

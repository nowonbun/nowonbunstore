using AisProjectCore.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    [Table("AIS_Employee_Information")]
    public class EmployeeInfoModel
    {
        [Key]
        public int Idx { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OAuth { get; set; }
    }
}

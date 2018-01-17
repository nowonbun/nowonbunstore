using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Hibernate.Entity
{
    public class Usertable
    {
        public virtual int Idx { get; set; }
        public virtual String ID { get; set; }
        public virtual String PW { get; set; }
    }
}
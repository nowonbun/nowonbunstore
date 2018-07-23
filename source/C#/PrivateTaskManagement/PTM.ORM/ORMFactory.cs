using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADOX;
using System.IO;
using PTM.ORM.Impl;
using PTM.ORM.Common;
using PTM.ORM.Dao;

namespace PTM.ORM
{
    public class ORMFactory
    {
        private static ORMFactory instance = null;
        public static void Initialize()
        {
            if (instance == null)
            {
                instance = new ORMFactory();
            }
        }
        public static T GetService<T>(Type type) where T : IDao
        {
            Initialize();
            if (!instance.servicemap.ContainsKey(type))
            {
                throw new Exception("The instance dao is not declared.");
            }
            return (T)instance.servicemap[type];
        }

        private Dictionary<Type, IDao> servicemap;
        private ORMFactory()
        {
            Database db = new Database();
            servicemap = new Dictionary<Type, IDao>();
            // DI Bindings
            servicemap.Add(typeof(IMemoDao), new MemoDao(db));
            servicemap.Add(typeof(ITaskDao), new TaskDao(db));
        }
    }
}

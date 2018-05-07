using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace PTM.ORM.Common
{
    sealed class Database
    {
        private string _db_path = Path.GetDirectoryName(Application.ExecutablePath) + "\\PrivateTaskManagement.mdb";

        public Database()
        {
            if (!Exists())
            {
                Create();
            }
        }
        public OleDbConnection GetConnetcion()
        {
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _db_path);
        }
        public bool Exists()
        {
            return File.Exists(_db_path);
        }
        public void Create()
        {
            new ADOX.CatalogClass().Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _db_path + ";Jet OLEDB:Engine Type=5");
        }
    }
}

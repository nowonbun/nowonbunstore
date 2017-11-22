using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace pgsqlExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string connect = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    "192.168.0.2", "5432", "postgres", "", "postgres");
            NpgsqlConnection conn = new NpgsqlConnection(connect);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from household.household order by IDX desc";
            conn.Open();

            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                String buf = dr.GetValue(0) + "\t" + dr.GetValue(1) + "\t" + dr.GetValue(2) + "\t" + dr.GetValue(3) + "\t" + dr.GetValue(4) + "\t";
                Console.WriteLine(buf);
            }

            conn.Close();

            Console.ReadLine();
        }
    }
}

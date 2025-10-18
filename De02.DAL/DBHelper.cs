using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace De02.DAL
{
    public static class DBHelper
    {
        // chỉnh lại Data Source / Integrated Security theo môi trường của bạn
        public static string ConnectionString = @"Data Source=.;Initial Catalog=QLSanpham;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}

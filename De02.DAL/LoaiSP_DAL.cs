using System;
using System.Collections.Generic;
using De02.Entities;
using System.Data.SqlClient;

namespace De02.DAL
{
    public class LoaiSP_DAL
    {
        public List<LoaiSP> GetAll()
        {
            var list = new List<LoaiSP>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT MaLoai, TenLoai FROM LoaiSP";
                using (var cmd = new SqlCommand(sql, conn))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(new LoaiSP
                        {
                            MaLoai = dr["MaLoai"].ToString().Trim(),
                            TenLoai = dr["TenLoai"].ToString()
                        });
                }
            }
            return list;
        }

        public string GetMaLoaiByTen(string tenLoai)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT MaLoai FROM LoaiSP WHERE TenLoai = @ten";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", tenLoai);
                    var r = cmd.ExecuteScalar();
                    return r == null ? null : r.ToString().Trim();
                }
            }
        }
    }
}

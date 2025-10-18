using De02.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace De02.DAL
{
    public class SanPham_DAL
    {
        public List<SanPham> GetAll()
        {
            List<SanPham> list = new List<SanPham>();
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SanPham", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SanPham
                    {
                        MaSP = dr["MaSP"].ToString().Trim(),
                        TenSP = dr["TenSP"].ToString(),
                        Ngaynhap = Convert.ToDateTime(dr["Ngaynhap"]),
                        MaLoai = dr["MaLoai"].ToString().Trim()
                    });
                }
            }
            return list;
        }
        
        public void Insert(SanPham sp)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Sanpham(MaSP, TenSP, Ngaynhap, MaLoai) VALUES(@MaSP, @TenSP, @Ngaynhap, @MaLoai)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                    cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                    cmd.Parameters.AddWithValue("@Ngaynhap", sp.Ngaynhap);
                    cmd.Parameters.AddWithValue("@MaLoai", sp.MaLoai);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(SanPham sp)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Sanpham SET TenSP=@TenSP, Ngaynhap=@Ngaynhap, MaLoai=@MaLoai WHERE MaSP=@MaSP";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                    cmd.Parameters.AddWithValue("@Ngaynhap", sp.Ngaynhap);
                    cmd.Parameters.AddWithValue("@MaLoai", sp.MaLoai);
                    cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string maSP)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Sanpham WHERE MaSP=@MaSP";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", maSP);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<SanPham> SearchByName(string namePart)
        {
            var list = new List<SanPham>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT MaSP, TenSP, Ngaynhap, MaLoai FROM Sanpham WHERE TenSP LIKE @key";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@key", "%" + namePart + "%");
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new SanPham
                            {
                                MaSP = dr["MaSP"].ToString().Trim(),
                                TenSP = dr["TenSP"].ToString(),
                                Ngaynhap = Convert.ToDateTime(dr["Ngaynhap"]),
                                MaLoai = dr["MaLoai"].ToString().Trim()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using XmlTask.Dto;
using XmlTask.Utils;

namespace XmlTask.Services
{
    public class SqlDataService
    {
        public FilesDto GetFiles()
        {
            var dto = new FilesDto
            {
                Files = new List<FileDto>()
            };

            var dataRows = GetAllFiles();

            if (dataRows == null) return dto;

            foreach (DataRow dr in dataRows)
            {
                dto.Files.Add(new FileDto
                {
                    DateTime = DateTime.Parse(dr["DateTime"].ToString()),
                    FileVersion = dr["FileVersion"].ToString(),
                    Name = dr["Name"].ToString()
                });
            }


            return dto;
        }

        public void InsertEntities(FilesDto list)
        {
            Logger.Log.Info("Подключение к Бд");
            Logger.Log.Info("Строка подключения к Бд: " + StringExtension.ConnectionString);

            using (var conn = new SqlConnection(StringExtension.ConnectionString))
            {
                conn.Open();
                Logger.Log.Info("Подключились к Бд");

                foreach (var el in list.Files)
                {
                    using (var comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = StringExtension.InsertFileQuery;
                        comm.Parameters.AddWithValue("@val1", el.Name);
                        comm.Parameters.AddWithValue("@val2", el.FileVersion);
                        comm.Parameters.AddWithValue("@val3", el.DateTime);
                        try
                        {
                            comm.ExecuteNonQuery();
                            Logger.Log.Info("Команда выполнилась: " + comm.CommandText);
                        }
                        catch (SqlException e)
                        {
                            Logger.Log.Error(e.Message);
                        }
                    }
                }
            }
        }

        public List<FileModel> GetFileModels()
        {
            var items = new List<FileModel>();

            var dataRows = GetAllFiles();
            if (dataRows == null) return items;

            items.AddRange(from DataRow dr in dataRows
                select new FileModel
                {
                    Id = long.Parse(dr["Id"].ToString()),
                    DateTime = dr["DateTime"].ToString(),
                    FileVersion = dr["FileVersion"].ToString(),
                    Name = dr["Name"].ToString()
                });

            return items;
        }

        public void UpdateFileInfo(FileModel model)
        {
            Logger.Log.Info("Подключение к Бд");
            Logger.Log.Info("Строка подключения к Бд: " + StringExtension.ConnectionString);

            using (var conn = new SqlConnection(StringExtension.ConnectionString))
            {
                conn.Open();
                Logger.Log.Info("Подключились к Бд");

                using (var comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = StringExtension.UpdateFileQuery;
                    comm.Parameters.AddWithValue("@val1", model.Name);
                    comm.Parameters.AddWithValue("@val2", model.FileVersion);
                    comm.Parameters.AddWithValue("@val3", model.DateTime);
                    comm.Parameters.AddWithValue("@val4", model.Id);
                    try
                    {
                        comm.ExecuteNonQuery();
                        Logger.Log.Info("Команда выполнилась: " + comm.CommandText);
                    }
                    catch (SqlException e)
                    {
                        Logger.Log.Error(e.Message);
                    }
                }
            }
        }

        private DataRowCollection GetAllFiles()
        {
            using (var conn = new SqlConnection(StringExtension.ConnectionString))
            {
                conn.Open();
                Logger.Log.Info("Подключились к Бд");

                using (var comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = StringExtension.GetAllQuery;
                    try
                    {
                        var da = new SqlDataAdapter(comm);
                        var dt = new DataTable();
                        da.Fill(dt);

                        Logger.Log.Info("Команда выполнилась: " + comm.CommandText);
                        return dt.Rows;
                    }
                    catch (SqlException e)
                    {
                        Logger.Log.Error(e.Message);
                    }
                }
            }

            return null;
        }
    }
}
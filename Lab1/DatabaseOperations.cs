using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace Lab1
{
    static class DatabaseOperations
    {
        private static void log_an_error(DateTime err_time, string lab_id, string err_text, NpgsqlCommand cmd)
        {
            cmd.CommandText = "insert into error_data(err_time,err_lab,err_text) values ('" + err_time.ToString() + "','" + lab_id + "','" + err_text + "');";
            cmd.ExecuteNonQuery();
        }

        public static bool initDB()
        {
            try
            {
                var settings = (new Form_Params()).getSettings();
                var cs = "Host=" + settings[0] + ";Port=" + settings[1] + ";Username=" + settings[2] + ";Password=" + settings[3] + ";Database=" + settings[4];

                var con = new NpgsqlConnection(cs);
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select datname from pg_database where datname='tzspd_user_errors';";

                var str = cmd.ExecuteScalar();

                if (str == null)    //in case if tzspd_user_errors does not exist
                {
                    cmd.CommandText = "create database tzspd_user_errors;"; //create it
                    cmd.ExecuteNonQuery();
                    con.ChangeDatabase("tzspd_user_errors");
                    cmd.CommandText = "create table  error_data( err_time timestamp, " +
                        " err_lab varchar, err_text varchar);"; //create table within new databese
                    cmd.ExecuteNonQuery();
                }
                //con.ChangeDatabase("tzspd_user_errors");
                cmd.Cancel();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static NpgsqlConnection connect()
        {
            try
            {
                var settings = (new Form_Params()).getSettings();
                var cs = "Host=" + settings[0] + ";Port=" + settings[1] + ";Username=" + settings[2] + ";Password=" + settings[3] + ";Database=" + settings[4];
                var con = new NpgsqlConnection(cs);
                con.Open();
                return con;
            }
            catch
            {
                return null;
            }
        }

        public static bool log_error(string lab_id, string err_text)
        {
            try
            {
                var connection = connect();
                if (connection == null)
                    throw new Exception("Error connecting");
                connection.ChangeDatabase("tzspd_user_errors");
                var cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                log_an_error(DateTime.Now, lab_id, err_text, cmd);
                cmd.Cancel();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

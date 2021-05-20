using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public static class MSSQL_logging
    {
        public static double log_error_onTimer(DateTime logDateTime, string labnumber, string logtext)
        {
            var runtimer = new System.Diagnostics.Stopwatch();
            runtimer.Start();
            log_error(logDateTime, labnumber, logtext);
            runtimer.Stop();
            return runtimer.ElapsedMilliseconds;
        }

        public static int log_error(DateTime logDateTime, string labnumber, string logtext)
        {
            try
            {
                if (logtext.Length >= 256 || labnumber.Length >= 40)
                    throw new Exception("Too long!");


                var log_in_char = logtext.ToArray();
                byte[] log_bytes = Encoding.UTF8.GetBytes(log_in_char);

                var encrypted_log = AesInst.CreateEncryptor().TransformFinalBlock(log_bytes, 0, log_bytes.Length);

                string encrypted_log_HEX = "0x";
                foreach (byte part in encrypted_log)
                {
                    encrypted_log_HEX += part.ToString("X2");
                }

                string queryString = "insert into logs values(\'" + labnumber + "\', " + encrypted_log_HEX + ", CURRENT_TIMESTAMP)";

                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(queryString, database_connection);

                if (command.Connection.State != System.Data.ConnectionState.Open)
                    command.Connection.Open();

                command.ExecuteNonQuery();

                command.Connection.Close();

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public static List<Decrypted_logline> get_database_logs()
        {
            try
            {
                string queryString = "select * from logs";
                var command = new Microsoft.Data.SqlClient.SqlCommand(queryString, database_connection);

                if (command.Connection.State != System.Data.ConnectionState.Open)
                    command.Connection.Open();

                command.ExecuteNonQuery();
                var reader = command.ExecuteReader();

                List<Decrypted_logline> logs = new List<Decrypted_logline>();
                while (reader.Read())
                {
                    //DEFINE BYTE LENGTH
                    //byte[] EncryptedBytes = new byte[1024];
                    //reader.GetBytes(1, 0, EncryptedBytes, 0, 1024);
                    byte[] EncryptedBytes = (byte[])reader[2];
                    //count non-zero end bytes
                    int nonZeroBytes = 0;
                    for (int i = EncryptedBytes.Length-1; i > -1; i--)
                    {
                        if (EncryptedBytes[i] == 0x00)
                            nonZeroBytes++;
                        else
                            break;
                    }
                    var DecryptedBytes = AesInst.CreateDecryptor().TransformFinalBlock(EncryptedBytes, 0, EncryptedBytes.Length-nonZeroBytes);
                    var DecryptedChars = Encoding.UTF8.GetChars(DecryptedBytes);
                    var Decrypted_Text = new string(DecryptedChars);
                    logs.Add(new Decrypted_logline(reader.GetDateTime(3), reader.GetString(1), Decrypted_Text));
                }

                command.Connection.Close();
                return logs;
            }
            catch
            {
                string queryString = "select * from logs";
                var command = new Microsoft.Data.SqlClient.SqlCommand(queryString, database_connection);

                if (command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();
                return new List<Decrypted_logline>();
            }
        }

        public static void GetPrivateKeyFromDB()
        {
            //getting key
            string queryString = "select * from keys where login_id=" + user_id.ToString();
            var command_newkey = new Microsoft.Data.SqlClient.SqlCommand(queryString, database_connection);

            if (command_newkey.Connection.State != System.Data.ConnectionState.Open)
                command_newkey.Connection.Open();

            command_newkey.ExecuteNonQuery();
            var reader = command_newkey.ExecuteReader();
            byte[] dbprivkey = new byte[48];
            byte[] dbiv = new byte[32];
            while (reader.Read())
            {
                reader.GetBytes(1, 0, dbprivkey, 0, 48);
                reader.GetBytes(2, 0, dbiv, 0, 32);
            }
            AesInst = System.Security.Cryptography.Aes.Create();

            var privKeysha256 = MathOperations.sha256_byte(userpassword);
            var ivmd5 = MathOperations.md5_byte(userpassword);
            //enc private key with sha256(pwd) and md5(pwd)
            AesInst.Key = privKeysha256;
            AesInst.IV = ivmd5;

            var SecretKey = AesInst.CreateDecryptor().TransformFinalBlock(dbprivkey, 0, dbprivkey.Length);
            var InitVector = AesInst.CreateDecryptor().TransformFinalBlock(dbiv, 0, dbiv.Length);

            //apply decrypted key
            AesInst.Key = SecretKey;
            AesInst.IV = InitVector;

            command_newkey.Connection.Close();
        }

        public class Decrypted_logline
        {
            public Decrypted_logline(DateTime logDateTime_, string labnumber_, string logtext_)
            {
                this.logDateTime = logDateTime_;
                this.labnumber = labnumber_;
                this.logtext = logtext_;
            }
            public DateTime logDateTime;
            public string labnumber;
            public string logtext;
        }

        public static Microsoft.Data.SqlClient.SqlConnection database_connection = null;
        public static string userpassword = "";
        public static int user_id = 0;
        public static System.Security.Cryptography.Aes AesInst = null;
    }
}

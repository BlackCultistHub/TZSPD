using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form_login : Form
    {
        public Form_login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string host = textBox_host.Text;
                string port = textBox_port.Text;
                //resolving host string
                string hostString = "tcp:" + host;
                if (port != "Default" && port != "")
                    hostString += ", " + port;


                string getLoginsLogin = "auth_check";
                string getLoginsPassw = "ceb3478&Bc23b2&";

                Microsoft.Data.SqlClient.SqlConnectionStringBuilder extractLoginsConnBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder();

                extractLoginsConnBuilder.DataSource = hostString;
                extractLoginsConnBuilder.ConnectTimeout = 10;
                extractLoginsConnBuilder.UserID = getLoginsLogin;
                extractLoginsConnBuilder.Password = getLoginsPassw;
                extractLoginsConnBuilder.Authentication = Microsoft.Data.SqlClient.SqlAuthenticationMethod.SqlPassword;
                extractLoginsConnBuilder.IntegratedSecurity = false;
                extractLoginsConnBuilder.TrustServerCertificate = true;

                string queryString = "select * from logins";
                StringBuilder errorMessages = new StringBuilder();

                int user_id = 0;

                using (Microsoft.Data.SqlClient.SqlConnection connection1 = new Microsoft.Data.SqlClient.SqlConnection(extractLoginsConnBuilder.ConnectionString))
                {
                    Microsoft.Data.SqlClient.SqlCommand command1 = new Microsoft.Data.SqlClient.SqlCommand(queryString, connection1);
                    try
                    {
                        command1.Connection.Open();
                        command1.ExecuteNonQuery();

                        var reader = command1.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            throw new Exception("Provided login not found or password is incorrect");
                        }

                        string login = textBox_login.Text;
                        string password = textBox_password.Text;
                        string hash = MathOperations.sha256(password);

                        bool login_in = false;
                        while (reader.Read())
                        {
                            if (reader.GetString(1) == login && reader.GetString(2) == hash)
                            {
                                user_id = reader.GetInt32(0);
                                login_in = true;
                            }
                        }
                        reader.Close();

                        command1.Connection.Close();
                        if (!login_in)
                        {
                            throw new Exception("Provided login not found or password is incorrect");
                        }
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex)
                    {
                        for (int i = 0; i < ex.Errors.Count; i++)
                        {
                            errorMessages.Append("Index #" + i + "\n" +
                                "Message: " + ex.Errors[i].Message + "\n" +
                                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                                "Source: " + ex.Errors[i].Source + "\n" +
                                "Procedure: " + ex.Errors[i].Procedure + "\n");
                        }
                        throw new Exception(errorMessages.ToString());
                    }
                }

                string basicLogin = "airlogger";
                string basicPassw = "n3i7A7834bo&T21h@tbn";

                extractLoginsConnBuilder.UserID = basicLogin;
                extractLoginsConnBuilder.Password = basicPassw;

                queryString = "select * from keys";

                Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(extractLoginsConnBuilder.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(queryString, connection);
                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    //check if user has key
                    var reader = command.ExecuteReader();
                    bool has_key = false;
                        
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == user_id)
                            has_key = true;
                    }
                    reader.Close();

                    if (!has_key)
                    {
                        MessageBox.Show("Ключ не найден. Сейчас будет сгенерирован новый ключ и добавлен в базу данных.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        var aesInst = System.Security.Cryptography.Aes.Create();
                        var privkey = aesInst.Key;
                        var iv = aesInst.IV;
                        var privKeysha256 = MathOperations.sha256_byte(textBox_password.Text);
                        var ivmd5 = MathOperations.md5_byte(textBox_password.Text);

                        //enc private key with sha256(pwd) and md5(pwd)
                        aesInst.Key = privKeysha256;
                        aesInst.IV = ivmd5;

                        var Encrypted_SK = aesInst.CreateEncryptor().TransformFinalBlock(privkey, 0, privkey.Length);
                        string Encrypted_SK_String_HEX = "0x";
                        foreach (byte part in Encrypted_SK)
                        {
                            Encrypted_SK_String_HEX += part.ToString("X2");
                        }
                        var Encrypted_IV = aesInst.CreateEncryptor().TransformFinalBlock(iv, 0, iv.Length);
                        string Encrypted_IV_String_HEX = "0x";
                        foreach (byte part in Encrypted_IV)
                        {
                            Encrypted_IV_String_HEX += part.ToString("X2");
                        }

                        queryString = "insert into keys values (" + user_id.ToString() + ", " + Encrypted_SK_String_HEX + ", " + Encrypted_IV_String_HEX + ")";
                        Microsoft.Data.SqlClient.SqlCommand command_newkey = new Microsoft.Data.SqlClient.SqlCommand(queryString, connection);
                        //command.Connection.Open();
                        command_newkey.ExecuteNonQuery();
                        command_newkey.Connection.Close();
                    }
                    //save session data
                    MSSQL_logging.user_id = user_id;
                    MSSQL_logging.userpassword = textBox_password.Text;
                    MSSQL_logging.database_connection = connection;
                    MSSQL_logging.GetPrivateKeyFromDB();

                    command.Connection.Close();

                    //open start panel
                    this.Hide();
                    var form_start = new Form_start();
                    form_start.Closed += (s, args) => this.Close();
                    form_start.Show();
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Index #" + i + "\n" +
                            "Message: " + ex.Errors[i].Message + "\n" +
                            "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                            "Source: " + ex.Errors[i].Source + "\n" +
                            "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    throw new Exception(errorMessages.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

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
    public partial class pwd_change : Form
    {
        List<string> badpwdlist = new List<string>();
        public pwd_change()
        {
            InitializeComponent();

            badpwdlist.Add("123456123456123456");
            badpwdlist.Add("123456789123456789");
            badpwdlist.Add("picture1picture1");
            badpwdlist.Add("passwordpassword");
            badpwdlist.Add("1234567812345678");
            badpwdlist.Add("11111111111111");
            badpwdlist.Add("123123123123123");
            badpwdlist.Add("123451234512345");
            badpwdlist.Add("12345678901234567890");
            badpwdlist.Add("senhasenhasenha");
            badpwdlist.Add("12345671234567");
            badpwdlist.Add("qwertyqwertyqwerty");
            badpwdlist.Add("abc123abc123abc123");
            badpwdlist.Add("Million2Million2");
            badpwdlist.Add("OOOOOOOOOOOOOO");
            badpwdlist.Add("1234123412341234");
            badpwdlist.Add("iloveyouiloveyou");
            badpwdlist.Add("aaron431aaron431");
            badpwdlist.Add("password1password1");
            badpwdlist.Add("qqww1122qqww1122");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_prevpwd.Text != MSSQL_logging.userpassword)
                    throw new Exception("Пароль не соответствует текущему");
                if (textBox_newpwd.TextLength < 14)
                    throw new Exception("Минимальная длина пароля - 14 символов");
                if (textBox_newpwd.Text != textBox_newPwdCopy.Text)
                    throw new Exception("Новые пароли не совпадают");

                //checking for bad passwords
                bool bad = false;
                foreach (var pwd in badpwdlist)
                {
                    if (pwd == textBox_newpwd.Text)
                    {
                        bad = true;
                        break;
                    }
                }
                if (bad)
                    throw new Exception("Выбран слабый пароль!");

                //hashing
                string hash = MathOperations.sha256(textBox_newpwd.Text);

                //update password ===============
                string query = "update logins set hash='" + hash + "' where id=" + MSSQL_logging.user_id.ToString();

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, MSSQL_logging.database_connection);

                if (command.Connection.State != System.Data.ConnectionState.Open)
                    command.Connection.Open();

                command.ExecuteNonQuery();

                //delete old pwd =============
                query = "delete from keys where login_id=" + MSSQL_logging.user_id;
                command.CommandText = query;
                command.ExecuteNonQuery();

                //renew key =================
                MSSQL_logging.GetPrivateKeyFromDB();

                var aesInst = System.Security.Cryptography.Aes.Create();
                var privKeysha256 = MathOperations.sha256_byte(textBox_newpwd.Text);
                var ivmd5 = MathOperations.md5_byte(textBox_newpwd.Text);

                //enc private key with sha256(pwd) and md5(pwd)
                aesInst.Key = privKeysha256;
                aesInst.IV = ivmd5;

                var Encrypted_SK = aesInst.CreateEncryptor().TransformFinalBlock(MSSQL_logging.AesInst.Key, 0, MSSQL_logging.AesInst.Key.Length);
                string Encrypted_SK_String_HEX = "0x";
                foreach (byte part in Encrypted_SK)
                {
                    Encrypted_SK_String_HEX += part.ToString("X2");
                }
                var Encrypted_IV = aesInst.CreateEncryptor().TransformFinalBlock(MSSQL_logging.AesInst.IV, 0, MSSQL_logging.AesInst.IV.Length);
                string Encrypted_IV_String_HEX = "0x";
                foreach (byte part in Encrypted_IV)
                {
                    Encrypted_IV_String_HEX += part.ToString("X2");
                }

                query = "insert into keys values (" + MSSQL_logging.user_id.ToString() + ", " + Encrypted_SK_String_HEX + ", " + Encrypted_IV_String_HEX + ")";

                command.CommandText = query;
                command.ExecuteNonQuery();
                command.Connection.Close();

                //renew obj=================
                MSSQL_logging.userpassword = textBox_newpwd.Text;
                MSSQL_logging.GetPrivateKeyFromDB();

                MessageBox.Show("Успешно!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using IniParser;
using IniParser.Model;

namespace Lab1
{
    public partial class Form_Params : Form
    {
        public bool use_db;
        public string database_role;
        public string pwd;
        public string default_db;
        public string host;
        public string port;

        ///<returns>Array: 0: Host, 1: Port, 2: User, 3: Password, 4: Database</returns>
        public string[] getSettings()
        {
            if (new FileInfo("settings.ini").Length == 0)
                fillDefaults();
            else
                readSettings();

            List<string> settings = new List<string>();
            settings.Add(host);
            settings.Add(port);
            settings.Add(database_role);
            settings.Add(pwd);
            settings.Add(default_db);
            return settings.ToArray();
        }

        public bool dataBaseEndabled()
        {
            if (new FileInfo("settings.ini").Length == 0)
                fillDefaults();
            else
                readSettings();

            return use_db;
        }
        public Form_Params()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.CenterToScreen();

            timer_check_checkboxes.Start();

            if (new FileInfo("settings.ini").Length == 0)
                fillDefaults();
            else
                readSettings();
            initSettings();
            this.Refresh();
        }

        void fillDefaults()
        {
            use_db = true;
            database_role = "postgres";
            pwd = "password";
            default_db = "postgres";
            host = "localhost";
            port = "5432";
            var parser = new FileIniDataParser();
            IniData data = new IniData();
            data.Sections.Add(new SectionData("Database"));
            data.Sections["Database"].AddKey("Use_database");
            data.Sections["Database"].AddKey("Host");
            data.Sections["Database"].AddKey("Port");
            data.Sections["Database"].AddKey("User");
            data.Sections["Database"].AddKey("Password");
            data.Sections["Database"].AddKey("Default_database");
            data.Sections["Database"]["Use_database"] = use_db.ToString();
            data.Sections["Database"]["Host"] = host;
            data.Sections["Database"]["Port"] = port;
            data.Sections["Database"]["User"] = database_role;
            data.Sections["Database"]["Password"] = pwd;
            data.Sections["Database"]["Default_database"] = default_db;
            parser.WriteFile("settings.ini", data);
        }

        void readSettings()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("settings.ini");
            use_db = bool.Parse(data["Database"]["Use_database"]);
            host = data["Database"]["Host"];
            port = data["Database"]["Port"];
            database_role = data["Database"]["User"];
            pwd = data["Database"]["Password"];
            default_db = data["Database"]["Default_database"];
        }

        void initSettings()
        {
            checkBox_use_db.Checked = use_db;
            textBox_host.Text = host;
            textBox_port.Text = port;
            textBox_user.Text = database_role;
            textBox_pwd.Text = pwd;
            textBox_database.Text = default_db;
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.IPAddress host_ip;
                if (textBox_host.Text != "localhost" && textBox_host.Text != "Localhost")
                    if (!System.Net.IPAddress.TryParse(textBox_host.Text, out host_ip))
                        throw new Exception("IP-адрес хоста введён некорректно.");
                int port_int = 0;
                if (!int.TryParse(textBox_port.Text, out port_int))
                    throw new Exception("Порт введён некорректно.");
                if (port_int < 0 || port_int > 65535)
                    throw new Exception("Порт должен находиться в диапазоне от 0 до 65535.");
                if (textBox_database.Text == "tzspd_user_errors")
                    throw new Exception("Нельзя выбрать данное имя БД.");
                use_db = checkBox_use_db.Checked;
                host = textBox_host.Text;
                port = textBox_port.Text;
                database_role = textBox_user.Text;
                pwd = textBox_pwd.Text;
                default_db = textBox_database.Text;

                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("settings.ini");
                data.Sections["Database"]["Use_database"] = use_db.ToString();
                data.Sections["Database"]["Host"] = host;
                data.Sections["Database"]["Port"] = port;
                data.Sections["Database"]["User"] = database_role;
                data.Sections["Database"]["Password"] = pwd;
                data.Sections["Database"]["Default_database"] = default_db;
                parser.WriteFile("settings.ini", data);

                toolStripStatusLabel_status.Text = "Сохранено.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void timer_check_checkboxes_Tick(object sender, EventArgs e)
        {
            if (checkBox_use_db.Checked)
            {
                textBox_host.Enabled = true;
                textBox_port.Enabled = true;
                textBox_user.Enabled = true;
                textBox_pwd.Enabled = true;
                textBox_database.Enabled = true;
                this.Refresh();
            }
            else
            {
                textBox_host.Enabled = false;
                textBox_port.Enabled = false;
                textBox_user.Enabled = false;
                textBox_pwd.Enabled = false;
                textBox_database.Enabled = false;
                this.Refresh();
            }
        }
    }
}

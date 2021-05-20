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
    public partial class Form_parse : Form
    {
        List<string> log_filenames = new List<string>(); // PARENT WINDOW ONLY
        List<string> log_files = new List<string>(); // PARENT WINDOW ONLY

        List<selection_list> selections = new List<selection_list>(); // 0 - curr; 1 - prev
        List<Color> selection_colors = new List<Color>(); // 0 - green; 1 - yellow

        string thisLogFile = null;
        public Form_parse()
        {
            mainInit();
            InitializeComponent();
        }

        public Form_parse(string filename, string logfile)
        {
            //parse
            thisLogFile = logfile;
            mainInit();
            InitializeComponent();
            //view filename
            this.Text = "REGEX-парсер   " + filename;
            richTextBox1.Text = thisLogFile;
        }

        private void mainInit()
        {
            //Color init
            selection_colors.Add(Color.Green);
            selection_colors.Add(Color.Yellow);
            //UI
            //comboBox_parsePattern.SelectedIndex = 1;
        }

        private void openToParseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //read file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Log files (*.log)|*.log";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                log_files.Clear();
                foreach (var file in openFileDialog1.FileNames)
                {
                    System.IO.StreamReader logfile = new System.IO.StreamReader(file);
                    if (logfile != null)
                    {
                        string log = logfile.ReadToEnd();

                        string ReplaceRegex = @"\r\n";
                        log = System.Text.RegularExpressions.Regex.Replace(log, ReplaceRegex, "\n");

                        log_filenames.Add(file);
                        log_files.Add(log);
                        logfile.Dispose();
                        logfile.Close();
                    }
                }
            }

            if (log_files.Count > 1)
            {
                //view filename
                this.Text = "REGEX-парсер    " + log_filenames[0];
                log_filenames.RemoveAt(0);
                //parse first
                thisLogFile = log_files[0];
                parselog(log_files[0]);
                //del first
                log_files.RemoveAt(0);
                foreach (var log in log_files)
                {
                    var temp = log_filenames[0];
                    System.Threading.Thread newParseThread = new System.Threading.Thread(() => Thread_form.runThreadedLogParser(temp, log));
                    newParseThread.Start();
                    log_filenames.RemoveAt(0);
                }
            }
            else if (log_files.Count == 1)
            {
                //view filename
                this.Text = "REGEX-парсер    " + log_filenames[0];
                log_filenames.RemoveAt(0);
                //parse first
                thisLogFile = log_files[0];
                parselog(log_files[0]);
            }
        }

        public void parselog(string logfile)
        {
            //  I. make selections
            // 1) read pattern
            string Regex = @"";
            switch (comboBox_parsePattern.SelectedIndex)
            {
                case 0:
                    if (!checkBox_timePick.Checked)
                    {
                        MessageBox.Show("Выберите время!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Regex = @"^.*\D([0-9]{1}:[0-5]{1}[0-9]{1}:[0-5]{1}[0-9]).*$|^.*([0-2]{1}[0-4]{1}:[0-5]{1}[0-9]{1}:[0-5]{1}[0-9]).*$";
                    selections.Insert(0, new selection_list(Regex_type.Variant_4));

                    break;
                case 1:
                    Regex = @"^.*(LAB).*$";
                    selections.Insert(0, new selection_list(Regex_type.LAB));
                    break;
                default:
                    MessageBox.Show("Выберите паттерн", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
            }
            // 2) find matches
            List<System.Text.RegularExpressions.Match> raw_matches = new List<System.Text.RegularExpressions.Match>();
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(logfile, Regex, System.Text.RegularExpressions.RegexOptions.Multiline))
            {
                raw_matches.Add(match);
            }

            if (comboBox_parsePattern.SelectedIndex == 0) //time check
            {
                TimeSpan interval_begin = dateTimePicker_begin.Value.TimeOfDay;
                TimeSpan interval_end = dateTimePicker_end.Value.TimeOfDay;

                foreach (var variant in raw_matches)
                {
                    TimeSpan time_check;
                    if (TimeSpan.TryParse(variant.Groups[1].Value, out time_check))
                    {
                        if ((interval_begin <= time_check) && (time_check <= interval_end))
                        {
                            selections[0].selections.Add(new selection_list.selection(variant, 1));
                        }
                    }
                    else if (TimeSpan.TryParse(variant.Groups[2].Value, out time_check))
                    {
                        if ((interval_begin <= time_check) && (time_check <= interval_end))
                        {
                            selections[0].selections.Add(new selection_list.selection(variant, 2));
                        }
                    }
                }
            }
            else
            {
                foreach (var variant in raw_matches)
                    selections[0].selections.Add(new selection_list.selection(variant, 1));
            }

            // check current-previous
            if (selections.Count > 2)
                while (selections.Count != 2)
                    selections.RemoveAt(2);

            //  II. draw
            if (comboBox_parsePattern.SelectedIndex == 0) //draw ip
                drawTB(richTextBox1, logfile, selections, selection_colors, true);
            else
                drawTB(richTextBox1, logfile, selections, selection_colors);
        }

        public void drawTB(RichTextBox rtb, string plaintext, List<selection_list> selections, List<Color> colors, bool ip = false)
        {
            richTextBox1.Text = plaintext;

            if (selections[0].list_type == Regex_type.Variant_4)
            {
                Clipboard.SetText("@192.168.0.154");
                for (int i = 0; i < selections[0].selections.Count; i++)
                {
                    var match = selections[0].selections[i];

                    rtb.Select(match.match.Groups[0].Index + match.match.Groups[0].Length, 0);
                    rtb.Paste();
                    List<selection_list> newColl;
                    recalcMatches(rtb.Text, selections, out newColl);
                    selections = newColl;
                }
            }

            int curprev = 0;
            foreach (var selection_list in selections)
            {
                //очередность
                foreach (var match in selection_list.selections)
                {
                    rtb.SelectionStart = match.match.Groups[match.group].Index;
                    rtb.SelectionLength = match.match.Groups[match.group].Length;
                    rtb.SelectionBackColor = colors[curprev];
                }
                curprev++;
            }
        }

        public void recalcMatches(string newText, List<selection_list> oldColl, out List<selection_list> newColl)
        {
            newColl = new List<selection_list>();
            foreach (var selection in oldColl)
            {
                string Regex = @"";
                switch (selection.list_type)
                {
                    case Regex_type.LAB:
                        Regex = @"^.*(LAB).*$";
                        break;
                    case Regex_type.Variant_4:
                        Regex = @"^.*\D([0-9]{1}:[0-5]{1}[0-9]{1}:[0-5]{1}[0-9]).*$|^.*([0-2]{1}[0-4]{1}:[0-5]{1}[0-9]{1}:[0-5]{1}[0-9]).*$";
                        break;
                }
                selection_list new_sel = new selection_list(selection.list_type);
                //find matches
                List<System.Text.RegularExpressions.Match> raw_matches = new List<System.Text.RegularExpressions.Match>();
                foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(newText, Regex, System.Text.RegularExpressions.RegexOptions.Multiline))
                {
                    raw_matches.Add(match);
                }

                if (selection.list_type == Regex_type.Variant_4) //time check
                {
                    TimeSpan interval_begin = dateTimePicker_begin.Value.TimeOfDay;
                    TimeSpan interval_end = dateTimePicker_end.Value.TimeOfDay;

                    foreach (var variant in raw_matches)
                    {
                        TimeSpan time_check;
                        if (TimeSpan.TryParse(variant.Groups[1].Value, out time_check))
                        {
                            if ((interval_begin <= time_check) && (time_check <= interval_end))
                            {
                                new_sel.selections.Add(new selection_list.selection(variant, 1));
                            }
                        }
                        else if (TimeSpan.TryParse(variant.Groups[2].Value, out time_check))
                        {
                            if ((interval_begin <= time_check) && (time_check <= interval_end))
                            {
                                new_sel.selections.Add(new selection_list.selection(variant, 2));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var variant in raw_matches)
                        new_sel.selections.Add(new selection_list.selection(variant, 1));
                }
                //paste updated
                newColl.Add(new_sel);
            }

            oldColl = newColl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //USING ONLY THIS LOG FILE
            thisLogFile = richTextBox1.Text;
            recalcMatches(thisLogFile, selections, out selections);
            parselog(thisLogFile);
        }

        public class selection_list
        {
            public selection_list()
            { }
            public selection_list(Regex_type list_type_)
            {
                this.list_type = list_type_;
            }
            public List<selection> selections = new List<selection>();
            public Regex_type list_type;


            public class selection
            {
                public selection(System.Text.RegularExpressions.Match match_, int group_)
                {
                    this.match = match_;
                    this.group = group_;
                }
                public System.Text.RegularExpressions.Match match;
                public int group;
            }
        }

        public enum Regex_type
        {
            Variant_4,
            LAB
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_timePick.Checked)
                groupBox_timepick.Enabled = true;
            else
                groupBox_timepick.Enabled = false;
        }
    }
}

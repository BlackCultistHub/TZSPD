using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.IO;

using PacketDotNet;
using SharpPcap;

using System.Timers;

namespace Lab1
{
    public partial class Form4_tz : Form
    {
        public ICaptureDevice adaptor = null;
        bool adaptor_opened = false;

        //capture
        Thread capture_thread = null;
        System.Net.IPAddress Capture_self_ip = null;
        System.Net.IPAddress Capture_target_ip = null;

        //ARP
        System.Net.IPAddress ARP_asker_ip = null;
        System.Net.IPAddress ARP_target_ip = null;
        System.Net.NetworkInformation.PhysicalAddress destinationHW = null;
        System.Net.NetworkInformation.PhysicalAddress sourceHW = null;
        //vars
        public int packets_sent = 0;
        public HiddenMessage recieve = new HiddenMessage();

        //LOG
        public List<string> log = new List<string>();
        public delegate void UpdateLogBoxDelegate();

        //fake payload
        string fakePayloadString = "Excepteur sint occaecat cupidatat non proident, consectetur adipiscing elit, qui dolorem eum fugiat, quo voluptas nulla pariatur. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Sed ut perspiciatis, quia voluptas sit, aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos, qui ratione voluptatem sequi nesciunt, neque porro quisquam est, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem! Lorem ipsum dolor sit amet, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat? Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat? Ut enim ad minim veniam, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, nisi ut aliquid ex ea commodi consequatur! Nemo enim ipsam voluptatem, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, qui dolorem eum fugiat, quo voluptas nulla pariatur? Quis autem vel eum iure reprehenderit, quis nostrum exercitationem ullam corporis suscipit laboriosam, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minima veniam, qui blanditiis praesentium voluptatum deleniti atque corrupti, quos dolores et quas molestias excepturi sint, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Itaque earum rerum hic tenetur a sapiente delectus, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Lorem ipsum dolor sit amet, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, sunt in culpa qui officia deserunt mollit anim id est laborum. Nemo enim ipsam voluptatem, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo! Et harum quidem rerum facilis est et expedita distinctio, quis nostrum exercitationem ullam corporis suscipit laboriosam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat? Ut enim ad minima veniam, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga! Ut enim ad minima veniam, consectetur adipiscing elit, quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt, explicabo. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Sed ut perspiciatis, unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam eaque ipsa, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Lorem ipsum dolor sit amet, nam libero tempore, cum soluta nobis est eligendi optio, cumque nihil impedit, quo minus id, quod maxime placeat, facere possimus, omnis voluptas assumenda est, omnis dolor repellendus? Itaque earum rerum hic tenetur a sapiente delectus, qui in ea voluptate velit esse, quam nihil molestiae consequatur, vel illum, obcaecati cupiditate non provident, similique sunt in culpa, qui officia deserunt mollitia animi, id est laborum et dolorum fuga? Itaque earum rerum hic tenetur a sapiente delectus, qui blanditiis praesentium voluptatum deleniti atque corrupti, quos dolores et quas molestias excepturi sint, facere possimus, omnis voluptas assumenda est, omnis dolor repellendus! Excepteur sint occaecat cupidatat non proident, qui in voluptate velit esse, quam nihil molestiae consequatur, vel illum, qui dolorem ipsum, quia dolor sit, amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt, ut labore et dolore magnam aliquam quaerat voluptatem! Et harum quidem est voluptatum deleniti atque corrupt.";
        public void InvokeUpdateLogBox()
        {
            logBox.Text = "";
            foreach (string line in log)
            {
                logBox.Text += line + Environment.NewLine;
            }
        }
        public Form4_tz()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            //get adapters
            print_adapters();

            label_my_ip.Text = "127.0.0.1";
            label_my_ip.Refresh();

            timer1.Start();
            timer_check_message.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_msg.TextLength >= 256)
                {
                    MessageBox.Show("Сообщение не может превышать длину в 255 символов!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                for (int i = 0; i < textBox_msg.TextLength; i++)
                {
                    if (textBox_msg.Text[i] > 256)
                    {
                        MessageBox.Show("Могут использоваться только символы таблицы ASCII!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                //read ip
                System.Net.IPAddress ipDestinationAddress;
                if (!System.Net.IPAddress.TryParse(textBox_reciever_ip.Text, out ipDestinationAddress))
                {
                    throw new Exception("IP-адрес получателя имеет неверный формат!");
                }
                ARP_target_ip = ipDestinationAddress;
                //:::::::::::::::::::::::::::::OVERALL PREPARE
                
                //::::::::::::::::::::::::::::::DEFINE MAC
                //1. prepare reciever
                
                //2. init recieve
                if (!adaptor_opened)
                {
                    adaptor_opened = true;
                    adaptor = getCurrentAdaptor();
                    adaptor.Open(DeviceMode.Promiscuous);
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(ARP_recieve_handler);
                    capture_thread = new Thread(adaptor.Capture);
                    capture_thread.Start();
                }
                else
                {
                    adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(ARP_recieve_handler);
                }
                ARP_asker_ip = getDeviceIpv4Addr(adaptor);
                //3. send request
                ARP(ipDestinationAddress, adaptor);
                //4. wait for response
                while (destinationHW == null) { }
                adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(ARP_recieve_handler);
                adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                //LOCK CONTROLS
                textBox_reciever_ip.ReadOnly = true;
                textBox_reciever_ip.Refresh();
                button_send.Enabled = false;
                button_send.Refresh();
                //:::::::::::::::::::::::::::::::SEND
                packets_sent = 0;
                recieve.Clear();
                label_packets_recieved.Text = recieve.packets_recieved.ToString();
                var ipSourceAddress = getDeviceIpv4Addr(adaptor);

                List<IPv4Packet> packets = new List<IPv4Packet>();
                List<int> keyTable = new List<int>();

                var rnd = new Random();

                for (int i = 0; i < textBox_msg.Text.Length; i++)
                    keyTable.Add(rnd.Next(256, 512));  //ASCII guarantee

                List<int> values = new List<int>();

                for (int i = 0; i < textBox_msg.Text.Length; i++)
                    values.Add(((byte)textBox_msg.Text[i]) + keyTable[i]);

                //chain N + table + vals
                List<int> chain = new List<int>();
                chain.Add(keyTable.Count);
                chain.AddRange(keyTable.ToArray());
                chain.AddRange(values.ToArray());

                int pkg_index = 0;
                if (checkBox_fake_payload.Checked) //USE FAKE PAYLOAD
                {
                    //make fake payload file
                    File.WriteAllText("fake_udp_payload", fakePayloadString);
                    byte[] fakePayload = File.ReadAllBytes("fake_udp_payload");
                    int fakePayload_pointer = 0;

                    int fakePort = 0;
                    if (!int.TryParse(textBox_fakePort.Text, out fakePort))
                        throw new Exception("Фейк-порт имеет неверный формат!");
                    if (fakePort > 65535 || fakePort < 0)
                        throw new Exception("Фейк-порт должен находится в диапазоне от 0 до 65535!");
                    for (int i = 0; i < chain.Count; i++)
                    {
                        //UDP
                        var UDPpacket = new UdpPacket((ushort)fakePort, (ushort)fakePort);
                        List<byte> Payload = new List<byte>();
                        Payload.Add((byte)(0xF0));
                        Payload.Add((byte)(0x3F));
                        Payload.Add((byte)(0xF0));
                        for (int j = 0; j < chain[i]; j++)
                        {
                            if (fakePayload_pointer == fakePayload.Length)
                                fakePayload_pointer = 0;
                            //Payload.Add((byte)rnd.Next(0, 255));
                            Payload.Add(fakePayload[fakePayload_pointer]);
                            fakePayload_pointer++;
                        }
                        UDPpacket.PayloadData = Payload.ToArray();
                        //IP
                        var IPpacket = new IPv4Packet(ipSourceAddress, ipDestinationAddress);
                        IPpacket.TypeOfService = pkg_index;
                        IPpacket.PayloadPacket = UDPpacket;
                        //Ethernet
                        var EthernetPacket = new EthernetPacket(sourceHW,
                                destinationHW,
                                EthernetType.IPv4);
                        EthernetPacket.PayloadPacket = IPpacket;
                        UDPpacket.Checksum = UDPpacket.CalculateUdpChecksum();
                        //SEND
                        adaptor.SendPacket(EthernetPacket);
                        pkg_index++;
                    }
                    if (fakePayload_pointer != fakePayload.Length) //send file till end
                    {

                        while (fakePayload_pointer != fakePayload.Length)
                        {
                            //UDP
                            var UDPpacket = new UdpPacket((ushort)fakePort, (ushort)fakePort);
                            List<byte> Payload = new List<byte>();
                            Payload.Add((byte)(0xF0));
                            Payload.Add((byte)(0x3F));
                            Payload.Add((byte)(0xF0));
                            int fakeCodedSymbol = rnd.Next(256, 512) + rnd.Next(0, 255);
                            for (int j = 0; j < fakeCodedSymbol; j++)
                            {
                                if (fakePayload_pointer == fakePayload.Length)
                                    break;
                                //Payload.Add((byte)rnd.Next(0, 255));
                                Payload.Add(fakePayload[fakePayload_pointer]);
                                fakePayload_pointer++;
                            }
                            UDPpacket.PayloadData = Payload.ToArray();
                            //IP
                            var IPpacket = new IPv4Packet(ipSourceAddress, ipDestinationAddress);
                            IPpacket.TypeOfService = pkg_index;
                            IPpacket.PayloadPacket = UDPpacket;
                            //Ethernet
                            var EthernetPacket = new EthernetPacket(sourceHW,
                                    destinationHW,
                                    EthernetType.IPv4);
                            EthernetPacket.PayloadPacket = IPpacket;
                            UDPpacket.Checksum = UDPpacket.CalculateUdpChecksum();
                            //SEND
                            adaptor.SendPacket(EthernetPacket);
                            pkg_index++;
                        }
                    }
                }
                else //USE RAW IP-PACKETS
                {
                    //N + table packages
                    for (int i = 0; i < chain.Count; i++)
                    {
                        packets.Add(new IPv4Packet(ipSourceAddress, ipDestinationAddress));
                        //write index
                        packets[packets.Count - 1].TypeOfService = pkg_index;
                        pkg_index++;
                        //write value (totalLen - headerLen)
                        List<byte> Payload = new List<byte>();
                        for (int j = 0; j < chain[i]; j++)
                        {
                            Payload.Add((byte)rnd.Next(0, 255));
                        }
                        packets[packets.Count - 1].PayloadData = Payload.ToArray();
                    }
                    foreach (IPv4Packet packet in packets)
                    {
                        var ethernetPacket = new EthernetPacket(sourceHW,
                            destinationHW,
                            EthernetType.IPv4);
                        ethernetPacket.PayloadPacket = packet;

                        // Send the packet out the network device
                        adaptor.SendPacket(ethernetPacket);
                    }
                }



                packets_sent = pkg_index;
                label_packets_sent.Text = pkg_index.ToString();

                richTextBox_history.AppendText(DateTime.Now + " ", Color.Gray);
                richTextBox_history.AppendText("Отправлено ", Color.Green);
                richTextBox_history.AppendText("(");
                richTextBox_history.AppendText(ARP_asker_ip.ToString(), Color.DarkGoldenrod);
                richTextBox_history.AppendText("): " + textBox_msg.Text + Environment.NewLine);

                //UNLOCK CONTROLS
                textBox_reciever_ip.ReadOnly = false;
                textBox_reciever_ip.Refresh();
                button_send.Enabled = true;
                button_send.Refresh();
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB4_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB4_TZSPD", "LAB4_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                //UNLOCK CONTROLS
                textBox_reciever_ip.ReadOnly = false;
                textBox_reciever_ip.Refresh();
                button_send.Enabled = true;
                button_send.Refresh();
            }
        }

        private void timer_check_message_Tick(object sender, EventArgs e)
        {
            if (!recieve._locked)
            {
                if (recieve._updated)
                {
                    parseMessage();
                    recieve.Clear();
                }
            }
        }
        private void parseMessage()
        {
            try
            {
                recieve._updated = false;
                label_packets_recieved.Text = recieve.packets_recieved.ToString();
                label_packets_recieved.Refresh();
                //cut copies
                recieve.cutCopyPackets();

                //sort
                bubbleSort(recieve.packetList);

                //check packets
                int charsInMessage = recieve.packetList[0].value;
                int truePacketsLen = 1 + (charsInMessage * 2);

                int[] checkTable = new int[truePacketsLen];
                for (int i = 0; i < truePacketsLen; i++)
                {
                    int index = recieve.packetList[i].index;
                    checkTable[index]++;
                }
                int losses = 0;
                foreach (int ind in checkTable)
                {
                    if (ind == 0)
                    {
                        losses++;
                    }
                }
                if (losses != 0)
                {
                    throw new Exception("Пакеты прибыли с потерями! Потеряно пакетов: " + losses);
                }

                //export table
                List<int> keyTable = new List<int>();
                int keyTableLen = recieve.packetList[0].value;
                for (int i = 1; i < keyTableLen + 1; i++)
                    keyTable.Add(recieve.packetList[i].value);

                //export characters
                List<int> chars = new List<int>();
                for (int i = 1 + keyTableLen; i < 1 + (keyTableLen*2); i++)
                    chars.Add(recieve.packetList[i].value);

                //extract symbols
                List<char> symbols = new List<char>();
                for (int i = 0; i < chars.Count; i++)
                    symbols.Add((char)(chars[i] - keyTable[i]));

                //print data
                string message = "";
                foreach (var symb in symbols)
                {
                    message += symb;
                }
                textBox_recieve.Text = message;

                richTextBox_history.AppendText(DateTime.Now + " ", Color.Gray);
                richTextBox_history.AppendText("Получено ", Color.Red);
                richTextBox_history.AppendText("(");
                richTextBox_history.AppendText(Capture_target_ip.ToString(), Color.DarkGoldenrod);
                richTextBox_history.AppendText("): " + message + Environment.NewLine);
                Capture_target_ip = null;
                recieve.Clear();
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB4_TZSPD: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB4_TZSPD", "LAB4_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                recieve.Clear();
            }
        }
        private void bubbleSort(List<HiddenMessage.CustomPacket> packetList)
        {
            HiddenMessage.CustomPacket temp;
            for (int j = 0; j <= packetList.Count - 2; j++)
            {
                for (int i = 0; i <= packetList.Count - 2; i++)
                {
                    if (packetList[i].index > packetList[i + 1].index)
                    {
                        temp = packetList[i + 1];
                        packetList[i + 1] = packetList[i];
                        packetList[i] = temp;
                    }
                }
            }
        }

        System.Timers.Timer LockTimer = null;
        public void elapseLock(Object source, ElapsedEventArgs e)
        {
            recieve._locked = false;
            capturing_udp = false;
        }

        //bool begin_recieve = false;
        bool capturing_udp = false;
        private void device_OnPacketArrival(object sender, CaptureEventArgs packet)
        {
            var ethPacket = PacketDotNet.Packet.ParsePacket(packet.Packet.LinkLayerType, packet.Packet.Data);

            var parsedEthernetPacket = (EthernetPacket)ethPacket;

            if (parsedEthernetPacket.Type == EthernetType.IPv4)
            {
                var ipv4Packet = (IPv4Packet)parsedEthernetPacket.PayloadPacket;
                if ((ipv4Packet.SourceAddress.Equals(Capture_target_ip) || Capture_target_ip == null) && ipv4Packet.DestinationAddress.Equals(Capture_self_ip))
                {
                    if (!recieve._locked) //IP
                    {
                        if (!ipv4Packet.HasPayloadPacket)
                        {
                            recieve._locked = true;
                            Capture_target_ip = ipv4Packet.SourceAddress;
                            int index = ipv4Packet.TypeOfService;
                            int totalLen = ipv4Packet.TotalLength;
                            recieve.addPacket(index, 20, totalLen);
                            recieve._updated = true;
                            LockTimer.Stop();
                            LockTimer.Interval = 1500;
                            LockTimer.Start();
                        }
                        else if (ipv4Packet.Protocol == ProtocolType.Udp)//UDP
                        {
                            var udpPacket = (UdpPacket)ipv4Packet.PayloadPacket;
                            if (!udpPacket.HasPayloadPacket)
                            {
                                var payloadData = udpPacket.PayloadData;
                                if (payloadData.Length >= 3)
                                {
                                    //check message signature 0xF0 0x3F 0xF0
                                    if ((payloadData[0] == 0xF0) &&
                                    (payloadData[1] == 0x3F) &&
                                    (payloadData[2] == 0xF0))
                                    {
                                        capturing_udp = true;
                                        recieve._locked = true;
                                        Capture_target_ip = ipv4Packet.SourceAddress;
                                        int index = ipv4Packet.TypeOfService;
                                        int totalLen = ipv4Packet.TotalLength;
                                        recieve.addPacket(index, 28+3, totalLen);
                                        recieve._updated = true;
                                        LockTimer.Stop();
                                        LockTimer.Interval = 1500;
                                        LockTimer.Start();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((!ipv4Packet.HasPayloadPacket) && (!capturing_udp)) //IP
                        {
                            int index = ipv4Packet.TypeOfService;
                            int totalLen = ipv4Packet.TotalLength;
                            recieve.addPacket(index, 20, totalLen);
                            recieve._updated = true;
                            LockTimer.Stop();
                            LockTimer.Interval = 1500;
                            LockTimer.Start();
                        }
                        else if (ipv4Packet.Protocol == ProtocolType.Udp)//UDP
                        {
                            var udpPacket = (UdpPacket)ipv4Packet.PayloadPacket;
                            if (!udpPacket.HasPayloadPacket)
                            {
                                var payloadData = udpPacket.PayloadData;
                                if (payloadData.Length >= 3)
                                {
                                    //check message signature 0xF0 0x3F 0xF0
                                    if ((payloadData[0] == 0xF0) &&
                                    (payloadData[1] == 0x3F) &&
                                    (payloadData[2] == 0xF0))
                                    {
                                        int index = ipv4Packet.TypeOfService;
                                        int totalLen = ipv4Packet.TotalLength;
                                        recieve.addPacket(index, 28+3, totalLen);
                                        recieve._updated = true;
                                        LockTimer.Stop();
                                        LockTimer.Interval = 1500;
                                        LockTimer.Start();
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
        }

        public class HiddenMessage
        {
            public HiddenMessage() { _locked = false; _updated = false; }

            public bool _locked { get; set; }
            public bool _updated { get; set; }
            public void addPacket(int TypeOfService, int HeaderLen, int TotalLen)
            {
                this.packets_recieved++;
                this.packetList.Add(new HiddenMessage.CustomPacket(TypeOfService, HeaderLen, TotalLen));
            }
            public void cutCopyPackets()
            {
                for (int i = 0; i < packetList.Count; i++)
                {
                    for (int j = i + 1; j < packetList.Count; j++)
                    {
                        if (packetList[i].Equals(packetList[j]))
                        {
                            packetList.RemoveAt(j);
                            j--;
                        }
                    }

                }
            }
            public void Clear()
            {
                this.packetList.Clear();
                this.packets_recieved = 0;
            }
            public volatile List<CustomPacket> packetList = new List<CustomPacket>();
            public volatile int packets_recieved = 0;
            public class CustomPacket
            {
                public CustomPacket(int TypeOfService, int HeaderLen, int TotalLen)
                {
                    this.index = TypeOfService;
                    this.value = TotalLen - HeaderLen;
                }
                public bool Equals(CustomPacket packet)
                {
                    if ((this.index == packet.index) && (this.value == packet.value))
                        return true;
                    else
                        return false;
                }

                public int index = 0; //8bit index
                public int value = 0; //16bit
            }
        }
        public void ARP_recieve_handler(object sender, CaptureEventArgs packet)
        {
            var ethPacket = PacketDotNet.Packet.ParsePacket(packet.Packet.LinkLayerType, packet.Packet.Data);
            var parsedEthPacket = (EthernetPacket)ethPacket;

            if (parsedEthPacket.Type == EthernetType.Arp)
            {
                var arpPacket = (ArpPacket)parsedEthPacket.PayloadPacket;
                if (arpPacket.TargetProtocolAddress.Equals(ARP_asker_ip) && arpPacket.SenderProtocolAddress.Equals(ARP_target_ip))
                {
                    sourceHW = arpPacket.TargetHardwareAddress;
                    destinationHW = arpPacket.SenderHardwareAddress;
                }
            }
        }

        public static void ARP(System.Net.IPAddress ipAddress, ICaptureDevice device)
        {
            if (ipAddress == null)
                throw new Exception("ARP IP address Cannot be null");
            var ethernetPacket = new PacketDotNet.EthernetPacket(device.MacAddress, System.Net.NetworkInformation.PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"), EthernetType.Arp);

            var selfIp = getDeviceIpv4Addr(device);
            if (selfIp == null && device.Description == "Adapter for loopback traffic capture") //suggest loopback
                selfIp = new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 });

            var arpPacket = new PacketDotNet.ArpPacket(PacketDotNet.ArpOperation.Request, System.Net.NetworkInformation.PhysicalAddress.Parse("00-00-00-00-00-00"), ipAddress, device.MacAddress, selfIp);
            ethernetPacket.PayloadPacket = arpPacket;

            device.SendPacket(ethernetPacket);
        }

        public static System.Net.IPAddress getDeviceIpv4Addr(ICaptureDevice device)
        {
            var castedDevice = (SharpPcap.LibPcap.LibPcapLiveDevice)device;
            foreach (var address in castedDevice.Addresses)
            {
                if (address.Addr.ipAddress != null)
                    if (address.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return address.Addr.ipAddress;
            }
            return null;
        }


        private void print_adapters()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            for (int i = 0; i < devices.Count; i++)
            {
                string name = devices[i].Description;
                comboBox_adapters.Items.Add(name + Environment.NewLine);
            }
            comboBox_adapters.SelectedItem = comboBox_adapters.Items[comboBox_adapters.Items.Count - 1];
        }

        private ICaptureDevice getCurrentAdaptor()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            if (comboBox_adapters.SelectedIndex < devices.Count)
                return devices[comboBox_adapters.SelectedIndex];
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label_packets_recieved.Text = "0";
            label_packets_recieved.Refresh();
            recieve.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var myIP = getDeviceIpv4Addr(getCurrentAdaptor());
            string myIPStr = myIP == null ? "127.0.0.1" : myIP.ToString();
            label_my_ip.Text = myIPStr;
            label_my_ip.Refresh();
            if (checkBox_fake_payload.Checked)
                textBox_fakePort.ReadOnly = false;
            else
                textBox_fakePort.ReadOnly = true;
            textBox_fakePort.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                label_packets_recieved.Text = "0";
                label_packets_recieved.Refresh();
                button_begin_recieve.Enabled = false;
                button_begin_recieve.Refresh();
                button_end_recieve.Enabled = true;
                button_end_recieve.Refresh();

                recieve.Clear();

                if (!adaptor_opened)
                {
                    adaptor_opened = true;
                    adaptor = getCurrentAdaptor();
                    adaptor.Open(DeviceMode.Promiscuous);
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                    capture_thread = new Thread(adaptor.Capture);
                    capture_thread.Start();
                }
                else
                {
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                }

                Capture_self_ip = getDeviceIpv4Addr(adaptor);

                LockTimer = new System.Timers.Timer(1500);
                var reader_thread = Thread.CurrentThread;
                LockTimer.Elapsed += (senderE, Ee) => elapseLock(senderE, Ee);
                LockTimer.Start();
                
                pictureBox_led_recieve.Image = Lab1.Properties.Resources.LED_green;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB4_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB4_TZSPD", "LAB4_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        //SCANNER
        public System.Net.IPAddress ARPSCAN_sender_ip = null;
        public List<System.Net.IPAddress> ARPSCAN_target_ips = new List<System.Net.IPAddress>();
        public List<System.Net.NetworkInformation.PhysicalAddress> ARPSCAN_destinationHWs = new List<System.Net.NetworkInformation.PhysicalAddress>();

        public void ARPSCAN_recieve_handler(object sender, CaptureEventArgs packet)
        {
            var ethPacket = PacketDotNet.Packet.ParsePacket(packet.Packet.LinkLayerType, packet.Packet.Data);
            var parsedEthPacket = (EthernetPacket)ethPacket;

            if (parsedEthPacket.Type == EthernetType.Arp)
            {
                var arpPacket = (ArpPacket)parsedEthPacket.PayloadPacket;
                if (arpPacket.TargetProtocolAddress.Equals(ARPSCAN_sender_ip))
                {

                    ARPSCAN_target_ips.Add(arpPacket.SenderProtocolAddress);
                    ARPSCAN_destinationHWs.Add(arpPacket.SenderHardwareAddress);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!adaptor_opened)
                {
                    adaptor_opened = true;
                    adaptor = getCurrentAdaptor();
                    adaptor.Open(DeviceMode.Promiscuous);
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(ARPSCAN_recieve_handler);
                    capture_thread = new Thread(adaptor.Capture);
                    capture_thread.Start();
                }
                else
                {
                    adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                    adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(ARPSCAN_recieve_handler);
                }
                ARPSCAN_sender_ip = getDeviceIpv4Addr(adaptor);

                //get subnet
                System.Net.IPAddress subnet = null;
                
                //find Ipv4 of current device
                var devAddrs = ((SharpPcap.LibPcap.LibPcapLiveDevice)adaptor).Addresses;
                SharpPcap.LibPcap.PcapAddress gotAddr = null;
                foreach (var addr in devAddrs)
                {
                    if (addr.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        gotAddr = addr;
                        break;
                    }
                }
                if (gotAddr == null) //exit
                    return;
                //assign subnet
                if (gotAddr.Netmask.ipAddress == null) //suggest 0.0.0.0 mask
                    subnet = new System.Net.IPAddress(0);
                else
                    subnet = gotAddr.Netmask.ipAddress;
                //prepare scan
                byte[] byte_currentIp = new byte[4];
                Array.Clear(byte_currentIp, 0, byte_currentIp.Length);
                var byte_subnet = subnet.GetAddressBytes();
                if (byte_subnet[1] == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Операция займёт очень продолжительное время!\nВы уверены, что хотите продолжить?", "Предупреждение", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                        return;
                }

                //1. prepare reciever

                
                var byte_myIp = ARPSCAN_sender_ip.GetAddressBytes();

                for (int i = 0; i < byte_currentIp.Length; i++)
                    if (byte_subnet[i] == 255)
                        byte_currentIp[i] = byte_myIp[i];


                //open window
                var scanWindow = new Form4_tz_scan();
                scanWindow.Show();
                scanWindow.Refresh();


                
                //3. send requests

                //send scanning requests
                while (true)
                {
                    System.Net.IPAddress destinationIP = new System.Net.IPAddress(byte_currentIp);

                    ARP(destinationIP, adaptor);

                    byte_currentIp[3]++;
                    if (byte_currentIp[3] == 255)
                    {
                        if (byte_subnet[2] != 255)
                        {
                            byte_currentIp[2]++;
                            if (byte_currentIp[2] == 255)
                            {
                                if (byte_subnet[1] != 255)
                                {
                                    byte_currentIp[1]++;
                                    if (byte_currentIp[1] == 255)
                                    {
                                        if (byte_subnet[0] != 255)
                                        {
                                            byte_currentIp[0]++;
                                            if (byte_currentIp[0] == 255)
                                                break; //FULL Ip search end
                                            else
                                                byte_currentIp[1] = 0;
                                        }
                                        else
                                            break;
                                    }
                                    else
                                        byte_currentIp[2] = 0;
                                }
                                else
                                    break;
                            }
                            else
                                byte_currentIp[3] = 0;
                        }
                        else
                            break;
                    }
                }
                //4. wait for response a bit and close
                Thread.Sleep(2000);
                adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(ARPSCAN_recieve_handler);
                adaptor.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);


                //cut copies
                List<System.Net.IPAddress> copyips = new List<System.Net.IPAddress>();
                copyips.AddRange(ARPSCAN_target_ips.ToArray());
                for (int i = 0; i < copyips.Count; i++)
                {
                    var testedElem = copyips[i];
                    for (int j = i + 1; j < copyips.Count; j++)
                    {
                        if (testedElem == copyips[j])
                        {
                            copyips.RemoveAt(j);
                            ARPSCAN_target_ips.RemoveAt(j);
                            ARPSCAN_destinationHWs.RemoveAt(j);
                            i--;
                        }
                    }
                }

                //cast data to form
                for (int i = 0; i < ARPSCAN_target_ips.Count; i++)
                    scanWindow.addRow(ARPSCAN_target_ips[i].ToString(), ARPSCAN_destinationHWs[i].ToString());

                ARPSCAN_target_ips.Clear();
                ARPSCAN_destinationHWs.Clear();

                //end scan
                scanWindow.SetStatus("Сканирование завершено.");
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB4_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB4_TZSPD", "LAB4_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void button_end_recieve_Click(object sender, EventArgs e)
        {
            adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);

            LockTimer.Stop();

            button_begin_recieve.Enabled = true;
            button_begin_recieve.Refresh();
            button_end_recieve.Enabled = false;
            button_end_recieve.Refresh();
            pictureBox_led_recieve.Image = Lab1.Properties.Resources.LED_red;
        }

        private void comboBox_adapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //end recieve
                try
                {
                    if (capture_thread != null)
                        if (capture_thread.IsAlive)
                            capture_thread.Abort();
                }
                catch { }
                button_begin_recieve.Enabled = true;
                button_begin_recieve.Refresh();
                button_end_recieve.Enabled = false;
                button_end_recieve.Refresh();
                if (adaptor != null)
                    adaptor.OnPacketArrival -= new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                adaptor = null;
                adaptor_opened = false;
                pictureBox_led_recieve.Image = Lab1.Properties.Resources.LED_red;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB4_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB4_TZSPD", "LAB4_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void менюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Form_start();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void сохранитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "lab4_tz.log";
            saveFileDialog1.Filter = "Log files (*.log)|*.log";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter logfile = new StreamWriter(saveFileDialog1.OpenFile());
                if (logfile != null)
                {
                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    foreach (string line in log)
                    {
                        logfile.WriteLine(line);
                    }
                    logfile.Dispose();
                    logfile.Close();
                }
            }
        }

        private void загрузитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Log files (*.log)|*.log";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader logfile = new StreamReader(openFileDialog1.OpenFile());
                if (logfile != null)
                {
                    string line;
                    while ((line = logfile.ReadLine()) != null)
                    {
                        log.Add(line);
                    }
                    logfile.Dispose();
                    logfile.Close();
                }
            }
            Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
        }

        private void сохранитьИсториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = DateTime.Now.ToString().Replace(':', '-').Replace(' ', '_') + "_history.txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.OpenFile());
                if (file != null)
                {
                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    string contents = "";
                    foreach (char symbol in richTextBox_history.Text)
                    {
                        contents += symbol;
                    }
                    file.WriteLine(contents);
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void загрузитьИториюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_history.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.OpenFile());
                if (file != null)
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        richTextBox_history.Text += line + Environment.NewLine;
                    }
                    file.Dispose();
                    file.Close();
                }
            }
            richTextBox_history.Refresh();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void Form4_tz_FormClosing(object sender, FormClosingEventArgs e)
        {
            //kill thread
            try
            {
                if (capture_thread != null)
                    if (capture_thread.IsAlive)
                        capture_thread.Abort();
            }
            catch { }
        }
    }
}

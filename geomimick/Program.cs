using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace geomimick
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostsFile = @"C:\Windows\System32\drivers\etc\hosts";
            File.Copy(hostsFile, hostsFile + "_backup", true);
            StringBuilder sb = new StringBuilder();
            sb.Append(File.ReadAllText(hostsFile));
            TcpListener tcpListen = new TcpListener(System.Net.IPAddress.Parse("127.0.0.51"), 80);
            sb.AppendLine($"127.0.0.51 www.eversim.com");
            Console.WriteLine("Writing new hostfile");
            using (StreamWriter sw = new StreamWriter(new FileStream(hostsFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)))
                sw.Write(sb.ToString());
            tcpListen.Start();
            Console.WriteLine("TCP Server up; you can start the game now: Reminder to use this ID: 76561199095485707");
            TcpClient client = tcpListen.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            byte[] bf = new byte[client.ReceiveBufferSize];
            string cl = "_GEOLREG_EXIT=253071\n_GEOLREG_DATA=61Q8SURLS4LJ1PDE09JLSCB0M2Y3WTSNQGI0TK3MDTN5TI47VIACQ\n\n_GEOLREG_HEXA=8707b110b8ab0f1e0895712ab1de04de676b6bdea7cd51abeaaba5e76724fc4b";
            byte[] cc = Encoding.Default.GetBytes(cl);
            string headers = $"HTTP/1.1 200 OK\nDate: {DateTime.Now} GMC\nServer: Apache\nX-Powered-By: PleskLin\nContent-Length: {cc.Length}\nContent-Type: text/html";
            cl = $"{headers}\n\n{cl}";
            byte[] total = Encoding.Default.GetBytes(cl);
            string ab = Encoding.ASCII.GetString(total);
            ns.Write(total, 0, total.Length);
            Console.WriteLine("Returning to original hostfile.");
            File.Copy(hostsFile + "_backup", hostsFile, true);
            File.Delete(hostsFile + "_backup");
            Console.WriteLine("\"Crack\" Complete; you can now close this window.");
            Console.ReadLine();
        }
    }
}

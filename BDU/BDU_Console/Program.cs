using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BDU_Console
{
    class Program
    {
        public static int ThreadCount;
        public static string Address;
        public static int IDPadding;
        public static int StartID;
        public static int EndID;
        public static Queue<string> ID_Que = new Queue<string>();
        public static List<Task> tasks = new List<Task>();
        public static List<string> Failed_Images = new List<string>();

        /// <summary>
        /// Assembly Directory
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "BDU Console";
            ResetTop();
            Thread.Sleep(5000);

            Set_ThreadCount();
            Set_Address();
            Set_IDPadding();
            Set_StartID();
            Set_EndID();

            ResetTop();
            Console.WriteLine("Setting up... Please Wait");
            for (int i = StartID; i <= EndID; i++)
            {
                string str = Address.Replace("[]", (i.ToString().PadLeft(IDPadding, '0')));
                ID_Que.Enqueue(str);
                Console.WriteLine("Queue <- " + Path.GetFileName(new Uri(str).AbsolutePath));
            }
            Directory.CreateDirectory(AssemblyDirectory + "/Images");

            Console.WriteLine("Starting Download!");

            for (int i = 1; i <= ThreadCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => DownloadImage()));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Files downloaded with " + Failed_Images.Count + " failed files");
            Console.WriteLine("Failed Files");
            foreach (string file in Failed_Images)
                Console.WriteLine("Failed -> " + file);
            Console.WriteLine("");
            if (Failed_Images.Count >= 1)
                Console.WriteLine("Run Script again to download the " + Failed_Images.Count + " failed files");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Set_ThreadCount()
        {
            ResetTop();
            Console.WriteLine("That is not a valid number above 1");
            Console.Write("Enter Thread Count: ");
            try { ThreadCount = Convert.ToInt32(Console.ReadLine()); }
            catch
            {
                Set_ThreadCount();
            }
        }
        public static void Set_Address()
        {
            ResetTop();
            Console.WriteLine("When entering address, replace the photo ID with \"[]\"");
            Console.Write("Enter File Address: ");
            Address = Console.ReadLine();
        }
        public static void Set_IDPadding()
        {
            ResetTop();
            Console.WriteLine("That is not a valid number");
            Console.Write("ID Padding Length: ");
            try { IDPadding = Convert.ToInt32(Console.ReadLine()); }
            catch
            {
                Set_IDPadding();
            }
        }
        public static void Set_StartID()
        {
            ResetTop();
            Console.WriteLine("That is not a valid number");
            Console.Write("Start Image ID: ");
            try { StartID = Convert.ToInt32(Console.ReadLine()); }
            catch
            {
                Set_StartID();
            }
        }
        public static void Set_EndID()
        {
            ResetTop();
            Console.WriteLine("That is not a valid number");
            Console.Write("Last Image ID: ");
            try { EndID = Convert.ToInt32(Console.ReadLine()); }
            catch
            {
                Set_EndID();
            }
        }

        public static void ResetTop()
        {
            Console.Clear();
            Console.WriteLine("Bulk Image Downloader v0.0.1");
            Console.WriteLine("By: Tasman Leach (AssaultBird2454)");
            Console.WriteLine("© Tasman Leach 2017");
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine("");
        }

        public static void DownloadImage()
        {
            WebClient client = new WebClient();
            while (ID_Que.Count >= 1)
            {
                string str = ID_Que.Dequeue();
                string filename = Path.GetFileName(new Uri(str).AbsolutePath);

                try
                {
                    if (System.IO.File.Exists(AssemblyDirectory + "/Images/" + filename))
                    {
                        Console.WriteLine("Skip -> " + filename);
                        continue;
                    }

                    client.DownloadFile(str, AssemblyDirectory + "/Images/" + filename);
                    Console.WriteLine("Get -> " + filename);
                }
                catch (Exception ex)
                {
                    Failed_Images.Add(filename);
                    Console.WriteLine("Error -> " + ex.ToString());
                }
            }
        }
    }
}

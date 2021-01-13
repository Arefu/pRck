using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace pRck
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var Client = new WebClient();
            do
            {
                var Json = Client.DownloadString("https://a.4cdn.org/hc/catalog.json");
                var Threads = JsonConvert.DeserializeObject<List<Root>>(Json);
                foreach (var Thread in Threads)
                {
                    foreach (var Page in Thread.threads)
                    {
                        if (Page.sub == null)
                            continue;

                        if (Page.sub.ToLower().Contains("bdsm") || Page.sub.ToLower().Contains("impact") || Page.sub.ToLower().Contains("bruised") || Page.sub.ToLower().Contains("bondage"))
                        {
                            Console.WriteLine("Got Something!");
                            var Link = $"https://a.4cdn.org/hc/thread/{Page.no}.json";
                            Json = Client.DownloadString(Link);
                            var Posts = JsonConvert.DeserializeObject<RootPost>(Json);
                            bool ImageFound = false;
                            do
                            {
                                var RNJesus = new Random().Next(0, Posts.posts.Count);
                                if (Posts.posts[RNJesus].tim != null)
                                {
                                    // https://i.4cdn.org/[board]/[4chan image ID].[file extension]
                                    Link = $"https://i.4cdn.org/hc/{Posts.posts[RNJesus].tim}{Posts.posts[RNJesus].ext}";
                                    using (var ms = new MemoryStream(new WebClient().DownloadData(Link)))
                                    {
                                        Clipboard.SetImage(Image.FromStream(ms));
                                    }
                                    ImageFound = true;
                                }
                            } while (ImageFound != true);

                        }
                    }
                }
                Console.WriteLine("Waiting.");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            } while (true);
        }
    }
}

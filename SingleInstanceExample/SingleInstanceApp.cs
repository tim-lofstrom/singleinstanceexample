using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleInstanceExample
{
    class SingleInstanceApp
    {
        public static bool InitializeSingleInstanceApp(ISingleInstanceApp singleInstanceApp)
        {
            var temp = Environment.GetCommandLineArgs();
            string args = string.Join(" ", temp);
            var client = new NamedPipeClientStream("PipesOfPiece");
            try
            {
                client.Connect(20);
            }
            catch
            {
                StartServer(singleInstanceApp);
                return true;
            }
            StreamWriter writer = new StreamWriter(client);
            writer.WriteLine(args);
            writer.Flush();
            return false;
        }

        public static void StartServer(ISingleInstanceApp singleInstanceApp)
        {
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream("PipesOfPiece");

                while (true)
                {
                    server.WaitForConnection();
                    StreamReader reader = new StreamReader(server);

                    try
                    {
                        var line = reader.ReadLine();
                        string[] args = line.Split(' ');
                        singleInstanceApp.IncomingArgs(args);
                    }
                    catch { }

                    server.Disconnect();
                }
            });
        }

    }
}

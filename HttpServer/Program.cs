// https://codingvision.net/networking/c-simple-http-server

using System;
using System.Net;
using System.IO;
using System.Text;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener server = new HttpListener();

            if (server.IsListening)
                throw new InvalidOperationException("Server is currently running.");

            server.Prefixes.Add("http://127.0.0.1/");
            server.Prefixes.Add("http://localhost/");

            server.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;

                string page = context.Request.Url.LocalPath.Replace("/", "\\");

                if (page == string.Empty || page == "\\")
                    page = "index.html";

                page = Directory.GetCurrentDirectory() + "\\" + page;

                TextReader tr = new StreamReader(page);
                string msg = tr.ReadToEnd();

                byte[] buffer = Encoding.UTF8.GetBytes(msg);

                response.ContentLength64 = buffer.Length;
                Stream st = response.OutputStream;
                st.Write(buffer, 0, buffer.Length);

                context.Response.Close();
            }
        }
    }
}

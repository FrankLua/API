
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SharpCompress.Common;
using System.Runtime.CompilerServices;
using System.Text;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace API.DAL.Entity.SupportClass
{
    public static class Loger
    {
        private static Stopwatch _watch;
        private static char separator = Path.DirectorySeparatorChar;
        private static string _filepath = "/app/logerfolder/log.txt";
        private static FileInfo _logfile = new FileInfo(_filepath);
        private static string str = "\n<========================================================>\n";
        

        public static void WriterLogMethod(string Method, string message = "No message")
        {
            Console.WriteLine($"Method: {Method}, \n Message: {message}");
        }
        public static async Task BeginWriter(string Method, HttpRequest req)
        {
            StringBuilder sb = new StringBuilder();
            _watch = Stopwatch.StartNew(); //Setup timer                

                string querry = req.QueryString.Value;                
                var bodyStr = WriterBody(req);
                sb.Append(str);
                sb.Append("\n============================Begin-method=============================\n");
                sb.Append($"\n\tMethod: {Method}, was called in time: {DateTime.Now}\n");
                sb.Append($"\n\t\tQuerry: {querry}\n");
                sb.Append($"\n\t\t\tBody: {bodyStr}\n");
                sb.Append(str);
                File.AppendAllText(_logfile.FullName, sb.ToString());
                

            

            sb.Clear();
        }
        public static async Task WriterFinish(string Method,string message,string responce ,HttpRequest? req) {
            StringBuilder sb = new StringBuilder();
            _watch.Stop();//Setup timer          
            decimal time = (decimal)_watch.ElapsedMilliseconds / 1000;    
            


                
                
                sb.Append(str);
            sb.Append("\n============================Finish-method=============================\n");
            sb.Append($"\nMethod: {Method}, the method finish has finish, it time :{time}\n");
                sb.Append($"\n\tResponce: {responce}\n");
                sb.Append($"\n\t\tFile size:   {message}\n");
            sb.Append(str);
                File.AppendAllText(_logfile.FullName, sb.ToString());

                  
        }
        public static void CreateFirstTxt()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(str);
            sb.Append("\n============================Start-method=============================\n");
            sb.Append($"\nПриложение было запущенно!\n Время запуска: {DateTime.Now}\n");
            sb.Append(str);

            using (FileStream fstream = _logfile.Open(FileMode.Create))
            {
                byte[] buffer = Encoding.Default.GetBytes(sb.ToString());
                fstream.WriteAsync(buffer, 0, buffer.Length);
                fstream.Close();
            }


        }
        public static void Exaption(Exception ex,string method )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(str);
            sb.Append($"\nAttention!Attention!Attention!Attention!Attention!");
            sb.Append($"\n\tApplication detectid error: \n{ex.Message}\n in time: {DateTime.Now}\n");
            sb.Append($"\n\t\tAn error was created in the method{method}\n");
            sb.Append($"\n\t\t\tStack: {ex.StackTrace}\n");
            sb.Append($"\nAttention!Attention!Attention!Attention!Attention!");
            sb.Append(str);
            File.AppendAllText(_logfile.FullName, sb.ToString());
            sb.Clear();
        }
        public static string WriterBody(HttpRequest Request)
        {
            Request.EnableBuffering();
            string bodyContent = new StreamReader(Request.Body).ReadToEndAsync().Result;
            Request.Body.Position = 0;
            return bodyContent;
        }
        public static async Task BeginMethod(HttpRequest? Request)
        {
			string Metodname = Request.RouteValues.Values.ToArray()[0].ToString();
			await BeginWriter(Metodname, Request);
			WriterLogMethod(Metodname, "Called");
		}
        public static async Task FinishMethod(HttpRequest? Request, string message,string responce)
        {
            string Metodname = Request.RouteValues.Values.ToArray()[0].ToString();
            await WriterFinish(Metodname,responce,message, Request);
            WriterLogMethod(Metodname, message);
        }



    }
}

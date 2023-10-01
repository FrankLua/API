
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
        
        
        private static string _filepath = "/app/logerfolder/log.txt";
        private static FileInfo _logfile = new FileInfo(_filepath);
        private static string str = "\n<========================================================>\n";
        

        public static void WriterLogMethod(string Method, string message = "No message")
        {
            Console.WriteLine($"Method: {Method}, \n Message: {message}");
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
            sb.Append($"\nExaption!");
            sb.Append($"\n\tApplication detectid error: \n{ex.Message}\n in time: {DateTime.Now}\n");
            sb.Append($"\n\t\tAn error was created in the method{method}\n");
            sb.Append($"\n\t\t\tStack: {ex.StackTrace}\n");
            sb.Append($"\nExaption!");
            sb.Append(str);
            File.AppendAllText(_logfile.FullName, sb.ToString());
            sb.Clear();
        }
        public static void ExaptionForNotFound(Exception ex, string method, string id,params string[]add)
        {
            add = add ?? new string[] { "none" };
            StringBuilder sb = new StringBuilder();
            sb.Append(str);
            sb.Append($"\nNotFound!");
            sb.Append($"\n\tApplication detectid error: \n{ex.Message}\n in time: {DateTime.Now}\n");
            sb.Append($"\n\t\tData not found here ---> {method}\n");
            sb.Append($"\n\t\tSearch it ---> {id}\n");             
            sb.Append($"\n\t\tData base ---> {add[0]}\n");
            sb.Append($"\n\t\t\tStack: {ex.StackTrace}\n");
            sb.Append($"\nNotFound!");
            sb.Append(str);
            File.AppendAllText(_logfile.FullName, sb.ToString());
            sb.Clear();
        }
       



    }
}

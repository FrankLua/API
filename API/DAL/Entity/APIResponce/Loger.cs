using SharpCompress.Common;
using System.Runtime.CompilerServices;
using System.Text;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace API.DAL.Entity.APIResponce
{
    public  static class Loger
    {
        private static FileInfo _logfile;
        private static string _filepath = "logerfolder/";
        public static void WriterLogMethod(string Method, string message = "No message")
        {
            Console.WriteLine($"Method: {Method}, \n Message: {message}");
        }
        public static void WriterTxtfile(string message)
        {
            StringBuilder sb = new StringBuilder();
           
            sb.Append($"\n{message}\n");

            File.AppendAllText(_logfile.FullName, sb.ToString());

            sb.Clear();
        }
        public static void CreateFirstTxt()
        {
            
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Приложение было запущенно!\n Время запуска: {DateTime.Now}");

            _logfile = new FileInfo($"{Environment.CurrentDirectory}/{_filepath}API.Loger.txt");
            using (FileStream fstream = _logfile.Open(FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(sb.ToString());
                fstream.WriteAsync(buffer, 0, buffer.Length);
                fstream.Close();
            }           
            

        }


    }
}

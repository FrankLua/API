using System.Text;

namespace API.DAL.Entity.SupportClass
{
    public static class Loger
    {
        private const string FileDir = "/app/logerfolder";
        private const string FilePath = $"{FileDir}/log.txt";
        private static readonly FileInfo Logfile = new(FilePath);
        private const string Separator = "\n<========================================================>\n";

        public static void CreateFirstTxt()
        {
            Directory.CreateDirectory(FileDir);
            var sb = new StringBuilder();
            sb.Append(Separator);
            sb.Append("\n============================Start-method=============================\n");
            sb.Append($"\nПриложение было запущенно!\n Время запуска: {DateTime.Now}\n");
            sb.Append(Separator);
            using (var fileStream = Logfile.Open(FileMode.Create))
            {
                var buffer = Encoding.Default.GetBytes(sb.ToString());
                fileStream.WriteAsync(buffer, 0, buffer.Length);
                fileStream.Close();
            }
        }

        public static void Exception(Exception ex, string method)
        {
            var sb = new StringBuilder();
            sb.Append(Separator);
            sb.Append($"\nException!");
            sb.Append($"\n\tApplication detectid error: \n{ex.Message}\n in time: {DateTime.Now}\n");
            sb.Append($"\n\t\tAn error was created in the method{method}\n");
            sb.Append($"\n\t\t\tStack: {ex.StackTrace}\n");
            sb.Append($"\nException!");
            sb.Append(Separator);
            File.AppendAllText(Logfile.FullName, sb.ToString());
            sb.Clear();
        }

        public static void ExceptionForNotFound(Exception ex, string method, string id, params string[] add)
        {
            var sb = new StringBuilder();
            sb.Append(Separator);
            sb.Append($"\nNotFound!");
            sb.Append($"\n\tApplication detectid error: \n{ex.Message}\n in time: {DateTime.Now}\n");
            sb.Append($"\n\t\tData not found here ---> {method}\n");
            sb.Append($"\n\t\tSearch it ---> {id}\n");
            sb.Append($"\n\t\tData base ---> {add[0]}\n");
            sb.Append($"\n\t\t\tStack: {ex.StackTrace}\n");
            sb.Append($"\nNotFound!");
            sb.Append(Separator);
            File.AppendAllText(Logfile.FullName, sb.ToString());
            sb.Clear();
        }
    }
}
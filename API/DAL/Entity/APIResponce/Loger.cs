using ZstdSharp.Unsafe;

namespace API.DAL.Entity.APIResponce
{
    public  class Loger
    {
        public static void WriterLogMethod(string Method, string message = "No message")
        {
            Console.WriteLine($"Method: {Method}, \n Message: {message}");
        }


    }
}

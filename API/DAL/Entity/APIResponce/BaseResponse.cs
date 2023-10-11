using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace API.DAL.Entity.APIResponce
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string error { get; set; }

        public T data { get; set; }

        

        
    }
    public struct LinkFile
    {
        public string URL { get; set; }
    }
    
    public interface IBaseResponse<T>
    {
        public T data { get; }


    }
}

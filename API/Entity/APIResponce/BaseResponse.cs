namespace API.Entity.APIResponce
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string error { get; set; }

        public T data { get; set; }

        
    }
    
    public interface IBaseResponse<T>
    {
        T data { get; }


    }
}

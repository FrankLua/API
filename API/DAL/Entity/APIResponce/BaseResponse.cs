namespace API.DAL.Entity.APIResponce
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string error { get; set; }

        public T data { get; set; }

        

        
    }
    
    public interface IBaseResponse<T>
    {
        public T data { get; }


    }
}

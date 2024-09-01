namespace Social.APi.Dtos
{
    public class ResponseDTO<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}

namespace Book.Store.Api.Server
{
    public class OperationResult
    {
        public string ErrorTitle { get; set; }
        public int StatusCode { get; set; }
        public object ErrorResult { get; set; }
        public string ErrorMessage { get; set; }
        public bool Successful => ErrorMessage == null;
        public object Result { get; set; }
    }
}

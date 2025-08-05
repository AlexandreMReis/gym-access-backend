namespace GymAccessBackend.Core.Models
{
    public class Result<T>
    {
        public Result(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }

        public Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}

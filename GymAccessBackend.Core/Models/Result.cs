namespace GymAccessBackend.Core.Models
{
    public class Result<T>
    {
        public Result(string message, bool isSuccess = true)
        {
            IsSuccess = isSuccess;
            if(IsSuccess)
            {
                QrCode = message;
            }
            else
            {
                ErrorMessage = message;
            }   
        }

        public bool IsSuccess { get; set; }
        public string QrCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}

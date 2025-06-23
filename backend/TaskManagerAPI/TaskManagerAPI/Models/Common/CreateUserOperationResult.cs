namespace TaskManagerAPI.Models.Common
{
    public class CreateUserOperationResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Result { get; set; }

        public static CreateUserOperationResult<T> Ok(T result) =>
            new CreateUserOperationResult<T> { Success = true, Result = result };

        public static CreateUserOperationResult<T> Fail(string errorMessage) =>
            new CreateUserOperationResult<T> { Success = false, ErrorMessage = errorMessage };
    }
}

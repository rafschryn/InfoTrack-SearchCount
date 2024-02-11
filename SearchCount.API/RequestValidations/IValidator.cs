namespace SearchCount.API.RequestValidations
{
    public interface IValidator<T>
    {
        void Validate(T model);
    }
}

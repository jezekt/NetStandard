namespace JezekT.NetStandard.Validation
{
    public interface IValidation<in T> : IProvideValidationErrors where T : class 
    {
        bool Validate(T objToValidate);
    }
}

namespace Health_prescription_software_API.Contracts.Validations
{
    public interface IChatValidation : IValidationErrorMessage
    {
        Task<bool> IsEngValid(string egn);

        bool IsMsgLengthValid(string msg);
    }
}
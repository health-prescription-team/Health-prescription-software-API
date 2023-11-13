namespace Health_prescription_software_API.Contracts.Validations
{
    public interface IValidationErrorMessage
    {
        public string? ErrorMessage { get; }
        public string? ErrorPropName { get; }
    }
}

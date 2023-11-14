namespace Health_prescription_software_API.Contracts.Validations
{
    public class ModelError
    {
        public string? ErrorMessage { get; set; }
        public string? ErrorPropName { get; set; }
    }

    public interface IValidationErrorMessage
    {
        ICollection<ModelError> ModelErrors { get; }
    }
}

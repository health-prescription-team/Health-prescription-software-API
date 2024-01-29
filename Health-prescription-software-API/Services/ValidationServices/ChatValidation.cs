namespace Health_prescription_software_API.Services.ValidationServices
{
    using Contracts.Validations;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using static Common.EntityValidationConstants.User;
    using static Common.EntityValidationErrorMessages.User;
    using static Common.EntityValidationConstants.Chat;
    using static Common.EntityValidationErrorMessages.Chat;

    public class ChatValidation : IChatValidation
    {
        private readonly HealthPrescriptionDbContext dbContext;

        public ChatValidation(HealthPrescriptionDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.ModelErrors = new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsEngValid(string egn)
        {
            if (string.IsNullOrWhiteSpace(egn) || !Regex.Match(egn, EgnRegexPattern).Success)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = "EGN",
                    ErrorMessage = InvalidEgnErrorMessage
                };

                ModelErrors.Add(modelError);

                return false;
            }

            if (!await dbContext.Users.AnyAsync(u => u.Egn == egn))
            {
                var modelError = new ModelError
                {
                    ErrorPropName = "EGN",
                    ErrorMessage = UserWithEgnDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public bool IsMsgLengthValid(string msg)
        {
            if (!(msg.Length >= MsgMinLength && msg.Length <= MsgMaxLength))
            {
                var modelError = new ModelError
                {
                    ErrorPropName = "MSG",
                    ErrorMessage = string.Format(InvalidMsgLength, MsgMinLength, MsgMaxLength)
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }
    }
}
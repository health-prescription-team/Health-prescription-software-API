namespace Health_prescription_software_API.Services.ValidationServices
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    using Data;
    using Data.Entities.User;
    using Models.Authentication.GP;
    using Models.Authentication.Patient;
    using Models.Authentication.Pharmacist;
    using Models.Authentication.Pharmacy;

    using Common.Roles;
    using Contracts.Validations;

    using static Common.EntityValidationErrorMessages.User;
    using static Common.EntityValidationErrorMessages.Authentication;

    public class ValidationAuthentication : IValidationAuthentication
    {
        private readonly HealthPrescriptionDbContext dbContext;
        private readonly UserManager<User> userManager;

        public ValidationAuthentication(HealthPrescriptionDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.ModelErrors = new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsPharmacyLoginValid(LoginPharmacyDto loginModel)
        {
            User? userExistsByEmail = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            ModelError? modelError;

            if (userExistsByEmail == null)
            {
                modelError = new ModelError
                {
                    ErrorPropName = nameof(loginModel.Email),
                    ErrorMessage = PharmacyUserWithEmailDoesNotExists
                };

                ModelErrors.Add(modelError);

                return false;
            }

            bool isUserRoleValid = await userManager.IsInRoleAsync(userExistsByEmail, RoleConstants.Pharmacy);

            if (!isUserRoleValid)
            {
                modelError = new ModelError
                {
                    ErrorPropName = RoleConstants.Pharmacy,
                    ErrorMessage = InvalidLogin
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public async Task<bool> IsPharmacyRegisterValid(RegisterPharmacyDto registerModel)
        {
            bool isEmailPresent = await dbContext.Users.AnyAsync(u => u.Email == registerModel.Email);
            bool pharmacyUserExists = await dbContext.Users.AnyAsync(u => u.PharmacyName == registerModel.PharmacyName);

            var isPasswordValid = await this.IsRegisterPasswordValid(registerModel.Password);

            if (isEmailPresent || pharmacyUserExists || !isPasswordValid)
            {
                ModelError? modelError;
                if (isEmailPresent)
                {
                    var error = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Email),
                        ErrorMessage = PharmacyUserWithEmailExists
                    };

                    ModelErrors.Add(error);
                }

                if (pharmacyUserExists)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.PharmacyName),
                        ErrorMessage = PharmacyUserWithSameNameExists
                    };

                    ModelErrors.Add(modelError);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> IsPharmacistRegisterValid(RegisterPharmacistDto registerModel)
        {
            bool isEmailPresent = await dbContext.Users.AnyAsync(u => u.Email == registerModel.Email);
            bool userExistsByEgn = await dbContext.Users.AnyAsync(u => u.Egn == registerModel.Egn);
            bool userExistsByUni = await dbContext.Users.AnyAsync(u => u.UinNumber == registerModel.UinNumber);

            var isPasswordValid = await this.IsRegisterPasswordValid(registerModel.Password);

            if (isEmailPresent || userExistsByEgn || userExistsByUni || !isPasswordValid)
            {
                ModelError? modelError;

                if (isEmailPresent)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Email),
                        ErrorMessage = UserWithEmailExists
                    };

                    ModelErrors.Add(modelError);
                }

                if (userExistsByEgn)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Egn),
                        ErrorMessage = UserWithEgnExists
                    };

                    ModelErrors.Add(modelError);
                }

                if (userExistsByUni)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.UinNumber),
                        ErrorMessage = UserWithUinNumberExists
                    };

                    ModelErrors.Add(modelError);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> IsPharmacistLoginValid(LoginPharmacistDto loginModel)
        {
            User? userExistsByEgn = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == loginModel.Egn);

            ModelError? modelError;

            if (userExistsByEgn == null)
            {
                modelError = new ModelError
                {
                    ErrorPropName = nameof(loginModel.Egn),
                    ErrorMessage = UserWithEgnDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            bool isUserRoleValid = await userManager.IsInRoleAsync(userExistsByEgn, RoleConstants.Pharmacist);

            if (!isUserRoleValid)
            {
                modelError = new ModelError
                {
                    ErrorPropName = RoleConstants.Pharmacist,
                    ErrorMessage = InvalidLogin
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public async Task<bool> IsPatientRegisterValid(RegisterPatientDto registerModel)
        {
            bool userExistsByEgn = await dbContext.Users.AnyAsync(u => u.Egn == registerModel.Egn);

            var isPasswordValid = await this.IsRegisterPasswordValid(registerModel.Password);

            if (userExistsByEgn || !isPasswordValid)
            {
                ModelError? modelError;

                if (userExistsByEgn)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Egn),
                        ErrorMessage = UserWithEgnExists
                    };

                    ModelErrors.Add(modelError);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> IsPatientLoginValid(LoginPatientDto loginModel)
        {
            User? userExistsByEgn = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == loginModel.Egn);

            ModelError? modelError;

            if (userExistsByEgn == null)
            {
                modelError = new ModelError
                {
                    ErrorPropName = nameof(loginModel.Egn),
                    ErrorMessage = UserWithEgnDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            bool isUserRoleValid = await userManager.IsInRoleAsync(userExistsByEgn, RoleConstants.Patient);

            if (!isUserRoleValid)
            {
                modelError = new ModelError
                {
                    ErrorPropName = RoleConstants.Patient,
                    ErrorMessage = InvalidLogin
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public async Task<bool> IsGpRegisterValid(RegisterGpDto registerModel)
        {
            var userExistsByEgn = await dbContext.Users.AnyAsync(u => u.Egn == registerModel.Egn);
            var userExistsByUINNumber = await dbContext.Users.AnyAsync(u => u.UinNumber == registerModel.UinNumber);

            var isPasswordValid = await this.IsRegisterPasswordValid(registerModel.Password);

            ModelError modelError;

            if (userExistsByEgn || userExistsByUINNumber || !isPasswordValid)
            {

                if (userExistsByEgn)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Egn),
                        ErrorMessage = UserWithEgnExists

                    };

                    ModelErrors.Add(modelError);
                }

                if (userExistsByUINNumber)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.UinNumber),
                        ErrorMessage = UserWithUinNumberExists

                    };

                    ModelErrors.Add(modelError);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> IsGpLoginValid(LoginGpDto loginModel)
        {
            User? userExistsByEgn = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == loginModel.Egn);

            ModelError? modelError;

            if (userExistsByEgn == null)
            {
                modelError = new ModelError
                {
                    ErrorPropName = nameof(loginModel.Egn),
                    ErrorMessage = UserWithEgnDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            bool isUserRoleValid = await userManager.IsInRoleAsync(userExistsByEgn, RoleConstants.GP);

            if (!isUserRoleValid)
            {
                modelError = new ModelError
                {
                    ErrorPropName = RoleConstants.GP,
                    ErrorMessage = InvalidLogin
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        private async Task<bool> IsRegisterPasswordValid(string password)
        {
            foreach (var validation in userManager.PasswordValidators)
            {
                var result = await validation.ValidateAsync(this.userManager, null!, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelError modelError = new()
                        {
                            ErrorPropName = "Password",
                            ErrorMessage = error.Description
                        };

                        ModelErrors.Add(modelError);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}

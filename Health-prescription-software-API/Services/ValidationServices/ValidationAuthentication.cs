namespace Health_prescription_software_API.Services.ValidationServices
{
    using Common.Roles;
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities.User;
    using Health_prescription_software_API.Models.Authentication.GP;
    using Health_prescription_software_API.Models.Authentication.Patient;
    using Health_prescription_software_API.Models.Authentication.Pharmacist;
    using Health_prescription_software_API.Models.Authentication.Pharmacy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            bool isEmailPresent = await dbContext.Users
                .AnyAsync(u => u.Email == registerModel.Email);
            User? pharmacyUserExists = await dbContext.Users.FirstOrDefaultAsync(u => u.PharmacyName == registerModel.PharmacyName);
            
            if (isEmailPresent || pharmacyUserExists != null)
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
                if (pharmacyUserExists != null)
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
            User? userExistsByEgn = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == registerModel.Egn);
            User? userExistsByUni = await dbContext.Users.FirstOrDefaultAsync(u => u.UinNumber == registerModel.UinNumber);

            if (isEmailPresent || userExistsByEgn != null || userExistsByUni != null)
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

                if (userExistsByEgn != null)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(registerModel.Egn),
                        ErrorMessage = UserWithEgnExists
                    };

                    ModelErrors.Add(modelError);
                }

                if (userExistsByUni != null)
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

        public async Task<bool> IsPatientRegisterValid(PatientDto registerModel)
        {
           
            User? userExistsByEgn = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == registerModel.Egn);
            if (userExistsByEgn != null)
            {
                ModelError? modelError;
             

                if (userExistsByEgn != null)
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
            var userExistsByEgn = await dbContext.Users
               .AnyAsync(u => u.Egn == registerModel.Egn);
            var userExistsByUINNumber = await dbContext.Users.AnyAsync(u => u.UinNumber == registerModel.UinNumber);

            ModelError modelError;

            if (userExistsByEgn || userExistsByUINNumber)
            {

                if (userExistsByEgn)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = "UserWithEgnExists",
                        ErrorMessage = UserWithEgnExists

                    };

                    ModelErrors.Add(modelError);

                }

                if (userExistsByUINNumber)
                {

                    modelError = new ModelError
                    {
                        ErrorPropName = "UserWithUinNumberExists",
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
    }
}

namespace Health_prescription_software_API.Contracts
{
    using Data.Entities.User;
    using Models.Authentication.GP;
    using Models.Authentication.Patient;
    using Models.Authentication.Pharmacist;
    using Models.Authentication.Pharmacy;

    public interface IAuthenticationService
    {
        Task<string?> LoginPatient(LoginPatientDto model);

        Task<string?> RegisterPatient(RegisterPatientDto model);

        Task<string?> RegisterGp(RegisterGpDto model);

        Task<string?> LoginGp(LoginGpDto model);

		Task<string?> RegisterPharmacy(RegisterPharmacyDto pharmacyUser);

		Task<string?> LoginPharmacy(LoginPharmacyDto pharmacyUser);

        Task<string?> RegisterPharmacist(RegisterPharmacistDto model);

        Task<string> LoginPharmacist(LoginPharmacistDto model);

        Task<User?> GetUserByEgn(string egn);
    }

}

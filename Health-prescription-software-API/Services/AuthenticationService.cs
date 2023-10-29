using Health_prescription_software_API.Common.Roles;
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities.User;
using Health_prescription_software_API.Models.Authentification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Health_prescription_software_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HealthPrescriptionDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(HealthPrescriptionDbContext context,
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> RegisterDoctor(GPDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("GP data cannot be null!");
            }

            if (model.ProfilePicture == null || model.ProfilePicture.Length == 0)
            {
                throw new NullReferenceException("ProfilePicture model cannot be null!");
            }

            using (var memoryStream = new MemoryStream())
            {
                await model.ProfilePicture.CopyToAsync(memoryStream);

                var gpModel = new User
                {
                    FirstName = model.FirstName,
                    MiddleName = model.LastName,
                    LastName = model.LastName,
                    UinNumber = model.UinNumber,
                    ProfilePicture = memoryStream.ToArray(),
                    Egn = model.Egn,
                    HospitalName = model.HospitalName,
                    Email = "test@abv.bg93",
                    UserName = "TestGP93",
                    PhoneNumber = model.PhonrNumber

                };

                var result = await _userManager.CreateAsync(gpModel, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(gpModel, isPersistent: false);

                    var user = await GetUserByEgn(model.Egn);

                    await _userManager.AddToRoleAsync(user, RoleConstants.GP);

                    var securityToken = await GenerateToken(user);

                    return securityToken;
                }
            }

            return null;

        }
        public async Task<string> RegisterPatient(PatientDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Patient data cannot be null!");
            }
            if (model.ProfilePicture == null || model.ProfilePicture.Length == 0)
            {
                throw new NullReferenceException("ProfilePicture model cannot be null!");
            }
            using (var memoryStream = new MemoryStream())
            {
                await model.ProfilePicture.CopyToAsync(memoryStream);

                var patientModel = new User
                {
                    FirstName = model.FirstName,
                    MiddleName = model.LastName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = $"{model.FirstName}{model.Egn}",
                    ProfilePicture = memoryStream.ToArray(),
                    Egn = model.Egn,
                    Email = "test3@abv.bg"
                };
                var result = await _userManager.CreateAsync(patientModel, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(patientModel, isPersistent: false);

                    var user = await GetUserByEgn(model.Egn);

                    await _userManager.AddToRoleAsync(user, RoleConstants.Patient);

                    var securityToken = await GenerateToken(user);

                    return securityToken;
                }
            }
            return null;
        }
            private async Task<string> GenerateToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"));

                var userRole = await GetUserRole(user);

                if (string.IsNullOrEmpty(userRole))
                {

                    throw new Exception("User role not found");
                }

                var claims = new List<Claim>
{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.MiddleName} {user.LastName}"),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                new Claim(ClaimTypes.Role, userRole)

};

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        private async Task<string> GetUserRole(User user)
        {
            var userRole = await _context.Users.FirstOrDefaultAsync(x => x.Egn == user.Egn);

            if (userRole == null)
            {
                throw new ArgumentNullException("The given user was not found!");
            }

            var role = await _userManager.GetRolesAsync(userRole);

            return role[0].ToString();

        }

        private async Task<User?> GetUserByEgn(int egn)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Egn == egn);
        }

    }
}

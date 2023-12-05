namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Common.Roles;
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities.User;
    using Health_prescription_software_API.Models.Authentication.GP;
    using Health_prescription_software_API.Models.Authentication.Patient;
    using Health_prescription_software_API.Models.Authentication.Pharmacist;
    using Health_prescription_software_API.Models.Authentication.Pharmacy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;


    public class AuthenticationService : IAuthenticationService
    {
        private readonly HealthPrescriptionDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(
            HealthPrescriptionDbContext context,
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string?> LoginPatient(LoginPatientDto model)
        {
            User? user = await GetUserByEgn(model.Egn);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    var token = await GenerateToken(user);
                    return token;
                }
            }

            return string.Empty;
        }
        public async Task<string?> RegisterPatient(PatientDto model)
        {
            using var memoryStream = new MemoryStream();
            await model.ProfilePicture.CopyToAsync(memoryStream);
            
            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.LastName,
                LastName = model.LastName,
                ProfilePicture = memoryStream.ToArray(),
                Egn = model.Egn,
                Email = null,
                UserName = model.Egn,
                PhoneNumber = model.PhoneNumber

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var userEntity = await this.GetUserByEgn(model.Egn);

            await _userManager.AddToRoleAsync(userEntity!, RoleConstants.Patient);

            var securityToken = await this.GenerateToken(userEntity!);

            return securityToken;
            
        }

        public async Task<string?> RegisterGp(RegisterGpDto model)
        {
            using var memoryStream = new MemoryStream();
            await model.ProfilePicture.CopyToAsync(memoryStream);

            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.LastName,
                LastName = model.LastName,
                ProfilePicture = memoryStream.ToArray(),
                Egn = model.Egn,
                Email = null,
                UserName = model.Egn,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var userEntity = await this.GetUserByEgn(model.Egn);

            await _userManager.AddToRoleAsync(userEntity!, RoleConstants.GP);

            var securityToken = await this.GenerateToken(userEntity!);

            return securityToken;
            
        }

        public async Task<string?> LoginGp(LoginGpDto model)
        {
            var user = await GetUserByEgn(model.Egn);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    var token = await GenerateToken(user);
                    return token;
                }
            }

            return string.Empty;
        }


        public async Task<string?> RegisterPharmacy(RegisterPharmacyDto model)
        {

            User pharmacyUser = new User
            {
                FirstName = null,
                MiddleName = null,
                LastName = null,
                UinNumber = null,
                ProfilePicture = null,
                Egn = null,
                HospitalName = null,
                Email = model.Email,
                UserName = model.PharmacyName,
                PhoneNumber = model.PhoneNumber
            };

            IdentityResult? result = await _userManager.CreateAsync(pharmacyUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(pharmacyUser, isPersistent: false);

                User? user = await GetUserByEmailAsync(pharmacyUser.Email);

                await _userManager.AddToRoleAsync(user!, RoleConstants.Pharmacy);

                var securityToken = await GenerateToken(user!);

                return securityToken;
            }

            return null;

        }

        public async Task<string?> LoginPharmacy(LoginPharmacyDto model)
        {
            User? pharmacyUser = await GetUserByEmailAsync(model.Email);
            if (pharmacyUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(pharmacyUser, model.Password, false, false);

                if (result.Succeeded)
                {
                    var token = await GenerateToken(pharmacyUser);
                    return token;
                }
            }

            return null;
        }

        public async Task<string?> RegisterPharmacist(RegisterPharmacistDto model)
        {
            using var memoryStream = new MemoryStream();
            await model.ProfilePicture.CopyToAsync(memoryStream);

            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.LastName,
                LastName = model.LastName,
                UinNumber = model.UinNumber,
                ProfilePicture = memoryStream.ToArray(),
                Egn = model.Egn,
                Email = model.Email,
                UserName = model.Egn,
                PhoneNumber = model.PhoneNumber

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var userEntity = await this.GetUserByEgn(model.Egn);

            await _userManager.AddToRoleAsync(userEntity!, RoleConstants.Pharmacist);

            var securityToken = await this.GenerateToken(userEntity!);

            return securityToken;
        }

        public async Task<string> LoginPharmacist(LoginPharmacistDto model)
        {
            var user = await GetUserByEgn(model.Egn);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    var token = await this.GenerateToken(user);
                    return token;
                }
            }

            return string.Empty;
        }

        private async Task<string> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var userRole = await GetUserRole(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.MiddleName} {user.LastName}"),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                new Claim(ClaimTypes.Role, userRole)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToString = tokenHandler.WriteToken(token);

            return tokenToString;
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

        private async Task<User?> GetUserByEgn(string egn)
        {
            if (egn == null)
            {
                throw new ArgumentException("Egn cannot be null!");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.Egn == egn);
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}

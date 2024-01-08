namespace Health_prescription_software_API.Services
{
    using Common.Roles;
    using Contracts;
    using Data;
    using Data.Entities.User;

    using Models.Authentication.GP;
    using Models.Authentication.Patient;
    using Models.Authentication.Pharmacist;
    using Models.Authentication.Pharmacy;

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
                    var token = GenerateToken(user, RoleConstants.Patient);
                    return token;
                }
            }

            return string.Empty;
        }

        public async Task<string?> RegisterPatient(RegisterPatientDto model)
        {
            using var memoryStream = new MemoryStream();
            await model.ProfilePicture.CopyToAsync(memoryStream);

            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
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

            var userEntity = await GetUserByEgn(model.Egn);

            await _userManager.AddToRoleAsync(userEntity!, RoleConstants.Patient);

            var securityToken = GenerateToken(userEntity!);

            return securityToken;

        }

        public async Task<string?> RegisterGp(RegisterGpDto model)
        {
            using var memoryStream = new MemoryStream();
            await model.ProfilePicture.CopyToAsync(memoryStream);

            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                ProfilePicture = memoryStream.ToArray(),
                Egn = model.Egn,
                Email = null,
                UserName = model.Egn,
                PhoneNumber = model.PhoneNumber,
                UinNumber = model.UinNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var userEntity = await GetUserByEgn(model.Egn);

            await _userManager.AddToRolesAsync(userEntity!, new string[] { RoleConstants.GP, RoleConstants.Patient });

            var securityToken = GenerateToken(userEntity!, RoleConstants.GP);

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
                    var token = GenerateToken(user, RoleConstants.GP);
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
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                PharmacyName = model.PharmacyName
            };

            IdentityResult? result = await _userManager.CreateAsync(pharmacyUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(pharmacyUser, isPersistent: false);

                User? user = await GetUserByEmailAsync(pharmacyUser.Email);

                await _userManager.AddToRoleAsync(user!, RoleConstants.Pharmacy);

                var securityToken = GenerateToken(user!, RoleConstants.Pharmacy);

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
                    var token = GenerateToken(pharmacyUser, RoleConstants.Pharmacy);
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
                MiddleName = model.MiddleName,
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

            var userEntity = await GetUserByEgn(model.Egn);

            await _userManager.AddToRolesAsync(userEntity!, new string[] { RoleConstants.Pharmacist, RoleConstants.Patient });

            var securityToken = GenerateToken(userEntity!, RoleConstants.Pharmacist);

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
                    var token = GenerateToken(user, RoleConstants.Pharmacist);
                    return token;
                }
            }

            return string.Empty;
        }

        private string GenerateToken(User user, params string[] loginRoles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            string nameClaim;
            List<Claim> claims = [];

            //  Adding role specific claims
            if (loginRoles.Contains(RoleConstants.Pharmacy))
            {
                nameClaim = user.PharmacyName!;
                claims.Add(new Claim(ClaimTypes.Email, user.Email!));
            }
            else
            {
                nameClaim = $"{user.FirstName} {(string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName + " ")}{user.LastName}";
                claims.Add(new Claim("EGN", user.Egn!));
            }

            // Adding shared claims
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, nameClaim),
                new Claim("PhoneNumber", user.PhoneNumber!)
            });

            // Adding role claims
            foreach (var role in loginRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToString = tokenHandler.WriteToken(token);

            return tokenToString;
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

namespace Health_prescription_software_API.Services.Tests.Seeding
{
    using Health_prescription_software_API.Data.Entities.User;
    using Microsoft.AspNetCore.Http;

    public static class UserSeed
    {
        public static User[] GenerateUsers()
        {
            return new User[]
            {
                new()
                {
                    Id = "169f00d6-4abd-493f-8024-95d9c49337b3",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0123456789",
                    Email = "testov@tests.org",
                    UinNumber = "0123456789",
                    PhoneNumber = "088888888",
                    PharmacyName = "Аптека",
                    ProfilePicture = Array.Empty<byte>(),
                },
                new()
                {
                    Id = "20dfb9d8-5e6b-4a9e-8f88-0e3f2f4b681b",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0111111112",
                    PhoneNumber = "088888888",
                    ProfilePicture = Array.Empty<byte>(),
                    
                },
                new()
                {
                    Id = "26d9947f-9192-4c08-9124-8e016714427a",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0111111111",
                    PhoneNumber = "088888888",
                    ProfilePicture = Array.Empty<byte>(),
                },
                new()
                {
                    Id = "9d36d260-fcac-4402-9833-77f1a4ac4288"
                }
            };
        }
    }
}

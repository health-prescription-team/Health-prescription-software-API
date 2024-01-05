namespace Health_prescription_software_API.Services.Tests.Seeding
{
    using Data.Entities.User;

    public static class UserSeed
    {
        public static User[] GenerateUsers()
        {
            return
            [
                new User()
                {
                    Id = "169f00d6-4abd-493f-8024-95d9c49337b3",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0123456789",
                    Email = "testov@tests.org",
                    UinNumber = "0123456789",
                    PhoneNumber = "088888888",
                    PharmacyName = "Аптека",
                    ProfilePicture = [],
                },
                new User()
                {
                    Id = "20dfb9d8-5e6b-4a9e-8f88-0e3f2f4b681b",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0111111112",
                    PhoneNumber = "088888888",
                    ProfilePicture = [],
                    
                },
                new User()
                {
                    Id = "26d9947f-9192-4c08-9124-8e016714427a",
                    FirstName = "Петър",
                    LastName = "Тестов",
                    Egn = "0111111111",
                    PhoneNumber = "088888888",
                    ProfilePicture = [],
                },
                new User()
                {
                    Id = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205",
                    FirstName = "Доктор",
                    MiddleName = "Тестов",
                    LastName = "Тестов",
                    Egn = "5555555555",
                    UinNumber = "5555555555",
                    PhoneNumber = "0888888888"
                },
                new User()
                {
                    Id = "0fb3a33a-9796-4f4b-8349-192a93a136dc",
                    FirstName = "Доктор 2",
                    MiddleName = "Тестов",
                    LastName = "Тестов",
                    Egn = "6666666666",
                    UinNumber = "6666666666",
                    PhoneNumber = "0888888888"
                },
                new User()
                {
                    Id = "df0d66dc-515b-429a-8160-582bef3f7dbe",
                    FirstName = "Фармацевт",
                    LastName = "Тестов",
                    Egn = "4444444444",
                    UinNumber = "4444444444",
                    PhoneNumber = "0888888888",
                    Email = "test@abv.bg"
                },
                new User()
                {
                    Id = "56d1c54a-6dae-4502-b1cd-e10832ed7777",
                    FirstName = "Пациент",
                    LastName = "Тестов",
                    Egn = "3333333333",
                    PhoneNumber = "0888888888"
                },
                new User()
                {
                    Id = "bb02a027-408e-49d7-86d3-8e7b5471feba",
                    FirstName = "Пациент 2",
                    LastName = "Тестов",
                    Egn = "2222222222",
                    PhoneNumber = "0888888888"
                },
                new User()
                {
                    Id = "31bcded4-bbe9-45a3-b235-2d24766cfdf3",
                    Email = "pharmacy@test.bg",
                    PharmacyName = "Аптека",
                    PhoneNumber = "0888888888"
                }
            ];
        }
    }
}

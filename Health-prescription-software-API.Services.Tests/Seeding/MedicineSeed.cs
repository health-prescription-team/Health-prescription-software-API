namespace Health_prescription_software_API.Services.Tests.Seeding
{
    using Data.Entities;

    public static class MedicineSeed
    {
        public static Medicine[] GenerateMedicine()
        {
            return
            [
                new Medicine()
                {
                    Id = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d"),
                    Name = "Аспирин",
                    MedicineImageBytes = [],
                    MedicineCompany = "Fake Pharma",
                    MedicineDetails = "Аспирин 50 мг.",
                    Price = 12.33m,
                    Ingredients = "Кора от дърво?",
                    OwnerId = "31bcded4-bbe9-45a3-b235-2d24766cfdf3"
                },
                new Medicine()
                {
                    Id = Guid.Parse("8f35d73d-6e00-4ba8-a8c8-9fc056d724ad"),
                    Name = "Парацетамол",
                    MedicineImageBytes = [],
                    MedicineCompany = "Лекарства ООД",
                    MedicineDetails = "Парацетамол 14 мг.",
                    Price = 4.33m,
                    Ingredients = null,
                    OwnerId = "31bcded4-bbe9-45a3-b235-2d24766cfdf3"
                },
                new Medicine()
                {
                    Id = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                    Name = "medicine X",
                    MedicineImageBytes = [],
                    MedicineCompany = "Unknown",
                    MedicineDetails = "Gives random super powers.",
                    Price = 4.33m,
                    Ingredients = "J-23A-14Y",
                    OwnerId = "31bcded4-bbe9-45a3-b235-2d24766cfdf3"
                }
            ];
        }
    }
}

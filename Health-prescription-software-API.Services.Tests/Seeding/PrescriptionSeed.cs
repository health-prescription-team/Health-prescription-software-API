namespace Health_prescription_software_API.Services.Tests.Seeding
{
    using Data.Entities;

    public static class PrescriptionSeed
    {
        public static Prescription[] GeneratePrescriptions()
        {
            return
            [
                new Prescription()
                {
                   
                }
            ];
        }

        public static PrescriptionDetails[] GeneratePrescriptionDetails()
        {
            return
            [
                new PrescriptionDetails()
                {

                }
            ];
        }
    }
}

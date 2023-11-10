namespace Health_prescription_software_API.Models.Medicine
{
	public class AllMedicineServiceModel
	{
        public AllMedicineDTO[] Medicines { get; set; } = null!;

        public int MedicinesCount { get; set; }
    }
}

namespace Health_prescription_software_API.Models.Medicine
{
	public class QueryMedicineDTO
	{
		//todo: validation attributes
		public string? SearchTerm { get; set; }


        public int? PageNumber { get; set; } //first page is 1


        public int? EntriesPerPage { get; set; }

    }
}

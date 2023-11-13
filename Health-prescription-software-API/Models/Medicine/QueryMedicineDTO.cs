namespace Health_prescription_software_API.Models.Medicine
{
	using System.ComponentModel.DataAnnotations;

	using static Common.EntityValidationConstants.Medicine;
    using static Common.GeneralConstants;

	public class QueryMedicineDTO
	{
        public QueryMedicineDTO()
        {
            EntriesPerPage = DefaultHitsPerPage;
            PageNumber = DefaultCurrentPage;
        }

        
        [StringLength(SearchTermMax, MinimumLength = SearchTermMin)]
		public string? SearchTerm { get; set; }

		//todo: dynamic validation
		public int? PageNumber { get; set; } 


		[Range(EntriesPerPageMin, EntriesPerPageMax)]
		public int? EntriesPerPage { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurdOperationFinalToFinal.Models
{
	public class state
	{
		[Key]
		public int id { get; set; }
        public string  name { get; set; }
		
		[Display(Name = "country")]
		public virtual int countryId { get; set; }

		[ForeignKey("id")]
		public virtual country country { get; set; }

	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurdOperationFinalToFinal.Models
{
	public class userAddress
	{
		[Key]
		public int addressId{ get; set; }
        public string  address { get; set; }
        public string  city { get; set; }

		
		public  int stateId { get; set; }
		
        public bool isDelete { get; set; }

		
		public  int countryId { get; set; }
		
		
		public  int userId { get; set; }
		
        public List<country> countries { get; set; }
        public List<state> States { get; set; }

	




    }
}

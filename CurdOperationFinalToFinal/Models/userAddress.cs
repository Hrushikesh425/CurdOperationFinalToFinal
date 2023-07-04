using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurdOperationFinalToFinal.Models
{
	public class userAddress
	{
		[Key]
		public int Id { get; set; }
        public string  address { get; set; }
        public string  city { get; set; }

		//state id foreign key 
		[Display(Name ="state")]
		public virtual int stateId { get; set; }
		[ForeignKey("id")]
		public virtual state state { get; set; }
        public bool isDelete { get; set; }

		//country id foreign key 
		[Display(Name = "country")]
		public virtual int countryId { get; set; }
		[ForeignKey("id")]
		public virtual country country { get; set; }

		//user id foregin key 
		[Display(Name = "userData")]
		public virtual int userId { get; set; }
		[ForeignKey("id")]
		public virtual userData userData { get; set; }




	}
}

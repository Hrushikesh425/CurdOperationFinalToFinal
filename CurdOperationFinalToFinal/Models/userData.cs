using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurdOperationFinalToFinal.Models
{
	public class userData
	{
		[Key]
        public int id { get; set; }
		[Required]
        public string  firstName { get; set; }
		[Required]
        public string  lastName { get; set; }
        //[Required]
        //public string  city { get; set; }
        public DateTime dob { get; set; }
        public string Gender { get; set; }
        //gender id foreign key 
        //[Display(Name = "gender")]
        //public virtual int genderId { get; set; }
        //[ForeignKey("id")]
        //public virtual gender gender { get; set; }
        //ending of foreign key
        public string phoneNumber { get; set; }
        public string  Email { get; set; }
		public bool isActive { get; set; } = true;
        //public bool isDelete { get; set; }

		//gender id foreign key 
		//[Display(Name = "userAddress")]
		//public virtual int userAddressId { get; set; }
		//[ForeignKey("id")]
		//public virtual userAddress userAddress { get; set; }
		//ending of foreign key 
	}
}

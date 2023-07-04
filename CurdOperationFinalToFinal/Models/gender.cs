using System.ComponentModel.DataAnnotations;

namespace CurdOperationFinalToFinal.Models
{
	public class gender
	{
		[Key]
        public int id { get; set; }
        public string  name { get; set; }
    }
}

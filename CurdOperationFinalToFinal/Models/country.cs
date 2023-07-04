using System.ComponentModel.DataAnnotations;

namespace CurdOperationFinalToFinal.Models
{
	public class country
	{
		[Key]
		public int id { get; set; }
        public string  name { get; set; }
    }
}

namespace CurdOperationFinalToFinal.Models.UserVM
{
	public class UserVM
	{
        public userData userData { get; set; }

		public userAddress Address { get; set; }

		
       
        public List<country> countries { get; set; }
		public List<state> States { get; set; }
		/*public static implicit operator UserVM(userData u)
		{
			throw new NotImplementedException();
		}*/
    }
}

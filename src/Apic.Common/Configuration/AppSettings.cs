namespace Apic.Common.Configuration
{
	public class AppSettings
	{
		public ApplicationSettings Application { get; set; }
		public ConnectionStrings ConnectionStrings { get; set; }
        public Throttling Throttling { get; set; }
	}
}

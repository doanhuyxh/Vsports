namespace vsports.Models
{
	public class JsonResultVM
	{
		public string Message { get; set; }
		public int StatusCode { get; set; }
		public dynamic Data { get; set; }
	}
}

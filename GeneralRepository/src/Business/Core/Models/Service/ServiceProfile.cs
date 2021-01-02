using Core.Enums;

namespace Core.Models.Service
{
	public class ServiceProfile
	{
		public string ObjectID { get; set; }
		public string Description { get; set; }
		public string BaseUrl { get; set; }
		public ServiceCallTypeEnum CallType { get; set; }
	}
}
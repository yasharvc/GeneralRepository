using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Service;

namespace ServiceRegistry.Interfaces
{
	public interface IServiceRegistry
	{
		Task<IEnumerable<ServiceProfile>> GetAllServices();
		Task<IEnumerable<ServiceProfile>> GetServicesByObjectID(string objectID);
		Task<ServiceProfile> GetServiceByRegisteredId(Guid registeredId);
		Task<Guid> Register(ServiceProfile service);
		Task<bool> Unregister(Guid registeredId);
		event EventHandler<ServiceProfile> OnServiceRegister;
		event EventHandler<Guid> OnServiceUnregister;

	}
}

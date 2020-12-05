using Core.Models.Service;
using ServiceRegistry.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMemoryServiceRegistry
{
	public class ServiceRegistry : IServiceRegistry
	{
		public event EventHandler<ServiceProfile> OnServiceRegister;
		public event EventHandler<Guid> OnServiceUnregister;

		public Task<IEnumerable<ServiceProfile>> GetAllServices()
		{
			throw new NotImplementedException();
		}

		public Task<ServiceProfile> GetServiceByRegisteredId(Guid registeredId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ServiceProfile>> GetServicesByObjectID(string objectID)
		{
			throw new NotImplementedException();
		}

		public Task<Guid> Register(ServiceProfile service)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Unregister(Guid registeredId)
		{
			throw new NotImplementedException();
		}
	}
}

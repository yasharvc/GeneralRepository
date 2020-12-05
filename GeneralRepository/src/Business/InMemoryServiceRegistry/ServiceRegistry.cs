using Core.Models.Service;
using ServiceRegistry.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryServiceRegistry
{
	public class ServiceRegistry : IServiceRegistry
	{
		BlockingCollection<RegisteredServiceProfile> RegisteredServices { get; } = new BlockingCollection<RegisteredServiceProfile>();
		public event EventHandler<ServiceProfile> OnServiceRegister;
		public event EventHandler<Guid> OnServiceUnregister;

		public Task<IEnumerable<ServiceProfile>> GetAllServices() => Task.FromResult(RegisteredServices.Cast<ServiceProfile>());

		public Task<ServiceProfile> GetServiceByRegisteredId(Guid registeredId)
			=> Task.FromResult(RegisteredServices.SingleOrDefault(m => m.RegisteredID == registeredId) as ServiceProfile);

		public Task<IEnumerable<ServiceProfile>> GetServicesByObjectID(string objectID)
		{
			throw new NotImplementedException();
		}

		public Task<Guid> Register(ServiceProfile service)
		{
			var guid = Guid.NewGuid();
			RegisteredServiceProfile registeredService = (RegisteredServiceProfile)service;
			registeredService.RegisteredID = guid;
			RegisteredServices.Add(registeredService);
			OnServiceRegister?.Invoke(this, service);
			return Task.FromResult(guid);
		}

		public Task<bool> Unregister(Guid registeredId)
		{
			throw new NotImplementedException();
		}
	}
}

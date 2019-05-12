using System;
using Apic.Data.Repositories;

namespace Apic.Data.Context
{
    public interface IUnitOfWork : IEfUnitOfWork, IDisposable
    {
        CustomerRepository Customers { get; }
        DocumentRepository Documents { get; }
    }
}
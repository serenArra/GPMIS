﻿using System.Threading.Tasks;
using Abp.Dependency;

namespace MFAE.Jobs.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}
﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.MultiTenancy.Accounting;
using MFAE.Jobs.Web.Areas.App.Models.Accounting;
using MFAE.Jobs.Web.Controllers;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    public class InvoiceController : JobsControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new InvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}
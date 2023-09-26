using MFAE.Jobs.XRoad;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.XRoad.Exporting;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;
using Newtonsoft.Json;
using System.Dynamic;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.AspNetCore.NodeServices;
using System.Xml.XPath;
using Abp.Domain.Uow;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Logging;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Xml;

namespace MFAE.Jobs.XRoad
{
    [AbpAuthorize(AppPermissions.Pages_XRoadServices)]
    public class XRoadServicesAppService : JobsAppServiceBase, IXRoadServicesAppService
    {
        private readonly INodeServices _nodeServices;
        private readonly IRepository<XRoadService> _xRoadServiceRepository;
        private readonly IRepository<XRoadServiceAttribute> _xRoadServiceAttributeRepository;
        private readonly IRepository<XRoadServiceError> _xRoadServiceErrorRepository;
        private readonly IRepository<XRoadServiceAttributeMapping> _xRoadServiceAttributeMappingRepository;
        private readonly IRepository<IdentificationType> _lookup_identificationTypeRepository;
        private readonly IXRoadServicesExcelExporter _xRoadServicesExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public XRoadServicesAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<IdentificationType> lookup_identificationTypeRepository, INodeServices nodeServices, IRepository<XRoadServiceAttributeMapping> xRoadServiceAttributeMappingRepository, IRepository<XRoadServiceError> xRoadServiceErrorRepository, IRepository<XRoadServiceAttribute> xRoadServiceAttributeRepository, IRepository<XRoadService> xRoadServiceRepository, IXRoadServicesExcelExporter xRoadServicesExcelExporter)
        {
            _nodeServices = nodeServices;
            _xRoadServiceRepository = xRoadServiceRepository;
            _xRoadServiceAttributeRepository = xRoadServiceAttributeRepository;
            _xRoadServiceErrorRepository = xRoadServiceErrorRepository;
            _xRoadServiceAttributeMappingRepository = xRoadServiceAttributeMappingRepository;
            _lookup_identificationTypeRepository = lookup_identificationTypeRepository;
            _xRoadServicesExcelExporter = xRoadServicesExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task<ExpandoObject> GetDynamic(string serviceCode, string requestXML)
        {
            //dynamic obj
            var xRoadServiceID = -1;
            List<XRoadServiceAttribute> xRoadServiceAttributesRequest = new List<XRoadServiceAttribute>();
            List<XRoadServiceAttributeMapping> xRoadServiceAttributeMappingRequestList = new List<XRoadServiceAttributeMapping>();
            List<XRoadServiceAttribute> xRoadServiceAttributesResponse = new List<XRoadServiceAttribute>();
            List<XRoadServiceAttributeMapping> xRoadServiceAttributeMappingResponseList = new List<XRoadServiceAttributeMapping>();
            List<XRoadServiceError> xRoadServiceErrorList = new List<XRoadServiceError>();


            var xRoadURL = await SettingManager.GetSettingValueAsync(XRoadSettingsConsts.XRoadURL);
            var xRoadConsumer = await SettingManager.GetSettingValueAsync(XRoadSettingsConsts.XRoadConsumer);
            var xRoadID = await SettingManager.GetSettingValueAsync(XRoadSettingsConsts.XRoadID);

            //var service = await _xRoadServiceRepository.GetAsync(xRoadServiceID);
            var service = await _xRoadServiceRepository.GetAll().Where(x => x.Code == serviceCode).FirstAsync();
            xRoadServiceID = service.Id;

            xRoadServiceAttributesRequest = await _xRoadServiceAttributeRepository.GetAll()
               .Where(x =>
               x.ServiceAttributeType == XRoadServiceAttributeTypeEnum.Request
               && x.XRoadServiceID == xRoadServiceID
               ).ToListAsync();

            xRoadServiceErrorList = await _xRoadServiceErrorRepository.GetAll()
                .Where(x => x.XRoadServiceId == xRoadServiceID).ToListAsync();

            var xRoadServiceAttributesRequestIds = xRoadServiceAttributesRequest.Select(x => x.Id).ToList();
            xRoadServiceAttributeMappingRequestList = await _xRoadServiceAttributeMappingRepository.GetAll()
                .Where(x => xRoadServiceAttributesRequestIds.Contains(x.AttributeID.Value)).ToListAsync();

            xRoadServiceAttributesResponse = await _xRoadServiceAttributeRepository.GetAll()
                .Where(x =>
                x.ServiceAttributeType == XRoadServiceAttributeTypeEnum.Response
                && x.XRoadServiceID == xRoadServiceID
                ).ToListAsync();
            var xRoadServiceAttributesResponseIds = xRoadServiceAttributesResponse.Select(x => x.Id).ToList();
            xRoadServiceAttributeMappingResponseList = await _xRoadServiceAttributeMappingRepository.GetAll()
                .Where(x => xRoadServiceAttributesResponseIds.Contains(x.AttributeID.Value)).ToListAsync();




            var testCheck = await _nodeServices.InvokeAsync<string>("Scripts/Function.js", 5, "var a=2;var b=3;var c=a * b; return x * c");

            requestXML = string.Format("<request>{0}</request>", requestXML);
            var requestDoc = XDocument.Parse(requestXML);



            foreach (var attributeItem in xRoadServiceAttributesRequest)
            {
                var nodeList = requestDoc.XPathSelectElements(attributeItem.XMLPath);
                foreach (var node in nodeList)
                {
                    var nodeValue = node.Value;
                    if (!string.IsNullOrWhiteSpace(attributeItem.FormatTransition))
                    {
                        var transResult = await _nodeServices.InvokeAsync<string>("Scripts/Function.js", nodeValue, attributeItem.FormatTransition);
                        node.Value = transResult;
                    }

                    var mappedValue = xRoadServiceAttributeMappingRequestList.Where(x => x.AttributeID == attributeItem.Id && x.SourceValue == nodeValue).FirstOrDefault();
                    if (mappedValue != null)
                    {
                        node.Value = mappedValue.DestinationValue;
                    }
                }
            }
            var requestXMLStr = requestDoc.ToString();


            var resultXML = await SendSOAPRequest2(xRoadURL, service.ActionName, requestXMLStr, service.SoapActionName,
                service.ProducerCode, xRoadConsumer, xRoadID, AbpSession.UserId.ToString(), service.VersionNo);
            XDocument doc = XDocument.Parse(resultXML);

            var responseXML = doc.Descendants().Where(x => x.Name.LocalName == "response").First();

            var resultCodeNode = responseXML.XPathSelectElements(service.ResultCodePath);
            foreach (var node in resultCodeNode)
            {
                var nodeValue = node.Value;
                var errorObj = xRoadServiceErrorList.Where(x => x.ErrorCode == nodeValue).FirstOrDefault();
                if (errorObj != null)
                {
                    var errorMessage = CultureInfo.CurrentUICulture.Name == "ar" ? errorObj.ErrorMessageAr : errorObj.ErrorMessageEn;
                    throw new UserFriendlyException(errorMessage);
                }
            }
            foreach (var attributeItem in xRoadServiceAttributesResponse)
            {
                var nodeList = responseXML.XPathSelectElements(attributeItem.XMLPath);
                foreach (var node in nodeList)
                {
                    var nodeValue = node.Value;
                    if (!string.IsNullOrWhiteSpace(attributeItem.FormatTransition) && !string.IsNullOrWhiteSpace(nodeValue))
                    {
                        var transResult = await _nodeServices.InvokeAsync<string>("Scripts/Function.js", nodeValue, attributeItem.FormatTransition);
                        node.Value = transResult;
                    }
                    var mappedValue = xRoadServiceAttributeMappingResponseList.Where(x => x.AttributeID == attributeItem.Id && x.SourceValue == nodeValue).FirstOrDefault();
                    if (mappedValue != null)
                    {
                        node.Value = mappedValue.DestinationValue;
                    }
                }
            }
            foreach (var el in responseXML.Descendants())
            {
                el.RemoveAttributes();
            }
            string jsonText = JsonConvert.SerializeXNode(responseXML);
            dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            return dyn;
        }

        public async Task<PagedResultDto<GetXRoadServiceForViewDto>> GetAll(GetAllXRoadServicesInput input)
        {
            var filteredXRoadServices = _xRoadServiceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ProviderCode.Contains(input.Filter) || e.ActionName.Contains(input.Filter) || e.SoapActionName.Contains(input.Filter) || e.VersionNo.Contains(input.Filter) || e.ProducerCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderCodeFilter), e => e.ProviderCode == input.ProviderCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionNameFilter), e => e.ActionName == input.ActionNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SoapActionNameFilter), e => e.SoapActionName == input.SoapActionNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VersionNoFilter), e => e.VersionNo == input.VersionNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProducerCodeFilter), e => e.ProducerCode == input.ProducerCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredXRoadServices = filteredXRoadServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var xRoadServices = from o in pagedAndFilteredXRoadServices
                                select new GetXRoadServiceForViewDto()
                                {
                                    XRoadService = new XRoadServiceDto
                                    {
                                        Name = o.Name,
                                        ProviderCode = o.ProviderCode,
                                        ResultCodePath = o.ResultCodePath,
                                        ActionName = o.ActionName,
                                        SoapActionName = o.SoapActionName,
                                        VersionNo = o.VersionNo,
                                        ProducerCode = o.ProducerCode,
                                        Description = o.Description,
                                        Id = o.Id,
                                        Status = o.Status
                                    }
                                };

            var totalCount = await filteredXRoadServices.CountAsync();

            return new PagedResultDto<GetXRoadServiceForViewDto>(
                totalCount,
                await xRoadServices.ToListAsync()
            );

        }

        public async Task<List<IdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown()
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await _lookup_identificationTypeRepository.GetAll().OrderBy(e => CultureInfo.CurrentUICulture.Name == "ar" ? e.NameAr : e.NameEn)
                .Select(identificationType => new IdentificationTypeLookupTableDto
                {
                    Id = identificationType.Id,
                    NameAr = identificationType == null || identificationType.NameAr == null ? "" : identificationType.NameAr.ToString(),
                    NameEn = identificationType == null || identificationType.NameEn == null ? "" : identificationType.NameEn.ToString(),
                    IsDefault = identificationType.IsDefault
                }).ToListAsync();
            }
        }

        public async Task<GetXRoadServiceForViewDto> GetXRoadServiceForView(int id)
        {
            var xRoadService = await _xRoadServiceRepository.GetAsync(id);

            var output = new GetXRoadServiceForViewDto { XRoadService = ObjectMapper.Map<XRoadServiceDto>(xRoadService) };

            return output;
        }

        public async Task<CitizensListWithCodesDto> GetInformationBankForViewCitizensListWithCodes(int identificationTypeId, string identificationDocumentNo)
        {
            CitizensListWithCodesDto citizensListWithCodesDto = await GetCitizensListWithCodes(identificationTypeId, identificationDocumentNo);
            return citizensListWithCodesDto;
        }

        public async Task<PassportInfoDto> GetInformationBankForViewPassportInfo(int identificationTypeId, string identificationDocumentNo)
        {
            PassportInfoDto citizensListWithCodesDto = await GetPassportInfo(identificationTypeId, identificationDocumentNo);
            return citizensListWithCodesDto;
        }

        public async Task<CitizenPhotoDto> GetInformationCitizenPhoto(int identificationTypeId, string identificationDocumentNo)
        {
            CitizenPhotoDto citizenPhotoDto = await GetCitizenPhoto(identificationTypeId, identificationDocumentNo);
            return citizenPhotoDto;
        }

        public async Task<InformationBankViewDto> GetInformationBankForView(int identificationTypeId, string identificationDocumentNo)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                InformationBankViewDto informationBankViewDto = new InformationBankViewDto
                {
                    CitizensListWithCodes = new CitizensListWithCodesDto(),
                    PassportInfo = new PassportInfoDto()                   
                };

                
                #region serviceCitizensListWithCodes
                var serviceCitizensListWithCodes = await _xRoadServiceRepository.GetAll().Where(x => x.Code == XRoadServiceConsts.MOICitizensListWithCodes).FirstAsync();
                if (serviceCitizensListWithCodes?.Status == XRoadServiceStatusEnum.Active)
                {
                    informationBankViewDto.IsCitizensListWithCodesEnabled = true;
                    //informationBankViewDto.CitizensListWithCodes = await GetCitizensListWithCodes(identificationTypeId, identificationDocumentNo);
                }
                else
                {
                    informationBankViewDto.IsCitizensListWithCodesEnabled = false;
                    //informationBankViewDto.CitizensListWithCodes = null;
                }
                #endregion

                #region serviceCitizensListWithCodes
                var servicePassportInfo = await _xRoadServiceRepository.GetAll().Where(x => x.Code == XRoadServiceConsts.MOIPassportInfo).FirstAsync();
                if (servicePassportInfo?.Status == XRoadServiceStatusEnum.Active)
                {
                    informationBankViewDto.IsPassportInfoEnabled = true;
                }
                else
                {
                    informationBankViewDto.IsPassportInfoEnabled = false;
                }
                #endregion


               
                #region israelWorkPermit
                var israelWorkPermit = await _xRoadServiceRepository.GetAll().Where(x => x.Code == XRoadServiceConsts.MOICitizenPhoto).FirstAsync();
                if (israelWorkPermit?.Status == XRoadServiceStatusEnum.Active)
                {
                    informationBankViewDto.IsCitizenPictureEnabled = true;
                }
                else
                {
                    informationBankViewDto.IsCitizenPictureEnabled = false;
                }
                #endregion





                return informationBankViewDto;
            }
        }

        private async Task<CitizensListWithCodesDto> GetCitizensListWithCodes(int identificationTypeId, string identificationDocumentNo)
        {
            var citizensListWithCodes = new CitizensListWithCodesDto();
            var obj = await GetDynamic(XRoadServiceConsts.MOICitizensListWithCodes, "<CardID>" + identificationDocumentNo + "</CardID>");
            ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
            ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "citizenWithCode").Select(x => x.Value).FirstOrDefault();
            var propertyList = (IDictionary<String, Object>)view;
            if (propertyList != null)
            {
                citizensListWithCodes.CardID = propertyList.ContainsKey("CardID") ? propertyList["CardID"]?.ToString() : "";
                citizensListWithCodes.FirstName = propertyList.ContainsKey("FirstName") ? propertyList["FirstName"]?.ToString() : "";
                citizensListWithCodes.FatherName = propertyList.ContainsKey("FatherName") ? propertyList["FatherName"]?.ToString() : "";
                citizensListWithCodes.GrandFatherName = propertyList.ContainsKey("GrandFatherName") ? propertyList["GrandFatherName"]?.ToString() : "";
                citizensListWithCodes.FamilyName = propertyList.ContainsKey("FamilyName") ? propertyList["FamilyName"]?.ToString() : "";
                citizensListWithCodes.PreviousFamilyName = propertyList.ContainsKey("PreviousFamilyName") ? propertyList["PreviousFamilyName"]?.ToString() : "";
                citizensListWithCodes.MotherName = propertyList.ContainsKey("MotherName") ? propertyList["MotherName"]?.ToString() : "";
                citizensListWithCodes.FirstNameEN = propertyList.ContainsKey("FirstName_EN") ? propertyList["FirstName_EN"]?.ToString() : "";
                citizensListWithCodes.FatherNameEN = propertyList.ContainsKey("FatherName_EN") ? propertyList["FatherName_EN"]?.ToString() : "";
                citizensListWithCodes.GrandFatherNameEN = propertyList.ContainsKey("GrandFatherName_EN") ? propertyList["GrandFatherName_EN"]?.ToString() : "";
                citizensListWithCodes.FamilyNameEN = propertyList.ContainsKey("FamilyName_EN") ? propertyList["FamilyName_EN"]?.ToString() : "";
                citizensListWithCodes.MotherNameEN = propertyList.ContainsKey("MotherName_EN") ? propertyList["MotherName_EN"]?.ToString() : "";
                citizensListWithCodes.FirstNameHE = propertyList.ContainsKey("FirstName_HE") ? propertyList["FirstName_HE"]?.ToString() : "";
                citizensListWithCodes.FatherNameHE = propertyList.ContainsKey("FatherName_HE") ? propertyList["FatherName_HE"]?.ToString() : "";
                citizensListWithCodes.FamilyNameHE = propertyList.ContainsKey("FamilyName_HE") ? propertyList["FamilyName_HE"]?.ToString() : "";
                citizensListWithCodes.MotherNameHE = propertyList.ContainsKey("MotherName_HE") ? propertyList["MotherName_HE"]?.ToString() : "";
                citizensListWithCodes.GenderNameEN = propertyList.ContainsKey("GenderName_EN") ? propertyList["GenderName_EN"]?.ToString() : "";
                citizensListWithCodes.GenderNameHE = propertyList.ContainsKey("GenderName_HE") ? propertyList["GenderName_HE"]?.ToString() : "";
                var birthDateStr = propertyList.ContainsKey("BirthDate") ? propertyList["BirthDate"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(birthDateStr))
                {
                    citizensListWithCodes.BirthDate = DateTime.Parse(birthDateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
                var deathDateStr = propertyList.ContainsKey("DeathDate") ? propertyList["DeathDate"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(deathDateStr))
                {
                    citizensListWithCodes.DeathDate = DateTime.Parse(deathDateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
                citizensListWithCodes.Governorate = propertyList.ContainsKey("Governorate") ? propertyList["Governorate"]?.ToString() : "";
                citizensListWithCodes.GovernorateEN = propertyList.ContainsKey("Governorate_EN") ? propertyList["Governorate_EN"]?.ToString() : "";
                citizensListWithCodes.BirthCityName = propertyList.ContainsKey("BirthCityName") ? propertyList["BirthCityName"]?.ToString() : "";
                citizensListWithCodes.BirthCountryName = propertyList.ContainsKey("BirthCountryName") ? propertyList["BirthCountryName"]?.ToString() : "";
                citizensListWithCodes.ReligionName = propertyList.ContainsKey("ReligionName") ? propertyList["ReligionName"]?.ToString() : "";
                citizensListWithCodes.ReligionNameEN = propertyList.ContainsKey("ReligionName_EN") ? propertyList["ReligionName_EN"]?.ToString() : "";
                citizensListWithCodes.ReligionNameHE = propertyList.ContainsKey("ReligionName_HE") ? propertyList["ReligionName_HE"]?.ToString() : "";
                citizensListWithCodes.MaritalStatusNameEN = propertyList.ContainsKey("MaritalStatusName_EN") ? propertyList["MaritalStatusName_EN"]?.ToString() : "";
                citizensListWithCodes.MaritalStatusNameHE = propertyList.ContainsKey("MaritalStatusName_HE") ? propertyList["MaritalStatusName_HE"]?.ToString() : "";
                citizensListWithCodes.CityId = propertyList.ContainsKey("CityId") ? propertyList["CityId"]?.ToString() : "";
                citizensListWithCodes.ReligionId = propertyList.ContainsKey("ReligionId") ? propertyList["ReligionId"]?.ToString() : "";
                citizensListWithCodes.RegiondId = propertyList.ContainsKey("RegiondId") ? propertyList["RegiondId"]?.ToString() : "";
                citizensListWithCodes.SexId = propertyList.ContainsKey("SexId") ? propertyList["SexId"]?.ToString() : "";
                citizensListWithCodes.CityNameAR = propertyList.ContainsKey("CityName_AR") ? propertyList["CityName_AR"]?.ToString() : "";
                citizensListWithCodes.CityNameEN = propertyList.ContainsKey("CityName_EN") ? propertyList["CityName_EN"]?.ToString() : "";
            }
            return citizensListWithCodes;
        }

        private async Task<PassportInfoDto> GetPassportInfo(int identificationTypeId, string identificationDocumentNo)
        {
            var passportInfo = new PassportInfoDto();
            var obj = await GetDynamic(XRoadServiceConsts.MOIPassportInfo, "<IDNo>" + identificationDocumentNo + "</IDNo>");
            ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
            ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "passport").Select(x => x.Value).FirstOrDefault();
            var propertyList = (IDictionary<String, Object>)view;
            if (propertyList != null)
            {
                passportInfo.IDNo = propertyList.ContainsKey("IDNo") ? propertyList["IDNo"]?.ToString() : "";
                passportInfo.PassportNo = propertyList.ContainsKey("PassportNo") ? propertyList["PassportNo"]?.ToString() : "";
                var issueDateStr = propertyList.ContainsKey("IssueDate") ? propertyList["IssueDate"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(issueDateStr))
                {
                    passportInfo.IssueDate = DateTime.Parse(issueDateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
                var expireDateStr = propertyList.ContainsKey("ExpireDate") ? propertyList["ExpireDate"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(expireDateStr))
                {
                    passportInfo.ExpireDate = DateTime.Parse(expireDateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
                passportInfo.FirstName = propertyList.ContainsKey("FirstName") ? propertyList["FirstName"]?.ToString() : "";
                passportInfo.FatherName = propertyList.ContainsKey("FatherName") ? propertyList["FatherName"]?.ToString() : "";
                passportInfo.GrandFatherName = propertyList.ContainsKey("GrandFatherName") ? propertyList["GrandFatherName"]?.ToString() : "";
                passportInfo.FamilyName = propertyList.ContainsKey("FamilyName") ? propertyList["FamilyName"]?.ToString() : "";

                passportInfo.MotherName = propertyList.ContainsKey("MotherName") ? propertyList["MotherName"]?.ToString() : "";
                passportInfo.FirstNameEN = propertyList.ContainsKey("FirstNameEN") ? propertyList["FirstNameEN"]?.ToString() : "";
                passportInfo.FatherNameEN = propertyList.ContainsKey("FatherNameEN") ? propertyList["FatherNameEN"]?.ToString() : "";
                passportInfo.GrandFatherNameEN = propertyList.ContainsKey("GrandFatherNameEN") ? propertyList["GrandFatherNameEN"]?.ToString() : "";
                passportInfo.FamilyNameEN = propertyList.ContainsKey("FamilyNameEN") ? propertyList["FamilyNameEN"]?.ToString() : "";
                passportInfo.MotherNameEN = propertyList.ContainsKey("MotherNameEN") ? propertyList["MotherNameEN"]?.ToString() : "";
                var birthDateStr = propertyList.ContainsKey("BirthDate") ? propertyList["BirthDate"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(birthDateStr))
                {
                    passportInfo.BirthDate = DateTime.Parse(birthDateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }

                var genderStr = propertyList.ContainsKey("Sex") ? propertyList["Sex"]?.ToString() : "";
                if (!string.IsNullOrWhiteSpace(genderStr))
                {
                    passportInfo.SexId = int.Parse(genderStr);
                }
            }
            return passportInfo;
        }

        private async Task<CitizenPhotoDto> GetCitizenPhoto(int identificationTypeId, string identificationDocumentNo)
        {
            var pictureInfo = new CitizenPhotoDto();
            var obj = await GetDynamic(XRoadServiceConsts.MOICitizenPhoto, "<IDNo>" + identificationDocumentNo + "</IDNo>");
            ExpandoObject propertyNames = (ExpandoObject)obj.Where(v => v.Key == "response").Select(x => x.Value).FirstOrDefault();
            ExpandoObject view = (ExpandoObject)propertyNames.Where(v => v.Key == "service1Object").Select(x => x.Value).FirstOrDefault();
            var propertyList = (IDictionary<String, Object>)view;
            if (propertyList != null)
            {
                pictureInfo.CitizenPicture = propertyList.ContainsKey("ObjectContent") ? propertyList["ObjectContent"]?.ToString() : "";

            }
            return pictureInfo;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServices_Edit)]
        public async Task<GetXRoadServiceForEditOutput> GetXRoadServiceForEdit(EntityDto input)
        {
            var xRoadService = await _xRoadServiceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetXRoadServiceForEditOutput { XRoadService = ObjectMapper.Map<CreateOrEditXRoadServiceDto>(xRoadService) };

            return output;
        }

        public async Task<CreateOrEditXRoadServiceDto> CreateOrEdit(CreateOrEditXRoadServiceDto input)
        {
            if (input.Id == null)
            {
                var result = await Create(input);
                return result;
            }
            else
            {
                await Update(input);
                return input;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServices_Create)]
        protected virtual async Task<CreateOrEditXRoadServiceDto> Create(CreateOrEditXRoadServiceDto input)
        {
            var xRoadService = ObjectMapper.Map<XRoadService>(input);

            var resultId = await _xRoadServiceRepository.InsertAndGetIdAsync(xRoadService);
            var result = await _xRoadServiceRepository.GetAsync(resultId);
            var output = ObjectMapper.Map<CreateOrEditXRoadServiceDto>(result);
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServices_Edit)]
        protected virtual async Task Update(CreateOrEditXRoadServiceDto input)
        {
            var xRoadService = await _xRoadServiceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, xRoadService);
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServices_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _xRoadServiceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetXRoadServicesToExcel(GetAllXRoadServicesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (XRoadServiceStatusEnum)input.StatusFilter
                        : default;

            var filteredXRoadServices = _xRoadServiceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ProviderCode.Contains(input.Filter) || e.ResultCodePath.Contains(input.Filter) || e.ActionName.Contains(input.Filter) || e.SoapActionName.Contains(input.Filter) || e.VersionNo.Contains(input.Filter) || e.ProducerCode.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderCodeFilter), e => e.ProviderCode.Contains(input.ProviderCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResultCodePathFilter), e => e.ResultCodePath.Contains(input.ResultCodePathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActionNameFilter), e => e.ActionName.Contains(input.ActionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SoapActionNameFilter), e => e.SoapActionName.Contains(input.SoapActionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VersionNoFilter), e => e.VersionNo.Contains(input.VersionNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProducerCodeFilter), e => e.ProducerCode.Contains(input.ProducerCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter));

            var query = (from o in filteredXRoadServices
                         select new GetXRoadServiceForViewDto()
                         {
                             XRoadService = new XRoadServiceDto
                             {
                                 Name = o.Name,
                                 ProviderCode = o.ProviderCode,
                                 ResultCodePath = o.ResultCodePath,
                                 ActionName = o.ActionName,
                                 SoapActionName = o.SoapActionName,
                                 VersionNo = o.VersionNo,
                                 ProducerCode = o.ProducerCode,
                                 Description = o.Description,
                                 Status = o.Status,
                                 Code = o.Code,
                                 Id = o.Id
                             }
                         });

            var xRoadServiceListDtos = await query.ToListAsync();

            return _xRoadServicesExcelExporter.ExportToFile(xRoadServiceListDtos);
        }

        #region XRoad Request managment
        private async Task<string> SendSOAPRequest2(string url, string action, string requestStr, string soapAction,
           string producer, string consumer, string id, string userId, string versioNO)
        {





            var xmlStr = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:x=""http://x-road.eu/xsd/x-road.xsd"" xmlns:prod=""http://{0}.x-road.eu/producer"">
                    <soapenv:Header>
                        <x:service>{0}.{2}.{6}</x:service>
                        <x:id>{3}</x:id>
                        <x:userId>{4}</x:userId>
                        <x:producer>{0}</x:producer>
                        <x:consumer>{1}</x:consumer>
                    </soapenv:Header>
                    <soapenv:Body>
                      <prod:{2}>
                            {5}
                      </prod:{2}>
                    </soapenv:Body>
                  </soapenv:Envelope>";


            //string parms = string.Join(string.Empty, parameters.Select(kv => String.Format("<{0}>{1}</{0}>", kv.Key, kv.Value)).ToArray());
            //userId = string.IsNullOrEmpty(userId) ? "0000000000" : userId;
            var xmlWithParameters = String.Format(xmlStr, producer, consumer, action, id, userId, requestStr, versioNO);


            using (var client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression =
                DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            { Timeout = new TimeSpan(0, 0, 50) })
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post
                };

                request.Content = new StringContent(xmlWithParameters, Encoding.UTF8, "text/xml");

                //request.Headers.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                request.Headers.Add("SOAPAction", soapAction);
                request.Headers.Add("Accept-Encoding", "gzip,deflate");

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                /*
                 
                 webRequest.Method = "POST";
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Headers.Add("SOAPAction", soapAction);
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
                 */
                Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                Stream stream = streamTask.Result;
                var sr = new StreamReader(stream);
                var soapResponse = XDocument.Load(sr);
                //Console.WriteLine(soapResponse);
                return soapResponse.ToString();
                //var xml = soapResponse.Descendants(myns + "GetStuffResult").FirstOrDefault().ToString();
                //var purchaseOrderResult = StaticMethods.Deserialize<GetStuffResult>(xml);
            }
        }

        private async Task<string> SendSOAPRequest(string url, string action, string request, string soapAction,
            string producer, string consumer, string id, string userId, string versioNO)
        {



            // Create the SOAP envelope
            XmlDocument soapEnvelopeXml = new XmlDocument();
            var xmlStr = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:x=""http://x-road.eu/xsd/x-road.xsd"" xmlns:prod=""http://{0}.x-road.eu/producer"">
                    <soapenv:Header>
                        <x:service>{0}.{2}.{6}</x:service>
                        <x:id>{3}</x:id>
                        <x:userId>{4}</x:userId>
                        <x:producer>{0}</x:producer>
                        <x:consumer>{1}</x:consumer>
                    </soapenv:Header>
                    <soapenv:Body>
                      <prod:{2}>
                            {5}
                      </prod:{2}>
                    </soapenv:Body>
                  </soapenv:Envelope>";


            //string parms = string.Join(string.Empty, parameters.Select(kv => String.Format("<{0}>{1}</{0}>", kv.Key, kv.Value)).ToArray());
            //userId = string.IsNullOrEmpty(userId) ? "0000000000" : userId;
            var xmlWithParameters = String.Format(xmlStr, producer, consumer, action, id, userId, request, versioNO);

            //soapEnvelopeXml.LoadXml(xmlWithParameters);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Headers.Add("SOAPAction", soapAction);
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");

            var result = await GetResponseAsStringAsync(webRequest, xmlWithParameters);

            Logger.Log(LogSeverity.Info, $"request: {xmlWithParameters}  \n response: {result}");
            return result;
        }

        private async Task<string> GetResponseAsStringAsync(HttpWebRequest webRequest, string post)
        {
            if (post != null)
            {
                //webRequest.Method = "POST";
                using (Stream postStream = await webRequest.GetRequestStreamAsync())
                {
                    byte[] postBytes = Encoding.UTF8.GetBytes(post);
                    await postStream.WriteAsync(postBytes, 0, postBytes.Length);
                    await postStream.FlushAsync();
                }
            }
            try
            {
                Task<string> Response;
                using (var response = (HttpWebResponse)await webRequest.GetResponseAsync())
                using (Stream streamResponse = response.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(streamResponse))
                {
                    Response = streamReader.ReadToEndAsync();
                }
                // Ignore Warning: I don't want to await for this call, its for logging
                Response.ContinueWith(async x =>
                {
                    //_logger.Info(new HttpRequestLog
                    //{
                    //    Request = webRequest,
                    //    Url = webRequest.RequestUri.ToString(),
                    //    Method = webRequest.Method,
                    //    PostBody = post,
                    //    Response = await x
                    //});
                });

                return await Response;
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, ex.Message, ex);
                throw (ex);
            }
        }




        #endregion
    }
}
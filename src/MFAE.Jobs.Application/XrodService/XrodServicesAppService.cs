using Abp.Domain.Repositories;
using MFAE.Jobs.SoapApiRequest;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XrodService.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MFAE.Jobs.XrodService
{
    public class XrodServicesAppService : JobsAppServiceBase, IXrodServicesAppService
    {
        private readonly Random _random = new Random();

        private readonly ISoapApiRequestService _SoapApiRequestService;
        private readonly IRepository<XRoadMapping> _XrodeMapppingRepository;
        public XrodServicesAppService(ISoapApiRequestService soapApiRequestService, IRepository<XRoadMapping> xrodeMapppingRepository)
        {
            _SoapApiRequestService = soapApiRequestService;
            _XrodeMapppingRepository = xrodeMapppingRepository;
        }
        public async Task<CitizenInformation> GetCitizenInformation(string CardNo)
        {
            string baseURL = "https://localhost:44376/WebService1.asmx/Getmystring";
            var result = await _SoapApiRequestService.GetRequestSaopObject<mystring>(baseURL);

            var xrodData = new XrodCitizenInformation()
            {
                CardNo = CardNo,
                BirthPlace = "أبوديس",
                DateOfBirth = DateTime.Now.AddYears(-24),
                ArFirstName = "ماجد",
                ArSecondName = "جمال",
                ArThirdName = "محمد",
                ArFourthName = "عواد",
                EnFirstName = "Majed",
                EnSecondName = "Jamal",
                EnThirdName = "Mohammad",
                EnFourthName = "Awwad",
                Gender = _random.Next(2) > 0 ? "g" : "f",
                MaritalStatus = _random.Next(2) > 0 ? "m" : "s"
            };
            
            var data = new CitizenInformation()
            {
                CardNo = xrodData.CardNo,
                BirthPlace = xrodData.BirthPlace,
                DateOfBirth = xrodData.DateOfBirth,
                ArFirstName = xrodData.ArFirstName,
                ArSecondName = xrodData.ArSecondName,
                ArThirdName = xrodData.ArThirdName,
                ArFourthName = xrodData.ArFourthName,
                EnFirstName = xrodData.EnFirstName,
                EnSecondName = xrodData.EnSecondName,
                EnThirdName = xrodData.EnThirdName,
                EnFourthName = xrodData.EnFourthName

            };
            return data;
        }

        private Dictionary<string, Dictionary<string, int>> getLookupMappingDictionary()
        {
            var mappingDictionary = new Dictionary< /*LookupName*/ string, Dictionary</*ItemLookupCode*/string,/*SystemId*/ int>>();

            mappingDictionary.Add("GenderTypes", new Dictionary<string, int>());
            mappingDictionary["GenderTypes"].Add("g", 1);
            mappingDictionary["GenderTypes"].Add("f", 2);

            mappingDictionary.Add("MaritalStatuses", new Dictionary<string, int>());
            mappingDictionary["MaritalStatuses"].Add("m", 1);
            mappingDictionary["MaritalStatuses"].Add("s", 2);

            return mappingDictionary;
        }
        private async Task<List<XRoadMapping>> getLookupMapping(XRoadServicesEnum servicename)
        {
            var result = await _XrodeMapppingRepository.GetAll()/*.Where(e => e.ServiceName == servicename)*/.ToListAsync();
            return result;

        }
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}

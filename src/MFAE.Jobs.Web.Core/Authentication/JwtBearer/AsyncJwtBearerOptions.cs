using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MFAE.Jobs.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly JobsAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new JobsAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}

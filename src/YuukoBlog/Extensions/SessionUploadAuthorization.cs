namespace YuukoBlog.Extensions
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Pomelo.AspNetCore.Extensions.BlobStorage;

    public class SessionUploadAuthorization : IBlobUploadAuthorizationProvider
    {
        private readonly IServiceProvider services;

        public SessionUploadAuthorization(IServiceProvider provider)
        {
            services = provider;
        }

        public bool IsAbleToUpload()
        {
            var val = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session.GetString("Admin");
            return val == "true";
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    using Pomelo.AspNetCore.Extensions.BlobStorage;
    using YuukoBlog.Extensions;

    public static class SignedUserUploadAuthorizationProviderServiceCollectionExtensions
    {
        public static IBlobStorageBuilder AddSessionUploadAuthorization(this IBlobStorageBuilder self)
        {
            self.Services.AddSingleton<IBlobUploadAuthorizationProvider, SessionUploadAuthorization>();
            return self;
        }
    }
}
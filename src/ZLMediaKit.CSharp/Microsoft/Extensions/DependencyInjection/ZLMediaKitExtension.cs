using Microsoft.Extensions.Configuration;
using System;
using ZLMediaKit.CSharp;
using ZLMediaKit.CSharp.HostedService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZLMediaKitExtension
    {
        /// <summary>
        /// 注册ZLMediaKit服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option">ZLMediaKit的配置</param>
        /// <returns></returns>
        public static IServiceCollection AddMediaKitCSharp(this IServiceCollection services,Action<ZLMediaKitSettings> option)
        {
            services.Configure<ZLMediaKitSettings>(option);
            return AddMediaKitCSharp(services);
        }

        /// <summary>
        /// 注册ZLMediaKit服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">ZLMediaKit的配置</param>
        /// <returns></returns>
        public static IServiceCollection AddMediaKitCSharp(this IServiceCollection services ,IConfiguration configuration)
        {
            services.Configure<ZLMediaKitSettings>(configuration.GetSection("ZLMediaKit"));
            return AddMediaKitCSharp(services);
        }

        private static IServiceCollection AddMediaKitCSharp(this IServiceCollection services)
        {
            
            return services
                    .AddHostedService<ZLMediaKitInitService>();
        }
    }
}

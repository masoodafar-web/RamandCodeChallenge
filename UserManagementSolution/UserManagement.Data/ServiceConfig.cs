using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.Data;

public static class ServiceConfig
{
    public static IServiceCollection AddDataLayer(this IServiceCollection service)
    {
       
        service.AddSingleton<DapperContext>();
        return service;
    }
}
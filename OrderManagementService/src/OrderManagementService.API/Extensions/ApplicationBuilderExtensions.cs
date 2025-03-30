namespace OrderManagementService.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceCollection AddCorsApp(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://myfrontenddomain.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });

                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Allow all origins (use only for development)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return services;
        }

    }
}

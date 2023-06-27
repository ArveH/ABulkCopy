namespace ABulkCopy.Cmd.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMssServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, MssContext>();
        services.AddTransient<IMssSystemTables, MssSystemTables>();
        services.AddSingleton<IMssTableReader, MssTableReader>();
        services.AddSingleton<IMssTableSchema, MssTableSchema>();
        services.AddSingleton<IMssColumnFactory, MssColumnFactory>();
        
        return services;
    }

    public static IServiceCollection AddPgServices(
        this IServiceCollection services)
    {
        services.AddSingleton<PgContext>();
        services.AddSingleton<IDbContext>(s => s.GetRequiredService<PgContext>());
        services.AddSingleton<IPgContext>(s => s.GetRequiredService<PgContext>());
        services.AddSingleton<IPgCmd, PgCmd>();
        services.AddSingleton<ITypeConverter, PgTypeMapper>();
        services.AddSingleton<IPgColumnFactory, PgColumnFactory>();
        services.AddTransient<ISchemaReader, PgSchemaReader>();

        return services;
    }
}

namespace ABulkCopy.Cmd.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMssServices(this IServiceCollection services)
    {
        services.AddTransient<IMssSystemTables, MssSystemTables>();
        services.AddSingleton<IMssTableReader, MssTableReader>();
        services.AddSingleton<IMssTableSchema, MssTableSchema>();
        services.AddSingleton<IMssColumnFactory, MssColumnFactory>();
        
        return services;
    }

    public static IServiceCollection AddPgServices(this IServiceCollection services)
    {
        services.AddSingleton<IADataReader, PgDataReader>();
        services.AddSingleton<ISchemaReader, PgSchemaReader>();
        services.AddSingleton<ITypeConverter, PgTypeMapper>();
        services.AddSingleton<IPgColumnFactory, PgColumnFactory>();

        return services;
    }
}

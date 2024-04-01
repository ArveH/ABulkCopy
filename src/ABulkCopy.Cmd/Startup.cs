using Microsoft.Extensions.Logging;

namespace ABulkCopy.Cmd;

public static class Startup
{
    public static HostApplicationBuilder ConfigureServices(
        this HostApplicationBuilder builder, 
        Rdbms rdbms,
        IConfigurationRoot configuration)
    {
        builder.Services.ConfigureServices(rdbms, configuration);
        return builder;
    }

    public static void ConfigureServices(
        this IServiceCollection services,
        Rdbms rdbms,
        IConfigurationRoot configuration)
    {
        var loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger);
        });
        services.AddSingleton(loggerFactory);
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(Log.Logger);
        services.AddSingleton<ISchemaWriter, SchemaWriter>();
        services.AddSingleton<IDataWriter, DataWriter>();
        services.AddSingleton<ISchemaReaderFactory, SchemaReaderFactory>();
        services.AddSingleton<IADataReaderFactory, ADataReaderFactory>();
        services.AddSingleton<ITableReaderFactory, TableReaderFactory>();
        services.AddSingleton<ISelectCreator, SelectCreator>();
        services.AddSingleton<ICopyOut, CopyOut>();
        services.AddSingleton<ICopyIn, CopyIn>();
        services.AddSingleton<IMappingFactory, MappingFactory>();
        services.AddSingleton<IFileSystem>(new FileSystem());
        services.AddTransient<IDataFileReader, DataFileReader>();
        services.AddTransient<IDependencyGraph, DependencyGraph>();
        services.AddTransient<IVisitorFactory, VisitorFactory>();
        services.AddSingleton<INodeFactory, NodeFactory>();
        services.AddSingleton<IQueryBuilderFactory, QueryBuilderFactory>();
        services.AddSingleton<IIdentifier, Identifier>();
        if (rdbms == Rdbms.Mss) services.AddMssServices();
        if (rdbms == Rdbms.Pg) services.AddPgServices();
    }
}
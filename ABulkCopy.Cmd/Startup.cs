using Microsoft.Extensions.Logging;

namespace ABulkCopy.Cmd;

public static class Startup
{
    public static HostApplicationBuilder ConfigureServices(
        this HostApplicationBuilder builder, 
        Rdbms rdbms,
        IConfigurationRoot configuration)
    {
        var loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger);
        });
        builder.Services.AddSingleton(loggerFactory);

        builder.Services.AddSingleton(configuration);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddSingleton<ISchemaWriter, SchemaWriter>();
        builder.Services.AddSingleton<IDataWriter, DataWriter>();
        builder.Services.AddSingleton<ISchemaReaderFactory, SchemaReaderFactory>();
        builder.Services.AddSingleton<IADataReaderFactory, ADataReaderFactory>();
        builder.Services.AddSingleton<ITableReaderFactory, TableReaderFactory>();
        builder.Services.AddSingleton<ISelectCreator, SelectCreator>();
        builder.Services.AddSingleton<ICopyOut, CopyOut>();
        builder.Services.AddSingleton<ICopyIn, CopyIn>();
        builder.Services.AddSingleton<IMappingFactory, MappingFactory>();
        builder.Services.AddSingleton<IFileSystem>(new FileSystem());
        builder.Services.AddTransient<IDataFileReader, DataFileReader>();
        builder.Services.AddTransient<IDependencyGraph, DependencyGraph>();
        builder.Services.AddTransient<IVisitorFactory, VisitorFactory>();
        builder.Services.AddSingleton<INodeFactory, NodeFactory>();
        builder.Services.AddSingleton<IQuoter, Quoter>();
        if (rdbms == Rdbms.Mss) builder.Services.AddMssServices();
        if (rdbms == Rdbms.Pg) builder.Services.AddPgServices();

        return builder;
    }
}
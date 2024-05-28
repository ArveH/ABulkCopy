namespace ABulkCopy.Cmd.Internal.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMssServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, MssContext>();
        services.AddSingleton<IMssSystemTables, MssSystemTables>();
        services.AddSingleton<IMssCmd, MssCmd>();
        services.AddSingleton<IMssTableSchema, MssTableSchema>();
        services.AddSingleton<IMssColumnFactory, MssColumnFactory>();
        services.AddSingleton<IQueryBuilderFactory, ASqlServer.QueryBuilderFactory>();

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
        services.AddSingleton<IPgSystemTables, PgSystemTables>();
        services.AddTransient<IPgBulkCopy, PgBulkCopy>();
        services.AddSingleton<IPgParser, PgParser>();
        services.AddSingleton<ITokenFactory, TokenFactory>();
        services.AddSingleton<AParser.Tree.INodeFactory, AParser.Tree.NodeFactory>();
        services.AddSingleton<ISqlTypes, SqlTypes>();
        services.AddTransient<ITokenizerFactory, TokenizerFactory>();
        services.AddTransient<AParser.Tree.IParseTree, AParser.Tree.ParseTree>();
        services.AddSingleton<IQueryBuilderFactory, APostgres.QueryBuilderFactory>();

        return services;
    }
}

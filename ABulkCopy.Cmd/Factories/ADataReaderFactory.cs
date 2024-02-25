namespace ABulkCopy.Cmd.Factories;

public class ADataReaderFactory : IADataReaderFactory
{
    private readonly IPgContext _pgContext;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public ADataReaderFactory(
        IPgContext pgContext,
        IQueryBuilderFactory queryBuilderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _pgContext = pgContext;
        _queryBuilderFactory = queryBuilderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public IADataReader Get(Rdbms rdbms)
    {
        if (rdbms == Rdbms.Pg)
        {
            return new PgDataReader(
            _pgContext,
            _queryBuilderFactory,
            new DataFileReader(_fileSystem, _logger),
            _logger);
        }

        throw new NotSupportedException($"Rdbms {rdbms} is not supported yet");
    }
}
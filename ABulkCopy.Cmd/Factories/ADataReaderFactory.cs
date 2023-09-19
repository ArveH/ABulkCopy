namespace ABulkCopy.Cmd.Factories;

public class ADataReaderFactory : IADataReaderFactory
{
    private readonly IPgContext _pgContext;
    private readonly IQuoter _quoter;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public ADataReaderFactory(
        IPgContext pgContext,
        IQuoter quoter,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _pgContext = pgContext;
        _quoter = quoter;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public IADataReader Get(Rdbms rdbms)
    {
        if (rdbms == Rdbms.Pg)
        {
            return new PgDataReader(
            _pgContext,
            _quoter,
            new DataFileReader(_fileSystem, _logger),
            _logger);
        }

        throw new NotSupportedException($"Rdbms {rdbms} is not supported yet");
    }
}
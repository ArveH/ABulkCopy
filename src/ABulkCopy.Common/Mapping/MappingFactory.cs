﻿namespace ABulkCopy.Common.Mapping;

public class MappingFactory : IMappingFactory
{
    private readonly ILogger _logger;
    private readonly IMapping _mapping;

    public MappingFactory(
        IConfiguration config,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _logger = logger.ForContext<MappingFactory>();
        _mapping = new Mapping(
            locations: GetDefaultMssToPgLocationMappings());
        var fileName = config.SafeGet(Constants.Config.MappingsFile);
        SetDefaultMssToPgMappings();
        if (fileName == string.Empty)
        {
            return;
        }

        SetMappingsFromFile(fileSystem, fileName);
    }

    public IMapping GetMappings() => _mapping;
    public bool ConvertBitToBool { get; private set; } = true;

    private static Dictionary<string, string?> GetDefaultMssToPgLocationMappings()
    {
        return new()
        {
            { "", null },
            { "PRIMARY", null }
        };
    }

    private void SetMappingsFromFile(IFileSystem fileSystem, string fileName)
    {
        try
        {
            var mappingsTxt = fileSystem.File.ReadAllText(fileName);
            if (string.IsNullOrWhiteSpace(mappingsTxt))
            {
                _logger.Error("The mappings file '{File}' was empty", fileName);
                throw new MappingsFileException($"The mappings file '{fileName}' was empty");
            }

            var mappingsFromFile = JsonSerializer.Deserialize<Mapping>(mappingsTxt);
            if (mappingsFromFile == null)
            {
                _logger.Error("The mappings file '{File}' was not valid JSON", fileName);
                throw new MappingsFileException($"The mappings file '{fileName}' was not valid JSON");
            }
            foreach (var (key, value) in mappingsFromFile.Schemas)
            {
                _mapping.Schemas[key] = value;
            }
            foreach (var (key, value) in mappingsFromFile.Collations)
            {
                _mapping.Collations[key] = value;
            }
            foreach (var (key, value) in mappingsFromFile.ColumnTypes)
            {
                _mapping.ColumnTypes[key] = value;
                CheckBitConversion(key, value);
            }
        }
        catch (MappingsFileException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error reading mappings file {File}. Current directory is {CurrentDir}.",
                fileName, fileSystem.Directory.GetCurrentDirectory());
            throw new MappingsFileException($"Error reading mappings file '{fileName}'", ex);
        }
    }

    private void SetDefaultMssToPgMappings()
    {
        _mapping.Schemas.Add("", "public");
        _mapping.Schemas.Add("dbo", "public");
        _mapping.Collations.Add("SQL_Latin1_General_CP1_CI_AI", "en_ci_ai");
        _mapping.Collations.Add("SQL_Latin1_General_CP1_CI_AS", "en_ci_as");
        foreach (var (key, value) in GetDefaultMssToPgColumnMappings())
        {
            _mapping.ColumnTypes.Add(key, value);
            CheckBitConversion(key, value);
        }
    }

    private Dictionary<string, string> GetDefaultMssToPgColumnMappings()
    {
        return new Dictionary<string, string>
            {
                {MssTypes.Binary, PgTypes.ByteA},
                {MssTypes.Bit, PgTypes.Boolean},
                {MssTypes.DateTime, PgTypes.TimestampTz},
                {MssTypes.DateTime2, PgTypes.TimestampTz},
                {MssTypes.DateTimeOffset, PgTypes.TimestampTz},
                {MssTypes.Float, PgTypes.DoublePrecision},
                {MssTypes.NChar, PgTypes.Char},
                {MssTypes.Text, PgTypes.VarChar},
                {MssTypes.NText, PgTypes.VarChar},
                {MssTypes.NVarChar, PgTypes.VarChar},
                {MssTypes.SmallDateTime, PgTypes.Timestamp},
                {MssTypes.TinyInt, PgTypes.SmallInt},
                {MssTypes.UniqueIdentifier, PgTypes.Uuid },
                {MssTypes.Image, PgTypes.ByteA},
                {MssTypes.VarBinary, PgTypes.ByteA}
            };
    }

    private void CheckBitConversion(string key, string value)
    {
        if (key == MssTypes.Bit && value != "boolean")
        {
            ConvertBitToBool = false;
        }
    }
}
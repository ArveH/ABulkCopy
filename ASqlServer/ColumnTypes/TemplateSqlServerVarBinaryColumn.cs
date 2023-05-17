namespace ASqlServer.ColumnTypes
{
    public class TemplateSqlServerVarBinaryColumn : TemplateSqlServerColumn
    {
        private readonly string _typeString;
        public TemplateSqlServerVarBinaryColumn(string name, int length, bool isNullable, string def)
            : base(name, ColumnType.Blob, isNullable, false, def)
        {
            if (length <= 0)
            {
                _typeString = "varbinary(max)";
            }
            else
            {
                Details["Length"] = length;
                _typeString = $"varbinary({length})";
                Type = ColumnType.VarRaw;
            }
        }

        public override string InternalTypeName()
        {
            return _typeString;
        }

        public override string GetColumnDefinition()
        {
            string defaultValue = "";
            if (!string.IsNullOrEmpty(Default))
            {
                defaultValue = $"default {Default} ";
            }
            string notNullConstraint = IsNullable ? "null " : "not null ";

            return $"{InternalTypeName()} {defaultValue}{notNullConstraint}";
        }

        public override string ToString(object value)
        {
            if (Type == ColumnType.Blob) throw new NotImplementedException("Column.ToFile for BLOB");

            return Convert.ToBase64String((byte[])value);
        }

        public override object ToInternalType(string value)
        {
            if (Type == ColumnType.Blob) throw new NotImplementedException("Column.ToInternalType for BLOB");

            if (value == null)
            {
                return DBNull.Value;
            }
            return Convert.FromBase64String(value);
        }

        public override Type GetDotNetType()
        {
            return typeof(byte[]);
        }
    }
}
# ABulkCopy

The main purpose of this tool is to copy database tables from a source database to a destination database. To move data, you run it twice. First, you copy tables from the source database into files. Then you use the same files to create tables and copy data into the destination database.

> NOTE: I currently only support copy out from an SQL Server database, and into a Postgres database. There could also be data types that are not supported yet.

To get started, build the ABulkCopy.Cmd Console application. 

Run:  
```powershell
.\ABulkCopy.Cmd.exe --help
```

to see what commandline parameters there are. 
 
You typically want to do something like this:  
```powershell
.\ABulkCopy.Cmd.exe
  -d out
  -r Mss
  -c "Server=.;Database=u4erp;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  -l D:\Temp\abulkcopy\Logs\local_u4erp_out.log
  -f d:\temp\abulkcopy\LocalDbs\u4erp
```  
to copy data from SQL Server. 

Then something like this:
```powershell
.\ABulkCopy.Cmd.exe
  -d in
  -r Pg
  -c "Server=localhost;Port=5432;Database=u4erp;User Id=postgres;Include Error Detail=True;Command Timeout=1800;"
  -l D:\Temp\abulkcopy\Logs\local_u4erp_in.log
  -f D:\temp\abulkcopy\LocalDbs\u4erp
```  
to copy it into a Postgres database.
 
Remember that the Postgres database you copy into should be existing, empty, and containing these collations (for case insensitivity):  
```sql
DROP COLLATION IF EXISTS en_ci_ai;
CREATE COLLATION en_ci_ai (provider = 'icu', locale = 'en-u-ks-level1', deterministic = false);

DROP COLLATION IF EXISTS en_ci_ai_like;
CREATE COLLATION en_ci_ai_like (provider = 'icu',  locale = 'en-u-ks-level1',  deterministic = true);

DROP COLLATION IF EXISTS en_ci_as;
CREATE COLLATION en_ci_as (provider = 'icu', locale = 'en-u-ks-level2', deterministic = false);
 
DROP COLLATION IF EXISTS en_ci_as_like;
CREATE COLLATION en_ci_as_like (provider = 'icu',  locale = 'en-u-ks-level2',  deterministic = true);
```
The collations are needed if the SQL Server database you copied from uses the SQL_Latin1_General_CP1_CI_AI and SQL_Latin1_General_CP1_CI_AS collations

># This is currently under development. File formats, command parameters, and functionality can (and most likely will) change.

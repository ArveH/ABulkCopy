# A database copy tool

This is a .NET 8 Console application. The main purpose of this tool is to copy database tables from a source database to a destination database. To move data, you run it twice.

First, you copy tables from the source database into files. Then you use the same files to create tables and copy data into the destination database.

To get started, build the ABulkCopy.Cmd Console application. Then run .\abulkcopy.cmd.exe --help to see the parameters.

> NOTE: This is currently under development. File formats, command parameters, and functionality can (and most likely will) change.

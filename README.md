# Moudou.DBStructureReader

Easy to read database structure.

## Version
- 0.0.1-Preview

## Environment
- SQL Server
- .Net Framework 4.6.2

## Sample
```csharp
string connectionString = "Server=.\SQLExpress;Database=MCLDev;Trusted_Connection=True;";
TableInfo tInfo = new Moudou.DBStructureReader.DBSchemaReader().GetSchema(connectionString);
```

## Parameter
ConnectionString: 
> A connection string of database, or a connection name in app.config/web.config.
When it's empty, default value is 'DefaultConnection'.

## Return Type

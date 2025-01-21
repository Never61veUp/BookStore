var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume(isReadOnly: false)
    .AddDatabase("BookStoreDb");

var migrations = builder.AddProject<Projects.BookStore_PostgreSql_MigrationServicee>("migrations")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddProject<Projects.BookStore_Host>("host")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WaitFor(migrations);



builder.Build().Run();


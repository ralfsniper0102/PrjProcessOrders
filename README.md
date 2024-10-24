# PrjProcessOrders
 

#Comandos de migração e criação do banco de dados do projeto


dotnet ef migrations add InitialMigration --project ProjProcessOrders.Infrastructure --startup-project ProjProcessOrders.ProcessingAPI --context ApplicationDbContext


dotnet ef database update --project ProjProcessOrders.Infrastructure --startup-project ProjProcessOrders.ProcessingAPI --context ApplicationDbContext
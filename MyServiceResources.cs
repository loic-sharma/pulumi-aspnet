using System.Collections.Generic;

namespace pulumi_aspnet
{
    public class MyServiceResources : Pulumi.ResourceProvider<MyServicesResource>
    {
        // The name of the Pulumi resource is the class name followed by the property name.
        // This behavior can be overriden by attributes or by the fluent APIs.
        public Pulumi.Azure.Core.ResourceGroup ResourceGroup { get; set; }
        public Pulumi.Azure.Sql.SqlServer DbServer { get; set; }
        public Pulumi.Azure.Sql.Database Db { get; set; }
        public Pulumi.Azure.Storage.Account Storage { get; set; }
        public Pulumi.Azure.Storage.Container Container { get; set; }
        public Pulumi.Azure.AppService.Plan Plan { get; set; }
        public Pulumi.Azure.AppService.AppService App { get; set; }

        protected override void OnProvisioning(ResourceBuilder<MyServiceResources> builder)
        {
            builder.Export("Foo", "bar");

            builder.Resource(b => b.ResourceGroup)
                .HasLocation("northcentralus");

            builder.Resource(b => b.DbServer)
                .HasResourceGroupName(ResourceGroup.Name)
                .HasLocation(ResourceGroup.Location)
                .HasAdministratorLogin(builder.Config("db-username"))
                .HasAdministratorLoginPassword(builder.Config("db-password"))
                .HasVersion("12.0");

            builder.Resource(b => b.Db)
                .HasResourceGroupName(ResourceGroup.Name)
                .HasLocation(ResourceGroup.Location)
                .HasServerName(DbServer.Name)
                .HasEdition("Basic");

            builder.Resource(b => b.Storage)
                // ResourceGroupName + Location
                .HasAccountTier("Standard")
                .HasAccountReplicationType("LRS");

            builder.Resource(b => b.Container)
                .HasResourceGroupName(ResourceGroup.Name)
                .HasStorageAccountName(Storage.Name);

            builder.Resource(b => b.Plan)
                // ResourceGroupName + Location
                .HasSku(size: "F1", tier: "Free");

            builder.Resource(b => b.App)
                // ResourceGroupName + Location
                .HasAppServicePlanId(Plan.Id)
                .HasAppSettings(new Dictionary<string, string>
                {
                    { "Database:FQDN", DbServer.FullyQualifiedDomainName },
                    { "Database:Username", DbServer.AdministratorLogin },
                    { "Database:Password", DbServer.AdministratorLoginPassword },

                    { "Storage:AccountName", Storage.Name },
                    { "Storage:Container", Container.Name },
                    { "Storage:AccountKey", Storage.PrimaryAccessKey },
                });
        }
    }
}

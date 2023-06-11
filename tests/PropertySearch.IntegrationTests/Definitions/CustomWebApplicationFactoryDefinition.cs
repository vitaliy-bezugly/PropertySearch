using PropertySearchApp;

namespace PropertySearch.IntegrationTests.Definitions;

[CollectionDefinition(name: "web-application-factory")]
public class CustomWebApplicationFactoryDefinition 
    : ICollectionFixture<CustomWebApplicationFactory<Startup>>
{ }

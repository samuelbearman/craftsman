﻿namespace Craftsman.Helpers
{
    using Craftsman.Enums;
    using Craftsman.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class ClassPathHelper
    {
        public static ClassPath SolutionClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, "", className);
        }

        public static ClassPath WebApiServicesClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("WebApi", "Services"), className);
        }

        public static ClassPath IdentityProjectPath(string solutionDirectory, string className = "")
        {
            return new ClassPath(solutionDirectory, "Infrastructure.Identity", className);
        }

        public static ClassPath ControllerClassPath(string solutionDirectory, string className, string version = "v1")
        {
            return new ClassPath(solutionDirectory, Path.Combine("WebApi","Controllers", version), className);
        }

        public static ClassPath TestEntityIntegrationClassPath(string solutionDirectory, string className, string entityName, string solutionName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"{solutionName}.Tests", "IntegrationTests", entityName), className);
        }

        public static ClassPath WebApiExtensionsClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("WebApi","Extensions"), className);
        }

        public static ClassPath HttpClientExtensionsClassPath(string projectDirectory, string solutionName, string className)
        {
            return new ClassPath(projectDirectory, Path.Combine($"{solutionName}.Tests", "Helpers"), className);
        }

        public static ClassPath WebApiMiddlewareClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("WebApi", "Middleware"), className);
        }

        public static ClassPath StartupClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, "WebApi", className);
        }

        public static ClassPath StartupClassPath(string solutionDirectory, string className, string gatewayProjectName)
        {
            return new ClassPath(solutionDirectory, gatewayProjectName, className);
        }

        public static ClassPath TestProjectClassPath(string solutionDirectory, string solutionName)
        {
            return new ClassPath(solutionDirectory, $"{solutionName}.Tests", $"{solutionName}.Tests.csproj");
        }

        public static ClassPath WebApiProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, "WebApi", "WebApi.csproj");
        }

        public static ClassPath WebApiAppSettingsClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, "WebApi", className);
        }

        public static ClassPath WebApiLaunchSettingsClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("WebApi", "Properties"), className);
        }

        public static ClassPath GatewayAppSettingsClassPath(string solutionDirectory, string className, string gatewayProjectName)
        {
            return new ClassPath(solutionDirectory, Path.Combine(gatewayProjectName), className);
        }

        public static ClassPath GatewayLaunchSettingsClassPath(string solutionDirectory, string className, string gatewayProjectName)
        {
            return new ClassPath(solutionDirectory, Path.Combine(gatewayProjectName, "Properties"), className);
        }

        public static ClassPath TestRepositoryClassPath(string solutionDirectory, string className, string entityName, string solutionName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"{solutionName}.Tests","RepositoryTests",entityName), className);
        }

        public static ClassPath TestFakesClassPath(string solutionDirectory, string className, string entityName, string solutionName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"{solutionName}.Tests","Fakes",entityName), className);
        }

        public static ClassPath InfraPersistenceServiceProviderClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, "Infrastructure.Persistence", className);
        }

        public static ClassPath EntityClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("Domain","Entities"), className);
        }

        public static ClassPath SeederClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("Infrastructure.Persistence","Seeders"), className);
        }

        public static ClassPath IdentitySeederClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("Infrastructure.Identity", "Seeders"), className);
        }

        public static ClassPath DbContextClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine("Infrastructure.Persistence","Contexts"), className);
        }

        public static ClassPath ValidationClassPath(string solutionDirectory, string className, string entityName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application","Validation",entityName), className);
        }

        public static ClassPath IRepositoryClassPath(string solutionDirectory, string className, string entityName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application","Interfaces",entityName), className);
        }

        public static ClassPath ProfileClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application","Mappings"), className);
        }

        public static ClassPath ApplicationInterfaceClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application", "Interfaces"), className);
        }

        public static ClassPath ApplicationExceptionClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application", "Exceptions"), className);
        }

        public static ClassPath WrappersClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application", "Wrappers"), className);
        }

        public static ClassPath WebApiProjectRootClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"WebApi"), className);
        }

        public static ClassPath GatewayProjectRootClassPath(string solutionDirectory, string className, string gatewayProjectName)
        {
            return new ClassPath(solutionDirectory, gatewayProjectName, className);
        }

        public static ClassPath ApplicationProjectRootClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application"), className);
        }

        public static ClassPath InfrastructurePersistenceProjectRootClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Persistence"), className);
        }

        public static ClassPath TestProjectRootClassPath(string solutionDirectory, string className, string solutionName)
        {
            return new ClassPath(solutionDirectory, $"{solutionName}.Tests", className);
        }

        public static ClassPath InfrastructureSharedProjectRootClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Shared"), className);
        }

        public static ClassPath InfrastructureIdentityProjectRootClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Identity"), className);
        }

        public static ClassPath CommonDomainClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Domain", "Common"), className);
        }

        public static ClassPath DomainEnumClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Domain", "Enums"), className);
        }
        public static ClassPath DomainCommonClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Domain", "Common"), className);
        }

        public static ClassPath DomainSettingsClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Domain", "Settings"), className);
        }

        public static ClassPath RepositoryClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Persistence","Repositories"), className);
        }

        public static ClassPath DtoClassPath(string solutionDirectory, string className, string entityName)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application","Dtos",entityName), className);
        }

        public static ClassPath SharedDtoClassPath(string solutionDirectory, string className)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application", "Dtos", "Shared"), className);
        }

        public static ClassPath ApplicationProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Application"), "Application.csproj");
        }

        public static ClassPath GatewayProjectClassPath(string solutionDirectory, string gatewayProjectName)
        {
            return new ClassPath(solutionDirectory, gatewayProjectName, @$"{gatewayProjectName}.csproj");
        }

        public static ClassPath DomainProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Domain"), "Domain.csproj");
        }

        public static ClassPath InfrastructurePersistenceProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Persistence"), "Infrastructure.Persistence.csproj");
        }

        public static ClassPath InfrastructureSharedProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Shared"), "Infrastructure.Shared.csproj");
        }

        public static ClassPath InfrastructureIdentityProjectClassPath(string solutionDirectory)
        {
            return new ClassPath(solutionDirectory, Path.Combine($"Infrastructure.Identity"), "Infrastructure.Identity.csproj");
        }
    }
}

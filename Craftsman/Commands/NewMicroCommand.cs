﻿namespace Craftsman.Commands
{
    using Craftsman.Builders;
    using Craftsman.Builders.Dtos;
    using Craftsman.Builders.Seeders;
    using Craftsman.Builders.Tests.Fakes;
    using Craftsman.Builders.Tests.IntegrationTests;
    using Craftsman.Builders.Tests.RepositoryTests;
    using Craftsman.Enums;
    using Craftsman.Exceptions;
    using Craftsman.Helpers;
    using Craftsman.Models;
    using Craftsman.Removers;
    using FluentAssertions.Common;
    using LibGit2Sharp;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using YamlDotNet.Serialization;
    using static Helpers.ConsoleWriter;

    public static class NewMicroCommand
    {
        public static void Help()
        {
            WriteHelpHeader(@$"Description:");
            WriteHelpText(@$"   Scaffolds out one or more microservices in a new project based on a given template file in a json or yaml format.{Environment.NewLine}");

            WriteHelpHeader(@$"Usage:");
            WriteHelpText(@$"   craftsman new:micro [options] <filepath>");
            WriteHelpText(@$"   OR");
            WriteHelpText(@$"   craftsman new:microservice [options] <filepath>{Environment.NewLine}");

            WriteHelpHeader(@$"Arguments:");
            WriteHelpText(@$"   filepath         The full filepath for the yaml or json file that describes your web API using a proper Wrapt format.");

            WriteHelpText(Environment.NewLine);
            WriteHelpHeader(@$"Options:");
            WriteHelpText(@$"   -h, --help          Display this help message. No filepath is needed to display the help message.");

            WriteHelpText(Environment.NewLine);
            WriteHelpHeader(@$"Example:");
            WriteHelpText(@$"       craftsman new:micro C:\fullpath\micro.yaml");
            WriteHelpText(@$"       craftsman new:micro C:\fullpath\micro.yml");
            WriteHelpText(@$"       craftsman new:micro C:\fullpath\micro.json{Environment.NewLine}");

        }

        public static void Run(string filePath, string buildSolutionDirectory, IFileSystem fileSystem)
        {
            try
            {
                GlobalSingleton instance = GlobalSingleton.GetInstance();

                FileParsingHelper.RunInitialTemplateParsingGuards(filePath);
                var template = FileParsingHelper.GetApiTemplateFromFile(filePath);
                WriteHelpText($"Your template file was parsed successfully.");

                FileParsingHelper.RunPrimaryKeyGuard(template);
                FileParsingHelper.RunSolutionNameAssignedGuard(template);

                // solution level stuff
                var solutionDirectory = $"{buildSolutionDirectory}{Path.DirectorySeparatorChar}{template.SolutionName}";
                SolutionBuilder.BuildSolution(solutionDirectory, template, fileSystem);
                ReadmeBuilder.CreateReadme(solutionDirectory, template, fileSystem);
                if (template.AddGit)
                    GitSetup(solutionDirectory);

                // new microservice path
                fileSystem.Directory.CreateDirectory(Path.Combine(solutionDirectory, "src"));
                var newPath = Path.Combine(solutionDirectory, "src", "services");
                fileSystem.Directory.CreateDirectory(newPath);

                SolutionBuilder.AddProjects(solutionDirectory, template.DbContext.Provider, template.SolutionName, fileSystem, template.AuthSetup.InMemoryUsers);

                // add all files based on the given template config
                RunMicroTemplateBuilders(solutionDirectory, template, fileSystem);

                WriteFileCreatedUpdatedResponse();
                WriteHelpHeader($"{Environment.NewLine}Your API is ready! Build something amazing.");
                WriteGettingStarted(template.SolutionName);
                StarGithubRequest();
            }
            catch (Exception e)
            {
                if (e is FileAlreadyExistsException
                    || e is DirectoryAlreadyExistsException
                    || e is InvalidSolutionNameException
                    || e is FileNotFoundException
                    || e is InvalidDbProviderException
                    || e is InvalidFileTypeException)
                {
                    WriteError($"{e.Message}");
                }
                else
                    WriteError($"An unhandled exception occurred when running the API command.\nThe error details are: \n{e.Message}");
            }
        }

        private static void RunMicroTemplateBuilders(string solutionDirectory, ApiTemplate template, IFileSystem fileSystem)
        {
            // dbcontext
            DbContextBuilder.CreateDbContext(solutionDirectory, template.Entities, template.DbContext.ContextName, template.AuthSetup.AuthMethod, template.DbContext.Provider, template.DbContext.DatabaseName);

            //entities
            foreach (var entity in template.Entities)
            {
                EntityBuilder.CreateEntity(solutionDirectory, entity, fileSystem);
                DtoBuilder.CreateDtos(solutionDirectory, entity);

                RepositoryBuilder.AddRepository(solutionDirectory, entity, template.DbContext);
                ValidatorBuilder.CreateValidators(solutionDirectory, entity);
                ProfileBuilder.CreateProfile(solutionDirectory, entity);

                ControllerBuilder.CreateController(solutionDirectory, entity, template.SwaggerConfig.AddSwaggerComments);

                FakesBuilder.CreateFakes(solutionDirectory, template.SolutionName, entity);
                ReadTestBuilder.CreateEntityReadTests(solutionDirectory, template.SolutionName, entity, template.DbContext.ContextName, template.AuthSetup.AuthMethod);
                GetTestBuilder.CreateEntityGetTests(solutionDirectory, template.SolutionName, entity, template.DbContext.ContextName);
                PostTestBuilder.CreateEntityWriteTests(solutionDirectory, entity, template.SolutionName);
                UpdateTestBuilder.CreateEntityUpdateTests(solutionDirectory, entity, template.SolutionName, template.DbContext.ContextName);
                DeleteTestBuilder.DeleteEntityWriteTests(solutionDirectory, entity, template.SolutionName, template.DbContext.ContextName, template.AuthSetup.AuthMethod);
                WebAppFactoryBuilder.CreateWebAppFactory(solutionDirectory, template.SolutionName, template.DbContext.ContextName);
            }

            // environments
            AddStartupEnvironmentsWithServices(
                solutionDirectory,
                template.SolutionName,
                template.AuthSetup.AuthMethod,
                template.DbContext.DatabaseName,
                template.Environments,
                template.SwaggerConfig,
                template.AuthSetup.InMemoryUsers
            );

            //seeders
            SeederBuilder.AddSeeders(solutionDirectory, template.Entities, template.DbContext.ContextName);

            //services
            SwaggerBuilder.AddSwagger(solutionDirectory, template.SwaggerConfig, template.SolutionName);
            
            if(template.AuthSetup.AuthMethod == "JWT")
            {
                IdentityServicesModifier.SetIdentityOptions(solutionDirectory, template.AuthSetup);
                IdentitySeederBuilder.AddSeeders(solutionDirectory, template);
                IdentityRoleBuilder.CreateRoles(solutionDirectory, template.AuthSetup.Roles);
                RoleSeedBuilder.SeedRoles(solutionDirectory, template);
            }
        }

        private static void CreateNewFoundation(ApiTemplate template, string directory)
        {
            var newDir = $"{directory}{Path.DirectorySeparatorChar}{template.SolutionName}";
            if (Directory.Exists(newDir))
                throw new DirectoryAlreadyExistsException(newDir);

            //UninstallFoundation();
            //InstallFoundation();
            //UpdateFoundation();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"new foundation -n {template.SolutionName}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = directory
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void GitSetup(string solutionDirectory)
        {
            GitBuilder.CreateGitIgnore(solutionDirectory);

            Repository.Init(solutionDirectory);
            var repo = new Repository(solutionDirectory);

            string[] allFiles = Directory.GetFiles(solutionDirectory, "*.*", SearchOption.AllDirectories);
            Commands.Stage(repo, allFiles);

            var author = new Signature("Craftsman", "craftsman", DateTimeOffset.Now);
            repo.Commit("Initial Commit", author, author);
        }

        private static void UpdateFoundation()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"new Foundation.Api --update-apply",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void UninstallFoundation()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"new -u Foundation.Api",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void InstallFoundation()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"new -i Foundation.Api",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void AddStartupEnvironmentsWithServices(
            string solutionDirectory,
            string solutionName,
            string authMethod,
            string databaseName,
            List<ApiEnvironment> environments,
            SwaggerConfig swaggerConfig,
            List<ApplicationUser> inMemoryUsers)
        {
            // add a development environment by default for local work if none exists
            if (environments.Where(e => e.EnvironmentName == "Development").Count() == 0)
                environments.Add(new ApiEnvironment { EnvironmentName = "Development", ProfileName = $"{solutionName} (Development)" });

            foreach (var env in environments)
            {
                // default startup is already built in cleanup phase
                if(env.EnvironmentName != "Startup")
                    StartupBuilder.CreateStartup(solutionDirectory, env.EnvironmentName, authMethod, inMemoryUsers);

                AppSettingsBuilder.CreateAppSettings(solutionDirectory, env, databaseName, authMethod);
                LaunchSettingsModifier.AddProfile(solutionDirectory, env);

                //services
                if (!swaggerConfig.IsSameOrEqualTo(new SwaggerConfig()))
                    SwaggerBuilder.RegisterSwaggerInStartup(solutionDirectory, env);
            }
        }
    }
}
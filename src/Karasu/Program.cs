using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

using Autofac;
using Autofac.Integration.WebApi;

using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;

using Swashbuckle.Application;

using CommandLine;

using Karasu.Instrumentation;
using Karasu.Media;
using Karasu.Networking;
using Karasu.Properties;
using Karasu.Repositories;

namespace Karasu
{
    class Program
    {
        public static string Host = $"http://{NetworkManager.GetLanIp()}:8888/";

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "--help" };
            }

            var result = Parser.Default.ParseArguments<Options>(args);

            return result.Return(
                options =>
                {
                    if (!File.Exists(options.MplayerExecutablePath))
                    {
                        Console.Error.WriteLine("MPlayer path not specified or invalid: {0}", options.MplayerExecutablePath);
                        return 1;
                    }

                    if (!Directory.Exists(options.WebAppPath))
                    {
                        Console.Error.WriteLine("Web app path not specified or invalid: {0}", options.WebAppPath);
                        return 1;
                    }

                    if (options.LibraryPath.Count == 0)
                    {
                        Console.Error.WriteLine("At least one library path must be specified");
                        return 1;
                    }

                    foreach (var path in options.LibraryPath.Where(p => !Directory.Exists(p)))
                    {
                        Console.Error.WriteLine("Library path invalid: {0}", path);
                        return 1;
                    }

                    try
                    {
                        RunOwin(options);
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        return 1;
                    }
                },
                errors =>
                {
                    foreach (var error in errors.Where(e => !(e is HelpRequestedError)))
                    {
                        Console.Error.WriteLine(error);
                    }

                    return 1;
                });
        }

        private static void RunOwin(Options options)
        {
            var owinOptions = new StartOptions("http://*:8888")
            {
                ServerFactory = "Microsoft.Owin.Host.HttpListener"
            };

            using (WebApp.Start(owinOptions, appBuilder =>
            {
                // WA
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.EnableSwagger(c => c.SingleApiVersion("v1", "K")).EnableSwaggerUi();
                config.EnableCors(new EnableCorsAttribute("*", null, "*", "Content-Type,Link"));

                // Autofac
                var container = BuildContainer(options);
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

                appBuilder.UseCors(CorsOptions.AllowAll);

                appBuilder.UseAutofacMiddleware(container);
                appBuilder.UseAutofacWebApi(config);
                appBuilder.UseWebApi(config);
                appBuilder.UseFileServer(new FileServerOptions
                {
                    FileSystem = new PhysicalFileSystem(options.WebAppPath),
                    RequestPath = new PathString("/app"),
                    DefaultFilesOptions = { DefaultFileNames = new[] { "index.html" } }
                });

                appBuilder.Use(async (cx, next) =>
                {
                    var appPath = Host + "app";

                    cx.Response.ContentType = "text/html";

                    await cx.Response.WriteAsync(string.Format(
                        Resources.BlankPageTemplate,
                        BarcodeGenerator.GeneratePngDataUri(appPath, 800, 800),
                        appPath));
                });

                var settings = container.Resolve<ISettingsRepository>();
                var songs = container.Resolve<ISongRepository>();

                songs.Reindex(settings.ListPaths().ToArray());

                var runner = container.Resolve<SongRunner>();
                runner.Start();

            }))
            {
                PrintFlair();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(Resources.ListeningOnText);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Host);

                try
                {
                    Process.Start(Host);
                }
                catch (Exception)
                {
                    // ignored
                }

                Console.ReadKey();
            }
        }

        private static IContainer BuildContainer(Options options)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();

            var settings = new SettingsRepository(options.MplayerExecutablePath, options.LibraryPath);

            builder.RegisterInstance<ISettingsRepository>(settings);
            builder.RegisterType<SongRepository>().As<ISongRepository>().SingleInstance();

            builder.RegisterType<SongRunner>().SingleInstance();

            builder.RegisterWebApiModelBinderProvider();

            builder.RegisterApiControllers(assembly);

            return builder.Build();
        }

        private static void PrintFlair()
        {
            var dash = new string('-', 50);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(dash);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Resources.KaraokeServerTitle.PadLeft(35));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(dash);
            Console.WriteLine();

            Console.ResetColor();
        }
    }
}

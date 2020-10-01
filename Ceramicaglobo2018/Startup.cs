using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using CKSource.CKFinder.Connector.Core.Acl;
using CKSource.FileSystem.Local;
using CKSource.CKFinder.Connector.Host.Owin;
using CKSource.CKFinder.Connector.Config;
using CKSource.CKFinder.Connector.Core.Builders;
using WebSite.Infrastructure.Security;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(WebSite.Startup))]

namespace WebSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //*Register the "local" type backend file system.
            
            FileSystemFactory.RegisterFileSystem<LocalStorage>();
            /*
             * Map the CKFinder connector service under a given path. By default the CKFinder JavaScript
             * client expect the ASP.NET connector to be accessible under the "/ckfinder/connector" route.
             */

            app.Map("/ckfinder/connector", SetupConnector);

        }

        private static void SetupConnector(IAppBuilder app)
        {
            /*
               * Create a connector instance using ConnectorBuilder. The call to the LoadConfig() method
               * will configure the connector using CKFinder configuration options defined in Web.config.
               */
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();
            /*
             * Create an instance of authenticator implemented in the previous step.
             */
            var customAuthenticator = new CustomCKFinderAuthenticator();
            connectorBuilder
                /*
                 * Provide the global configuration.
                 *
                 * If you installed CKSource.CKFinder.Connector.Config you may load static configuration
                 * from XML:
                 * connectorBuilder.LoadConfig();
                 */
                .LoadConfig()
                .SetAuthenticator(customAuthenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        /* Add a local backend. */
                        //config.AddProxyBackend("local", new LocalStorage(@"public/upload"));
                        config.AddBackend("local", new LocalStorage(@"public/upload", "http://www.stampecreative.it/public/upload/"));
                        /* Add a resource type that uses the local backend. */
                        config.AddResourceType("Files", resourceBuilder => resourceBuilder.SetBackend("local", "files"));
                        config.AddResourceType("Images", resourceBuilder => resourceBuilder.SetBackend("local", "images"));
                        /* Give full access to all resource types at any path for all users. */
                        config.AddAclRule(new AclRule(
                            new StringMatcher("*"), new StringMatcher("/"), new StringMatcher("*"),
                            new Dictionary<Permission, PermissionType>
                            {
                                { Permission.FolderView, PermissionType.Allow },
                                { Permission.FolderCreate, PermissionType.Allow },
                                { Permission.FolderRename, PermissionType.Allow },
                                { Permission.FolderDelete, PermissionType.Allow },
                                { Permission.FileView, PermissionType.Allow },
                                { Permission.FileCreate, PermissionType.Allow },
                                { Permission.FileRename, PermissionType.Allow },
                                { Permission.FileDelete, PermissionType.Allow },
                                { Permission.ImageResize, PermissionType.Allow },
                                { Permission.ImageResizeCustom, PermissionType.Allow }
                            }));
                    });

            /*
             * Configure settings per request.
             *
             * The minimal configuration has to include at least one backend, one resource type
             * and one ACL rule.
             *
             * For example:
             * config.AddBackend("default", new LocalStorage(@"C:\files"));
             * config.AddResourceType("images", builder => builder.SetBackend("default", "images"));
             * config.AddAclRule(new AclRule(
             *     new StringMatcher("*"),
             *     new StringMatcher("*"),
             *     new StringMatcher("*"),
             *     new Dictionary<Permission, PermissionType> { { Permission.All, PermissionType.Allow } }));
             *
             * If you installed CKSource.CKFinder.Connector.Config, you may load the static configuration
             * from XML:
             * config.LoadConfig();
             *
             * If you installed CKSource.CKFinder.Connector.KeyValue.EntityFramework, you may enable caching:
             * config.SetKeyValueStoreProvider(
             *     new EntityFrameworkKeyValueStoreProvider("CKFinderCacheConnection"));
             */


            /*
             * Build the connector middleware.
             */
            var connector = connectorBuilder
                .Build(connectorFactory);
            /*
             * Add the CKFinder connector middleware to the web application pipeline.
             */
            app.UseConnector(connector);
        }
    }
}

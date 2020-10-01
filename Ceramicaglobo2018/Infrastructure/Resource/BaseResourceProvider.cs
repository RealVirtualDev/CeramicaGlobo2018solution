using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Models;
using System.Web.SessionState;
using WebSite.Helpers;

namespace WebSite.Infrastructure.Resource
{
    public abstract class BaseResourceProvider : IResourceProvider
    {
        // Cache list of resources
        private static Dictionary<string, LanguageResource> resources = null;
        private static object lockResources = new object();

        public BaseResourceProvider()
        {
            Cache = true; // By default, enable caching for performance
        }

        protected bool Cache { get; set; } // Cache resources ?

        /// <summary>
        /// Returns a single resource for a specific culture
        /// </summary>
        /// <param name="name">Resorce name (ie key)</param>
        /// <param name="culture">Culture code</param>
        /// <returns>Resource</returns>
        public object GetResource(string name, string culture)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(culture))
                throw new ArgumentException("Culture name cannot be null or empty.");

            // normalize
            culture = culture.ToLowerInvariant();

            if (Cache && resources == null)
            {
                // Fetch all resources

                lock (lockResources)
                {

                    if (resources == null)
                    {
                        resources = ReadResources().ToDictionary(r => string.Format("{0}.{1}", r.lang.ToLowerInvariant(), r.datakey));
                    }
                }
            }

            if (Cache)
            {
                return resources[string.Format("{0}.{1}", culture, name)].datavalue;
            }

            return ReadResource(name, culture).datavalue;

        }


        /// <summary>
        /// Returns all resources for all cultures. (Needed for caching)
        /// </summary>
        /// <returns>A list of resources</returns>
        protected abstract IList<LanguageResource> ReadResources();


        /// <summary>
        /// Returns a single resource for a specific culture
        /// </summary>
        /// <param name="name">Resorce name (ie key)</param>
        /// <param name="culture">Culture code</param>
        /// <returns>Resource</returns>
        protected abstract LanguageResource ReadResource(string name, string culture);

    }
}
﻿using System;
using System.Configuration;
using System.Linq;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.ServiceLocation;
using EPiServer.Templates.Alloy.Business.ContentProviders;
using EPiServer.Templates.Alloy.Models.Pages;
using EPiServer.Framework.Initialization;
using log4net;

namespace EPiServer.Templates.Alloy.Business.Initialization
{
    /// <summary>
    /// Registers a content provider used to clone global news from a global container to site(s) which specify a local page where global news should be accessible
    /// </summary>
    [ModuleDependency(typeof(Web.InitializationModule))]
    public class GlobalNewsContentProviderInitialization : IInitializableModule
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (GlobalNewsContentProviderInitialization));

        private bool _initialized;

        public void Initialize(InitializationEngine context)
        {
            if (_initialized || PageReference.IsNullOrEmpty(GlobalNewsContainer) || context.HostType != HostType.WebApplication )
            {
                return;
            }

            var providerManager = ServiceLocator.Current.GetInstance<IContentProviderManager>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var startPages = contentLoader.GetChildren<PageData>(ContentReference.RootPage).OfType<StartPage>();

            // Attach content provider to each site's global news container
            foreach (var startPage in startPages.Where(startPage => !PageReference.IsNullOrEmpty(startPage.GlobalNewsPageLink)))
            {
                try
                {
                    Logger.DebugFormat("Attaching global news content provider to page {0} [{1}], global news will be retrieved from page {2} [{3}]",
                        contentLoader.Get<PageData>(startPage.GlobalNewsPageLink).Name, 
                        startPage.GlobalNewsPageLink.ID, 
                        contentLoader.Get<PageData>(GlobalNewsContainer).PageName, 
                        GlobalNewsContainer.ID);

                    var provider = new ClonedContentProvider(GlobalNewsContainer, startPage.GlobalNewsPageLink, startPage.Category);

                    providerManager.ProviderMap.AddProvider(provider);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Unable to create global news content provider for start page with ID {0}: {1}", startPage.PageLink.ID, ex.Message);
                }
            }

            _initialized = true;
        }

        /// <summary>
        /// The global news container, ie the content we want to clone
        /// </summary>
        /// <remarks>Returns PageReference.Empty if no 'GlobalNewsContainerID' app settings exist</remarks>
        private PageReference GlobalNewsContainer
        {
            get
            {
                const string appSettingName = "GlobalNewsContainerID";

                var pageLinkIdString = ConfigurationManager.AppSettings[appSettingName];

                if (string.IsNullOrWhiteSpace(pageLinkIdString))
                {
                    return PageReference.EmptyReference;
                }

                int pageLinkId;

                if (!int.TryParse(pageLinkIdString, out pageLinkId) || pageLinkId == 0)
                {
                    Logger.ErrorFormat("The '{0}' app setting was not set to a valid page ID, expected a positive integer", appSettingName);

                    return PageReference.EmptyReference;
                }

                return new PageReference(pageLinkId);
            }
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context) { }
    }
}
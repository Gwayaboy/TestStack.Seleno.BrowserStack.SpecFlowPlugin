﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.BrowserStack.Core.Configuration
{
    public class BrowserHostFactory : IBrowserHostFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public TimeSpan CommandTimeOut { get; set; }

        public BrowserHostFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            CommandTimeOut = TimeSpan.FromMinutes(5);
        }


        public IBrowserHost CreateWithCapabilities(ICapabilities capabilities, BrowserConfiguration browserConfiguration = null)
        {
            var instance = CreateBrowserHost(browserConfiguration);

            instance.Run(CreateRemoteDriverWithCapabilities(capabilities), 
                         CreateWebServer(_configurationProvider.RemoteUrl));

            return instance;
        }

        public virtual IBrowserHost CreateBrowserHost(BrowserConfiguration browserConfiguration)
        {
            return new BrowserHost(new SelenoHost(), browserConfiguration);
        }

        public virtual Func<RemoteWebDriver> CreateRemoteDriverWithCapabilities(ICapabilities capabilities)
        {
            return () => new RemoteWebDriver(new Uri(_configurationProvider.RemoteUrl), capabilities, CommandTimeOut);
        }


        public virtual IWebServer CreateWebServer(string remoteUrl)
        {
            return new InternetWebServer("https://www.cbreresidential.com/");
        }
    }
}
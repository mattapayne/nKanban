using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using TechTalk.SpecFlow;
using System.Configuration;

namespace nKanban.AcceptanceTests
{
    [Binding]
    public static class WebBrowser
    {
        private const string CONTEXT_KEY = "__browser__";
        private const string WEB_APP_ROOT_URL_KEY = "WebAppRootUrl";
        private static IE _browser;

        public static IE Current
        {
            get
            {
                return _browser;
            }
        }

        public static void TypeFast(this TextField textField, string text)
        {
            textField.SetAttributeValue("value", text);
        }

        public static void GoToRelative(this IE browser, string relativeUrl)
        {
            var absolute = ToAbsoluteUrl(relativeUrl);
            browser.GoTo(absolute);
        }

        [BeforeScenario]
        public static void BeforeAllScenarios()
        {
            if (Current != null)
            {
                Current.ClearCookies();
                Current.ClearCache();
            }
        }

        [BeforeTestRun]
        public static void InitializeBrowser()
        {
            if (Current == null)
            {
                Settings.Instance.MakeNewIeInstanceVisible = false;
                _browser = new IE();
            }
        }

        [AfterTestRun]
        public static void KillBrowser()
        {
            if (Current != null)
            {
                Current.Close();
                Current.Dispose();
            }
        }

        private static string ToAbsoluteUrl(string relativeUrl)
        {
            string rootUrl = ConfigurationManager.AppSettings[WEB_APP_ROOT_URL_KEY];

            if (String.IsNullOrEmpty(rootUrl))
            {
                throw new Exception("Please set the web app root url in app.config.");
            }

            return new Uri(new Uri(rootUrl), relativeUrl).ToString();
        }
    }
}

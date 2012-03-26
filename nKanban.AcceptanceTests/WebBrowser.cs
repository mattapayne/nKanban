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
        private static string _rootUrl;

        static WebBrowser()
        {
            _rootUrl = ConfigurationManager.AppSettings[WEB_APP_ROOT_URL_KEY];

            if (String.IsNullOrEmpty(_rootUrl))
            {
                throw new Exception("Please set the web app root url in app.config.");
            }
        }

        public static IE Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey(CONTEXT_KEY))
                {
                    ScenarioContext.Current.Add(CONTEXT_KEY, new IE());
                }

                return ScenarioContext.Current[CONTEXT_KEY] as IE;
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

        [AfterScenario]
        public static void KillBrowser()
        {
            if (ScenarioContext.Current.ContainsKey(CONTEXT_KEY))
            {
                Current.Close();
            }
        }

        private static string ToAbsoluteUrl(string relativeUrl)
        {
            return new Uri(new Uri(_rootUrl), relativeUrl).ToString();
        }
    }
}

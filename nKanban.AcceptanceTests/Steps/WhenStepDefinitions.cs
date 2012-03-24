using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace nKanban.AcceptanceTests.Steps
{
    [Binding]
    public class WhenStepDefinitions
    {
        [When(@"I navigate to (.*)")]
        public void WhenINavigateTo(string url)
        {
            WebBrowser.Current.GoToRelative(url);
        }

        [When(@"I click the (.*) link")]
        public void WhenIClickTheSpecifiedLink(string linkText)
        {
            var link = WebBrowser.Current.Links.Where(l => l.Text == linkText).FirstOrDefault();
            link.Click();
        }
    }
}

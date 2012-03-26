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
        [When(@"I login")]
        public void WhenILogin()
        {
            WebBrowser.Current.GoToRelative("Login");
            WebBrowser.Current.TextField("UserName").TypeFast("paynmatt@gmail.com");
            WebBrowser.Current.TextField("Password").TypeFast("232423");
            WebBrowser.Current.Button("Login").Click();
        }

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

        [When(@"I click the (.*) button")]
        public void WhenIClickTheSpecifiedButton(string buttonText)
        {
            var button = WebBrowser.Current.Buttons.Where(b => b.Text == buttonText).FirstOrDefault();
            button.Click();
        }

        [When(@"I fill in the form with")]
        public void WhenIFillInTheFormWith(Table data)
        {
            foreach (var row in data.Rows)
            {
                foreach (var key in row.Keys)
                {
                    string value;

                    if (row.TryGetValue(key, out value))
                    {
                        var field = WebBrowser.Current.TextFields.Where(t => t.Name == key).FirstOrDefault();
                        
                        if (field != null)
                        {
                            field.TypeFast(value);
                        }
                    }
                }
            }
        }
    }
}

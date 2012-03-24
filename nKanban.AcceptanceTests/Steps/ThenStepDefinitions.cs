using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace nKanban.AcceptanceTests.Steps
{
    [Binding]
    public class ThenStepDefinitions
    {
        [Then(@"I should be on the (.*) page")]
        public void ThenIShouldBeOnTheSpecifiedPage(string pageName)
        {
            WebBrowser.Current.Title.Should().ContainEquivalentOf(pageName);
        }

        [Then(@"I should see \ban?\b (.*) link")]
        public void ThenIShouldSeeTheLinkSpecified(string linkName)
        {
            var link = WebBrowser.Current.Links.Where(l => l.Text == linkName).FirstOrDefault();
            link.Should().NotBeNull();
            link.Text.Should().ContainEquivalentOf(linkName);
        }

        [Then(@"I should see errors in an element with the id: (.*) and the class: (.*) on the page")]
        public void ThenThereShouldBeAnElementOnThePage(string elementId, string className)
        {
            var element = WebBrowser.Current.Elements.Where(e => e.Id == elementId).FirstOrDefault();
            element.Should().NotBeNull();
            element.ClassName.Should().ContainEquivalentOf(className);
        }
    }
}

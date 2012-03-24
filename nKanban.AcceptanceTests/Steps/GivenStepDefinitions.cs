using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace nKanban.AcceptanceTests.Steps
{
    [Binding]
    public class GivenStepDefinitions
    {
        [Given(@"I am not logged in")]
        public void GivenIAmNotLoggedIn()
        {
            //no-op for now.
        }
    }
}

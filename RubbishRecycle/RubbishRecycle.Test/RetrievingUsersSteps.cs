using System;
using TechTalk.SpecFlow;

namespace RubbishRecycle.Test
{
    [Binding]
    public class RetrievingUsersSteps
    {
        [Given(@"an exsiting user")]
        public void GivenAnExsitingUser()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"it is retrieved")]
        public void WhenItIsRetrieved()
        {
            
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"a '(.*)' status is returned")]
        public void ThenAStatusIsReturned(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}

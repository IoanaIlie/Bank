using System;
using BoDi;
using SpecFlowProjectBank.Models;
using TechTalk.SpecFlow;

namespace SpecFlowProjectBank.Hooks
{
    [Binding]
    public class Hooks
    {
        [AfterTestRun]
        public static void SetupStuffForFeatures()
        {
            //GET all created ACCOUNTS and delete them
            
            //GET all created USERS and delete them
        }
    }
}
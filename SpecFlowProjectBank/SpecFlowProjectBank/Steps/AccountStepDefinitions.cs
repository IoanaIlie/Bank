using System.Collections;
using System.Net;
using BoDi;
using Newtonsoft.Json;
using NUnit.Framework;
using SpecFlowProjectBank.Models;
using SpecFlowProjectBank.Utils;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowProjectBank.Steps;

[Binding]
public sealed class AccountStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private readonly List<User> users = new List<User>();
    private readonly List<Account> accounts = new List<Account>();


    public AccountStepDefinitions(ScenarioContext scenarioContext,ISpecFlowOutputHelper specFlowOutputHelper, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _specFlowOutputHelper = specFlowOutputHelper;
        _featureContext = featureContext;
    }
/*
 * GIVEN
 */
    [Given(@"the new user ""(.*)""")]
    public void GivenTheNewUser(string testUser)
    {
        //call the POST endpoint for creation of the user. The response from POST will contain a user ID. So we deserialize the response in a User
        User user = new User(testUser);
        users.Add(user);
        //we transfer for the rest of the scenario the userId we need to create accounts for this user
        _specFlowOutputHelper.WriteLine("{0} - {1}","POST ", user.Print());
        _scenarioContext["userId"] = user.Id;
    } 
    
    /*
 * WHEN
 */
    
    [When(@"the user create account for:")]
    public void WhenTheUserCreateAccountFor(Table table)
    {
        if (_scenarioContext.ContainsKey("userId"))
        {
            var dictionary = ToDictionary(table);
            var typeStr = dictionary.Keys.Contains("type") ? dictionary["type"] : null;
            var valueStr = dictionary.Keys.Contains("value") ? dictionary["value"] : null;
            if (valueStr != null)
            {
                double value = Double.Parse(valueStr);
                string userId = _scenarioContext["userId"].ToString();
                //call the POST endpoint for creation of the account. The response from POST will contain an account ID. So we deserialize the response in an Account
                Account account = new Account(Int32.Parse(userId), typeStr, value);
                accounts.Add(account);
                _specFlowOutputHelper.WriteLine("{0} - {1}","POST ", account.Print());
                _scenarioContext["createdAccount"] = account;
            }
        }
        if (_scenarioContext.ContainsKey("loop"))
        {
            int loop = (int)_scenarioContext["loop"];
            _scenarioContext["loop"] = ++loop;
        }
        else
         _scenarioContext["loop"] = 1;

        var loop1 = _scenarioContext["loop"];
        //the Response code and message from the POST Account call should look like
        IEnumerator en = _scenarioContext.ScenarioInfo.Arguments.Values.GetEnumerator();
        en.MoveNext();
        var responseId = _scenarioContext.ScenarioInfo.Title[0] + "." + en.Current;
        int responseLoop = (int)_scenarioContext["loop"];
        Response response = JsonUtils.GetResponse(responseId, responseLoop);
        _scenarioContext["responseCode"] = response.ResponseCode;
        _scenarioContext["responseText"] = response.ResponseText;
        
    }
    
    [When(@"the user create  ""(.*)"" accounts for:")]
    public void WhenTheUserCreateAccountsFor(string number, Table table)
    {
        if (_scenarioContext.ContainsKey("userId"))
        {
            var dictionary = ToDictionary(table);
            var typeStr = dictionary.Keys.Contains("type") ? dictionary["type"] : null;
            var valueStr = dictionary.Keys.Contains("value") ? dictionary["value"] : null;
            double value = Double.Parse(valueStr);
            
            int userId = (int)_scenarioContext["userId"];
            
            if (_scenarioContext.ContainsKey("loop"))
            {
                int loop = (int)_scenarioContext["loop"];
                _scenarioContext["loop"] = ++loop;
            }
            else
                _scenarioContext["loop"] = 1;  

             int numberOfAccounts = Int32.Parse(number);
            do
            {
                Account account = new Account(userId, typeStr, value);
                //call POST ACCOUNT for each account till the number of the accounts is done
                accounts.Add(account);
                //here we need to verify the response after each POST call

                _specFlowOutputHelper.WriteLine("{0} - {1}","POST ", account.Print());
                numberOfAccounts--;
            } while (numberOfAccounts > 0);
        }
        
        //the Response code and message from the POST Account call should look like
        IEnumerator en = _scenarioContext.ScenarioInfo.Arguments.Values.GetEnumerator();
        en.MoveNext();
        var responseId = _scenarioContext.ScenarioInfo.Title[0] + "." + en.Current;
        int responseLoop = (int)_scenarioContext["loop"];
        Response response = JsonUtils.GetResponse(responseId, responseLoop);
        _scenarioContext["responseCode"] = response.ResponseCode;
        _scenarioContext["responseText"] = response.ResponseText;
    }
    
    [When(@"the created accounts are deleted")]
    public void WhenTheCreatedAccountsAreDeleted()
    {
        int userId = (int)_scenarioContext["userId"];
        //Call DELETE USER ACCOUNT for the above User Id
        
        if (_scenarioContext.ContainsKey("loop"))
        {
            int loop = (int)_scenarioContext["loop"];
            _scenarioContext["loop"] = ++loop;
        }
        else
            _scenarioContext["loop"] = 1;

        IEnumerator en = _scenarioContext.ScenarioInfo.Arguments.Values.GetEnumerator();
        en.MoveNext();
        var responseId = _scenarioContext.ScenarioInfo.Title[0] + "." + en.Current;
        int responseLoop = (int)_scenarioContext["loop"];
        
        //the Response code and message from the DELETE Account call should look like
        Response response = JsonUtils.GetResponse(responseId, responseLoop);
        _scenarioContext["responseCode"] = response.ResponseCode;
        _scenarioContext["responseText"] = response.ResponseText;
    }
    
/*
 * THEN
 */
    [Then(@"the result should have ""(.*)"" and ""(.*)""")]
    public void ThenTheResultShouldHaveAnd(string code, string message)
    {
        string responseText = (string) _scenarioContext["responseText"];
        string responseCode = (string) _scenarioContext["responseCode"];
        Assert.NotNull(responseCode);
        Assert.AreEqual(code, responseCode.ToString());
        Assert.IsTrue(message.Contains(responseText, StringComparison.InvariantCultureIgnoreCase));
        _featureContext["users"] = users;
        _featureContext["accounts"] = accounts;
    }

    [Then(@"the result should be ""(.*)""")]
    public void ThenTheResultShouldBe(string code)
    {
        string responseCode = (string) _scenarioContext["responseCode"];
        Assert.NotNull(responseCode);
        Assert.AreEqual(code, responseCode.ToString());
        _featureContext["users"] = users;
        _featureContext["accounts"] = accounts;
    }
    /*
    * PRIVATE methods
    */
    private static Dictionary<string, string> ToDictionary(Table table)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var row in table.Rows)
        {
            dictionary.Add(row[0], row[1]);
        }

        return dictionary;
    }
}
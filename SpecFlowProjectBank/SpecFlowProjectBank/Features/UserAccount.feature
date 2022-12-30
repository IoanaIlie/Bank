Feature: UserAccount

Background: 
	Given the new user "TestUser"
		
@positive
Scenario Outline:1. Create deposit account in range
	When the user create account for:
	| key   | value   |
	| value | <value> |
	| type  | deposit |
	Then the result should be "200"
	Examples:
	| id | value | 
	| 1  |  100   |
	| 2  |  101   | 
	| 3  |  10000 | 
	| 4  |  9999  |
 
@positive
Scenario Outline:2. Create withdraw account in below limit
	When the user create account for:
	| key   | value   |
	| value | 100 |
	| type  | deposit |
	Then the result should be "200"
	When the user create account for:
    | key   | value    |
    | value | <value>  |
    | type  | withdraw |
	Then the result should be "200"
	Examples:
	| id | value |
	| 1  | 10    |
	| 2  | 9     |
    
@positive
Scenario Outline:3. Account creation
	When the user create  "<number>" accounts for:
	| key   | value   |
    | value | <value> |
    | type  | deposit |
	Then the result should be "200"
	When the user create  "<number>" accounts for:
	| key   | value    |
	| value | <value>  |
	| type  | withdraw |
	Then the result should be "200"
	Examples:
	| id | number | value |
	| 1  | 1      | 100   |
	| 2  | 100    | 100   |
	| 3  | 101    | 100   |
	| 4  | 1000   | 100   |

@negative
Scenario Outline:4. Create deposit account not in range
	When the user create account for:
	| key   | value   |
	| value | <value> |
	| type  | deposit |
	Then the result should have "<responseCode>" and "<responseMessage>"
	Examples:
	| id | value  | responseCode | responseMessage                                                   |
	| 1  | 99     | 400          | The user cannot have less than 100 at anytime in his account      |
	| 2  | 99.99  | 400          | The user cannot have less than 100 at anytime in his account     |
	| 3  | 100001 | 400          | The user cannot deposit more than 10 0000 in a single transaction |

	@negative
Scenario Outline:5. Create withdraw account above limit
	When the user create account for:
	| key   | value   |
	| value | 100     |
    | type  | deposit |
	Then the result should be "200"
	When the user create account for:
	| key   | value    |
	| value | <value>      |
	| type  | withdraw |
	Then the result should have "<responseCode>" and "<responseMessage>"
	Examples:
	| id | value | responseCode | responseMessage                                                                 |
	| 1  | 91    | 400          | A user cannot withdraw more than 90% from total balance in a single transaction |
	| 2  | 99.99 | 400          | A user cannot withdraw more than 90% from total balance in a single transaction |
	| 3  | 100   | 400          | A user cannot withdraw more than 90% from total balance in a single transaction |
		
@positive
Scenario Outline:6. Delete accounts
	When the user create account for:
	| key   | value   |
	| value | 101     |
	| type  | <type1> |
	Then the result should be "200"
	When the user create account for:
	| key   | value    |
	| value | 1        |
	| type  | <type2> |
	Then the result should be "200"
	When the created accounts are deleted
	Then the result should be "204"
	Examples:
	| id | type1   | type2    |
	| 1  | deposit | deposit  |
	| 2  | deposit | withdraw |
	
@positive
Scenario Outline:7. Validation for account balance 
	When the user create  "<depositNumber>" accounts for:
	| key   | value          |
	| value | <depositValue> |
	| type  | deposit        |
	Then the result should be "200"
	When the user create  "<withdrawNumber>" accounts for:
	| key   | value           |
	| value | <withdrawValue> |
	| type  | withdraw        |
	Then the result should have "<responseCode>" and "<responseMessage>"
	Examples:
	| id | depositNumber | depositValue | withdrawNumber | withdrawValue | responseCode | responseMessage                                              |
	| 1  | 1             | 100          | 1              | 1             | 400          | The user cannot have less than 100 at anytime in his account |
	| 2  | 1             | 101          | 1              | 1             | 200          |                                                              |
	| 3  | 2             | 100          | 1              | 200.01        | 400          | The user cannot have less than 100 at anytime in his account |
	| 4  | 1             | 200          | 2              | 50.01         | 400          | The user cannot have less than 100 at anytime in his account |
	| 5  | 2             | 100          | 1              | 100           | 200          |                                                              |
	| 6  | 2             | 5000        | 1              | 100           | 200          |                                                              |


   
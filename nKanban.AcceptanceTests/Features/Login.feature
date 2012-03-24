Feature: Login
	In order to use the application
	As a user with an account,
	I want to be able to login

Scenario: Login page is accessible
	Given I am not logged in
	When I navigate to Login 
	Then I should be on the Login page

Scenario: Login with missing data
    Given I am not logged in
    When I navigate to Login
    And I fill in the form with
        | UserName              | Password  |
        | paynmatt@gmail.com    |           |
    And I click the Login button
    Then I should see errors in an element with the id: validation-errors and the class: validation-summary-errors on the page
    And I should be on the Login page

Scenario: Login with correct data
    Given I am not logged in
    When I navigate to Login
    And I fill in the form with
        | UserName              | Password  |
        | paynmatt@gmail.com    | 232423    |
    And I click the Login button
    Then I should be on the Dashboard page
Feature: Accesing the home page
	In order to use the application
	As a user
	I want to be able to access the application home page

Scenario: Browse to the home page
	When I navigate to /
	Then I should be on the Home page

Scenario: Login from the home page
    Given I am not logged in
    When I navigate to /
    Then I should see a Login link

Scenario: Register from the home page
    Given I am not logged in
    When I navigate to /
    Then I should see a Register link

Scenario: Forgot password from the home page
    Given I am not logged in
    When I navigate to /
    Then I should see a Forgot Password link

Scenario: About link from the home page
    When I navigate to /
    Then I should see an About link

Scenario: Home link from the home page
    When I navigate to /
    Then I should see an nKanban link

Scenario: Clicking the Register link goes to the Register page
    Given I am not logged in
    When I navigate to /
    And I click the Register link
    Then I should be on the Register page 

Scenario: Clicking the About link goes to the About page
    When I navigate to /
    And I click the About link
    Then I should be on the About page

Scenario: Clicking the Login link goes to the Login page
    Given I am not logged in
    When I navigate to /
    And I click the Login link
    Then I should be on the Login page

Scenario: Clicking the Forgot Password link goes to the Forgot Password page
    Given I am not logged in
    When I navigate to /
    And I click the Forgot Password link
    Then I should be on the Forgot Password page 
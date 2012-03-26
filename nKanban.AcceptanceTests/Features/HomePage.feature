Feature: Accesing the home page
	In order to use the application
	As a user
	I want to be able to access the application home page

Scenario: Browse to the home page
	When I navigate to /
	Then I should be on the Home page

Scenario: Login is available from the home page when anonymous
    Given I am not logged in
    When I navigate to /
    Then I should see a Login link

Scenario: Register is available from the home page when anonymous
    Given I am not logged in
    When I navigate to /
    Then I should see a Register link

Scenario: Forgot password is available from the home page when anonymous
    Given I am not logged in
    When I navigate to /
    Then I should see a Forgot Password link

Scenario: Logout is available from the home page when logged in
    Given I am not logged in
    When I login
    And I navigate to /
    Then I should see a Logout link

Scenario: Reset Password is available from the home page when logged in
    Given I am not logged in
    When I login
    And I navigate to /
    Then I should see a Reset Password link

Scenario: Dashboard is available from the home page when logged in
    Given I am not logged in
    When I login
    And I navigate to /
    Then I should see a Dashboard link

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
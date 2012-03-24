Feature: Forgot Password
	In order to use the application
	As a user who has forgotten my password
	I want to be able to reset my password

Scenario: Forgot Password page is accessible
	Given I am not logged in
	When I navigate to ForgotPassword 
	Then I should be on the Forgot Password page
Feature: Login
	In order to use the application
	As a user with an account,
	I want to be able to login

Scenario: Login page is accessible
	Given I am not logged in
	When I navigate to Login 
	Then I should be on the Login page
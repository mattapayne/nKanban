Feature: Registration
	In order to use the application
	As a user
	I want to be able to register

Scenario: Register page is accessible
	Given I am not logged in
	When I navigate to Register 
	Then I should be on the Register page
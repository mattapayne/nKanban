Feature: Registration
	In order to use the application
	As a user
	I want to be able to register

Scenario: Register page is accessible
	Given I am not logged in
	When I navigate to Register 
	Then I should be on the Register page

Scenario: Register with missing data
    Given I am not logged in
    When I navigate to Register
    And I fill in the form with
        | FirstName | LastName  | Email                 | Password  | PasswordConfirmation  | OrganizationName | Address1       | Address2  | City      | ProvinceId    | CountryId | PostalCode    |
        | Matt      |           | paynmatt@gmail.com    | 232423    | 232423                | MattCo           | 123 Test St    | Suite 2   | Waterloo  |               |           | N2L 2A2       |
    And I click the Register button
    Then I should see errors in an element with the id: validation-errors and the class: validation-summary-errors on the page
    And I should be on the Register page

Scenario: Register with correct data
    Given I am not logged in
    When I navigate to Register
    And I fill in the form with
        | FirstName | LastName  | Email                 | Password  | PasswordConfirmation  | OrganizationName | Address1       | Address2  | City      | ProvinceId    | CountryId | PostalCode    |
        | Matt      | Payne     | paynmatt@gmail.com    | 232423    | 232423                | MattCo           | 123 Test St    | Suite 2   | Waterloo  |               |           | N2L 2A2       |
    And I click the Register button
    Then I should be on the Dashboard page

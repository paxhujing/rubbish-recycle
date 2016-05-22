Feature: Retrieving users
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Retrieving an existing user
	Given an exsiting user
	When it is retrieved
	Then a '200 OK' status is returned
	
Feature: Locate a Soul In Need
	In order to help
	As an application user
	I want to locate a Soul In Need

Scenario: A user provides GPS coordinates
	Given An application user
	When He provides a latitude 3.139003 and a longitude 101.686855
	Then A Soul In Need is created

Scenario: A user provides GPS coordinates nearby a Soul In Need
	Given An application user
	When He provides a latitude 3.139004 and a longitude 101.686854
	Then A Soul In Need is returned if he is within 10 meters

Feature: A Sensitive Soul helps a Soul In Need
	In order to help a Soul In Need
	As a Sensitive Soul
	I want to make a donation 

Scenario: The Souls In Need close to my position are listed
	Given a Sensitive Soul position latitude 3.139004 and longitude 101.686854
	When he searches Souls In Need within 2000 meters
	Then the Souls In Need are listed

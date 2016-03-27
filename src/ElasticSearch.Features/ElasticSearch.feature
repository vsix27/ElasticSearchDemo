Feature: ElasticSearch using Elastic.Net package
	In order to proof concept 
	I want to demonstrated CRUD operations 

@Health
Scenario: See Elastic Health
	Given Elastic is running	
	When I run <GET /_cluster/health>
	Then the result should not be empty

@CRUD
Scenario: Retrieve Item from Elastic Search
  Given Elastic is running	
  When I get index <library> with type <books> with id <2>  	
  Then this item should be found

@CRUD
Scenario: Retrieve NON Existing Item from Elastic Search
  Given Elastic is running	
  When I get index <library> with type <books> with id <GUID>  	
  Then this item should not be found

@CRUD
Scenario: Add Item to Elastic Search
  Given Elastic is running	
  When I put index <library> with type <books> with id <2> with json
	| line                                        |
	| {                                           |
	| "title": "another book about them",         |
	| "name": {                                   |
	| "first": "john",                            |
	| "last": "doe"                               |
	| },                                          |
	| "publish_date": "2016-03-27T06:11:22-0400", |
	| "price": 21.27                              |
	| }                                           |
  Then this item should be found

@CRUD
Scenario: Update Item in Elastic Search
  Given Elastic is running	
  When I update index <library> with type <books> with id <2> with json
	| line                                        |
	| {                                           |
	| "title": "yet another book about GUID",     |	
	| "itin": "GUID"                              |
	| }                                           |
  Then this item should be found with new version

@CRUD
Scenario: Delete Item from Elastic Search
  Given Elastic is running	
  When I put index <library> with type <books> with id <GUID> with json
	| line                             |
	| {                                |
	| "title": "real book about GUID", |
	| "itin": "GUID"                   |
	| }                                |
	And delete this item 
  Then this item should not be found
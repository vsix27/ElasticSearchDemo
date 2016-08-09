@Elastic_NEST
Feature: ElasticSearch using NEST package
	In order to proof concept 
	I want to demonstrated CRUD operations 

@Health
Scenario: See Elastic Health with NEST
	Given Elastic is running	
	When I run <GET /_cluster/health>
	Then the result should not be empty

@CRUD_NEST
Scenario: Retrieve Item from Elastic Search with NEST
  Given Elastic is running	
  When I get index <nestlibrary> with type <calendar> with id <2>  	
  Then this item should be found

@CRUD_NEST
Scenario: Retrieve NON Existing Item from Elastic Search with NEST
  Given Elastic is running	
  When I get index <nestlibrary> with type <calendar> with id <GUID>  	
  Then this item should not be found

@CRUD_NEST
Scenario: Add Item to Elastic Search with NEST
  Given Elastic is running	
  When I put index <nestlibrary> with type <calendar> with id <2> with json
	| line |
	| {  } |
  Then this item should be found

@CRUD_NEST
Scenario: Update Item in Elastic Search with NEST
  Given Elastic is running	
  When I update index <nestlibrary> with type <calendar> with id <2> with json
	| line |
	| { }  |
  Then this item should be found with new version

@CRUD_NEST
Scenario: Delete Item from Elastic Search with NEST
  Given Elastic is running	
  When I put index <nestlibrary> with type <calendar> with id <GUID> with json
	| line                             |
	| { }                              |
	And delete this item 
  Then this item should not be found
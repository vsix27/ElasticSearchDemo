@Elastic_Search
Feature: ElasticSearch using Elastic.Net package
	In order to proof concept 
	I want to demonstrated CRUD operations 

Background: 
    Elastic Search server url: http://172.26.15.7:9200
    See claster at http://www.elastichq.org/app/index.php#
	with enabled CORS (Chrome plugin)
	to see message in web, put
	http://172.26.15.7:9200/library/books/50
	<ElastisSearchUrl>/<index>/<type>/<id>
	it will return json:
	{"_index":"library","_type":"books","_id":"50","_version":1,"found":true,"_source":{"title": "YET MORE pads about it","name": {"first": "nino","last": "rota"},"publish_date": "2010-03-27T06:11:22-0400","price": 55.55}}


Scenario: See Elastic - Health
	Given Elastic is running	
	When I run <GET /_cluster/health>
	Then the result should not be empty

Scenario: Retrieve Item from Elastic Search - CRUD
  Given Elastic is running	
  When I get index <library> with type <books> with id <2>  	
  Then this item should be found

Scenario: Retrieve NON Existing Item from Elastic Search - CRUD
  Given Elastic is running	
  When I get index <library> with type <books> with id <GUID>  	
  Then this item should not be found

Scenario: Add Item to Elastic Search - CRUD
  Given Elastic is running	
  When I put index <library> with type <books> with id <51> with json
	| line                                        |
	| {                                           |
	| "title": "YET MORE pads about it",          |
	| "name": {                                   |
	| "first": "nino",                            |
	| "last": "rota"                              |
	| },                                          |
	| "publish_date": "2010-03-27T06:11:22-0400", |
	| "price": 55.55,                             |
	| "itin": "<GUID>"                            |
	| }                                           |
  Then this item should be found

Scenario: Update Item in Elastic Search - CRUD
  Given Elastic is running	
  When I update index <library> with type <books> with id <2> with json
	| line                                        |
	| {                                           |
	| "title": "yet another book about GUID",     |	
	| "itin": "GUID"                              |
	| }                                           |
  Then this item should be found with new version

Scenario: Delete Item from Elastic Search - CRUD
  Given Elastic is running	
  When I put index <library> with type <books> with id <GUID> with json
	| line                             |
	| {                                |
	| "title": "real book about GUID", |
	| "itin": "<GUID>"                 |
	| }                                |
	And delete this item 
  Then this item should not be found


Scenario: Add Item -location- to Elastic Search - CRUD
  Given Elastic is running	
  When I put index <location> with type <location> with id <1> with json
	| line                                                                                                                                  |
	| {"identifier":[{"system":"2.16.840.1.113883.4.6","value":"5812345679","use":"Official"},                                              |
	| {"system":"2.16.840.1.113883.4.4","value":"123456789","use":"Secondary"},                                                             |
	| {"system":"LU","value":"123456789","use":"Secondary"}],"name":"KILDARE ASSOCIATES",                                                   |
	| "address":{"use":"Work","addressLine":["2345 OCEAN BLVD"],"city":"MIAMI","region":"FL","postalCode":"33111"},                         |
	| "resourceId":"8ce8732c-df22-4bb5-af65-0ef4e50e1473",                                                                                  |
	| "extension":[{"url":"urn:resourceDocumentPath","value":{"string":"Claim/1/Location/1"}}],                                             |
	| "meta":{"sourceMessageType":"eXML","sourceMessageDate":{"value":636060186616679286,"text":"2016-08-05 18:31:01"},                     |
	| "acquiredDate":{"value":636060186642581062,"text":"2016-08-05 18:31:04"},"sourceDocumentId":"eXML_encounter_EB_test.xml"},            |
	| "ResourceDocumentPath":"QOLU3jlvHEmqG6Cs0cHBaqEl0Jo+vQtmfYg4N2TBgYpMr/bjRib1mN9U08QjP835KUxDNY3yQQva/LOl4GHz2A==/Claim/1/Location/1"} |	
  Then this item should be found
  # http://172.26.15.7:9200/location/location/1

Scenario: Add Item -patient- to Elastic Search - CRUD
  Given Elastic is running	
  When I put index <library> with type <books> with id <49> with json
	| line                                                                                           |
	| {                                                                                              |
	| "title": "resourceId: f7b91b47-3c94-4927-a6ce-0b89fbfb23bd-4",                                 |
	| "name": {                                                                                      |
	| "first": "reference: Organization/organizationid-1",                                           |
	| "last": "ResourceDocumentPath: t674bcmehboqkrj3nb96teh34duqnyqbhy5nzfxwxpbcs3yagomy/Patient/1" |
	| },                                                                                             |
	| "publish_date": "2010-03-27T06:11:22-0400",                                                    |
	| "price": 55.55,                                                                                |
	| "itin": "code: OntologyPatient-74b012c1-05d7-4846-8ca9-94e8cfd9131e-2"                         |
	| }                                                                                              |
  Then this item should be found
# How to start the app
 - cd LocationSearch.Api/docker 
 - docker-compose up

# General
  - Has two parts; LocationSearch.Api contains application and LocationSearch.UnitTests contain unit tests.
  - Requires MySQL with DB named roamler, table named Locations [Address(string,text,nvarchar), Latitude(decimal(10,8), Longitude(decimal(11,8)] and data in it.

# LocationSearch.Api
  - LocationController: Straigthforward controller that gets request body, passes it to service and returns the result.
  - LocationService: Gets location information from request body and calculates distance for each location in db, builds a list and returns it.
      - Uses in-memory caching to reduce number of db requests. (In a real world applications; Redis, Memcached or similar caches should be used. 
      This way caching would be a seperate service and does not get affected by problems in application side. Also could be implemented in distributed way.)
      - For development, DB secrets are in "application.Development.json". In production they should be set via secrets if using Kubernetes or similar orchestration tools.
  - LocationDataContext: Basic Entity Framework db context to access DB.
  - FindLocationRequestDto: DTO for getting Location information, distance and/or maximum number of results.
      - All properties are non nullable types thats why when one of those fields are omitted in request body, they are automatically set as default values. This way, to do
      validation required custom validator. But I used a workaround to leave validation to .NET framework; Three fields are set as Required but they are also optional. This way
      framework leaves required properties as null if they are omitted and could do validation. But brings necessity to do type casting when I want to use those properties.
  - Location: Simple data model. Distance field is not mapped to DB table because its calculated in runtime and only used for sorting the list. Id property is omitted from json
    serilization.
  - MySqlSettings: Class to map db setting and getting conenction string. Also will be useful when run in docker container, appsettings can be overriden by env variables
  and settings will work without problem.
  
# LocationSearch.UnitTests
 - LocationControllerTests: Test class to test LocationController.
     - Only test return value's type and null.
     - Test scnearios like; InvalidRequestBody, checking size of return value and such could be added.
 - LocationServiceTests: Test class to test LocationService.
     - Tests only type and null of return value.
     - Test scenarios like; Cache testing and such could be added.
 - Some other tests, DBContext test, Caching test and similar should be added in a real world scenario.   
       

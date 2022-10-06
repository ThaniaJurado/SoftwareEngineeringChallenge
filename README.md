# SoftwareEngineeringChallenge


## Time and space complexity of the algorithm
This code uses a method that receives a list of Marble objects, filters it, sorts it and then it returns a list of Marble objects.
It is made like this in order to be reusable for either serverless apps or RESP APIs. 

I made several time tests with a list of 10,485,760 (adding the list to itself 20 times):
- Sorting first and then filtering by weight and palindrome: 25.500 seconds
- Filtering first by palindrome, then by weight and sorting: 30.600 seconds
- Filtering first by weight, then by palindrome and sorting: 22.300 seconds


## Deployment strategy to host this workload in any cloud platform.
The function I made, if needed, could be implemented in a REST API or in a WCF. For example, it would receive the list of marbles from the client side and return the sorted and filtered list. Also, the function could be reused in other projects. I would put it in the logic layer.  

If I have to choose a what to implement it, it would be in a WCF App because the client would have the definition of the Marble class, so in the client side it will only be necessary to adapt the data to the Marble Class, send the list to the WCF Server and then, in the Server Side, it would be processed and returned. 

If this algorithm  is intended to be used in several apps, I would put it in the Business Logic Layer. What way it is possible to import that .dll in both C# and Visual Basic projects.

## What would you do if Bob has millions of marbles to process?
That is why I made performance tests measuring the execution time of the method that filters and sorts. 

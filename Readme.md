# ChuckNorrisService

The Chuck Norris Service provides an API to Get a joke from `https://api.chucknorris.io/jokes/random`.  The API also provides methods 
to Save, Get All, and Delete jokes.

## To run with the Norris UI

The API is only set up and configured to run using the Development settings.  
The Norris UI is configured to call this api running on https://localhost:44386/ (IIS Express).  If you require to 
run the API using the .Net Core server, the Norris UI \src\app\services\chuck-norris-joke.service.ts will have to be 
updated with the new URL of the API.


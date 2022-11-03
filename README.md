# TwitterClient

## Configuration

A [Twitter Access token](https://developer.twitter.com/ja/docs/basics/authentication/guides/access-tokens) is required to run this application.

Access Tokens are set in the src/appSettings.json file in the TweetCollector:AccessToken property.

Ex:

``{
"TweetCollector": {
    "AccessToken": "<Access Token>"
  }
}``

## Running the app

From the src directory, run:
``dotnet run``

The application will start a background process that will collect tweets from the sample stream and add them to an in memory repository.
Another background process will use hash tags saved in the repository and display the 'top 10' every 10 seconds.

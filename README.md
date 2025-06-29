# Degreed Coding Challenge

## Run the application

From the root 'Degreed' directory execute the following:

**Linux/MacOS**
`./run_all.sh`

**Windows**
`./run_all.bat`

Go to http://localhost:5078

## Instructions

### Technical Challenge
Create an application that uses the "I can haz dad joke" API (https://icanhazdadjoke.com/api) to display jokes. The front-end code should be as simple as possible. Use any front-end mechanism you wish but keep it simple. We're interested in how you'd design and implement the back-end service. Aim to spend no more than 4 hours on this exercise.

### Back-End Service
All the communication with the icanhazdadjoke API and the preparation of data for display should be implemented in a back-end service using C# and .NET.

### User Options
*	Random Joke: Allow the user to fetch a random joke.
*	Search Jokes: Accept a search term and display the first 30 jokes containing that term. The matching term should be emphasized in some simple way (e.g., uppercase, quotes, angle brackets, etc.).

The matching jokes should also be grouped by length:
*	Short: < 10 words
*	Medium: < 20 words
*	Long: >= 20 words

### Technology Choice
We prefer to see your solution implemented in C# and ASP.NET Core so we can discuss your approach within that framework. Our focus is primarily on the back-end services, so feel free to keep the front-end simple and use any mechanism you prefer.

### Design & Adaptability
Please structure your application so that it is easily testable, extensible, and can be adapted to new requirements or features in the future. We look forward to discussing the key decisions and trade-offs you made in your design.

# 🚀 Minimal API with .NET 8.0 – Secure, Fast, and Efficient ![License](https://img.shields.io/badge/license-MIT-blue.svg)
👋 Hey there! Welcome to my super cool Minimal API project built with .NET 8.0. This isn’t just any API—it's a well-oiled machine, leveraging the best practices in software development to ensure your data is safe, your performance is top-notch, and your code is clean and maintainable.


## 🛠️ Technologies and Tools
### This project is powered by some amazing tech and libraries:

- `.NET 8.0 with Minimal API` : Cutting out the bloat, keeping things lean and mean.
- `Repository Pattern` : Because separation of concerns is a real thing!
- `FluentValidation` : Ensuring your data is on point before it even hits the database.
- `AutoMapper` : Mapping DTOs like a boss. No more manual, error-prone mapping.
- `JWT Authorization` : Secure your endpoints with token-based authentication.
- `RateLimiter` : Blocking those pesky DDoS attacks like a superhero.
- `Serilog` : Logging every detail, so you don’t have to. Whether it's an info log or an error, Serilog’s got it covered.


## 📬 Postman Collection
For those who like to tinker around with APIs using Postman, I’ve got you covered:

Get Started with Postman
Import the Collection: Download and import the Postman collection to get all the API endpoints at your fingertips.
Set Up Authorization: Don’t forget to grab your JWT token from the /login endpoint and add it to the Authorization header as a Bearer token. [Download Postman Collection](https://drive.google.com/file/d/1eKoiaycjCja0vRkKHtdynHySrgP6Azfo/view?usp=sharing)

### Explore the Endpoints:

| Method | URL                                           | Body                                                                                                       | Description                                                  |
|:--------:|-----------------------------------------------|------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------|
| POST   | https://localhost:7215/login                  | { "email": "string", "password": "string" }                                                                 | Returns a JWT Token for Authorization.                       |
| POST   | https://localhost:7215/signup                 | { "username": "string", "email": "string", "password": "string" }                                           | Sign up a new user.                                           |
| POST   | https://localhost:7215/api/users/send-request | { "receiverId": "string" }                                                                                  | Send a friend request to a user by their email.               |
| GET    | https://localhost:7215/api/users              | None                                                                                                       | Get a list of all users.                                      |
| GET    | https://localhost:7215/api/users/view-request | None                                                                                                       | View all received friend requests.                            |
| GET    | https://localhost:7215/api/users/pending-status | None                                                                                                       | View pending friend requests.                                 |
| PUT    | https://localhost:7215/api/users/respond-friend | { "senderId": "string", "status": 0 }                                                                       | Respond to a friend request: 1 to accept, 2 to reject.        |
| GET    | https://localhost:7215/api/users/my-friend    | None                                                                                                       | View your friend list.                                        |


--- 

### Pro Tips for Postman Users
 - Environment Variables: Set up environment variables for your baseUrl and token to make testing a breeze.
 - Testing Scenarios: Play around with different scenarios—invalid data, missing tokens, etc., to see how robust this API really is.
### 📖 A Few Words from the Developer
>
> This API was built with care, attention to detail, and a bit of caffeine-fueled enthusiasm. Whether you're using it for a 
> project, learning from it, or just poking around, I hope you find it useful and well-structured. Feel free to contribute,
> suggest improvements, or just drop a line if you want to chat tech. Happy coding!
>

## 📝 License

This project is licensed under the [MIT License](LICENSE.txt). See the [LICENSE](LICENSE.txt) file for details.

## 📬 Contact

Feel free to reach out if you have any questions, suggestions, or just want to connect!

- **LinkedIn** : [Ariya Nayagan](https://www.linkedin.com/in/ariyanayagan-t)
- **Email** 📧 : [aridheena@gmail.com](mailto:aridheena@gmail.com) 

# ASP.NET Core ![Code](https://img.shields.io/badge/Code%20Status-Demo-blueviolet?logo=Visual%20Studio%20Code&labelColor=indigo)

> Begin this lesson by moving this folder into the [`src\`](../../src/) folder.

----

## TOPIC ![Docs](https://img.shields.io/badge/Documentation%20Status-~10%25%20Minimal%20Outline-lightgrey?logo=Read%20the%20Docs)

> The demos for this lesson series will be delivered *ad-hoc* this term. Be sure to follow along in class and refer to the instructor's workbook when necessary.

- **Lesson 020** - Client-Server Web App Setup (w. Database)
  - [ ] Create application
    - [ ] **holiday CSS** site design
  - [ ] Add Class Library
  - [ ] Database Model
    - [ ] Reverse-Engineer
    - [ ] Make classes internal
- **Lesson 021** - Verifying Setup and Deployment Environment
  - [ ] About page with database and server information
    - Db version & date
    - Web Server type, content path & application path
      - `WebRootPath`
      - [Environments](https://docs.microsoft.com/aspnet/core/fundamentals/environments?view=aspnetcore-5.0) and the [`<environment>` tag helper](https://docs.microsoft.com/aspnet/core/mvc/views/tag-helpers/built-in/environment-tag-helper?view=aspnetcore-5.0)
    - Notes on [Tag Helpers](https://docs.microsoft.com/aspnet/core/mvc/views/tag-helpers/built-in/?view=aspnetcore-5.0)
- **Lesson 022** - CRUD Review
  - [ ] CRUD functionality for `Student` information
    - **Backend**
      - Modelling Student Information

        ```csharp
        public record Student(string ID, string FirstName, string LastName);
        ```
      - Backend CRUD Methods and mapping Models to your Entity classes

        ```csharp
        public int Add(Backend.Models.Student) { /* code */ }
        public void Update(int studentId, Backend.Models.Student) { /* code - validate no duplicates */ }
        public void Delete(int studentId, Backend.Models.Student) { /* code */ }
        ```

    - **Frontend**
      - Lookup using a [`<datalist>`](https://developer.mozilla.org/docs/Web/HTML/Element/datalist) on an `<input>`
      - Routing Parameter on a page directive with a `[BindProperty]` that allows for GET values on that route
      - `asp-page-handler` for POST
      - Post-Redirect-Get Pattern
        - Using `[TempData]` for feedback
      - Validation
    - **Test Data** - [RandomUser.me](https://randomuser.me/)
  - [ ] **Practice** - West Wind Demo
    - Create solution, Razor Pages project, and class library.
    - Reverse-engineer Database
    - CRUD for **Shippers**
    - CRUD for **Categories** - *Research handling a file upload of an image for the category and saving it to the database. See [this page](https://docs.microsoft.com/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0#upload-small-files-with-buffered-model-binding-to-a-database)*

> *NEXT Lessons*

- [ ] **Homework *(before next class)*:** View the 12 minute video on ["How to Create Your First Wireframes"](https://www.youtube.com/watch?v=qpH7-KFWZRI&list=PLWtPDlPVWF-9AmUZ49tWVtF3sF8guC5Xj&index=19)
- [ ] Query vs Command
- [ ] Query Samples
  - [ ] Our Staff
    - Distinguish Managers vs Front-Line Sales Staff (Card Layout, w. Pictures, Front-Line staff grouped by Area)
    - Simple form - Drop-down with option group for territories by region to find your area sales reps
  - [ ] Our Products
    - Grouped by Category, w. Category picture
  - [ ] Reports Menu
    - [ ] *Something needing a Table layout*
    - Pagination

----

## References

The following links are selected pages from the older notes for DMIT-2018 and other resources.

### Readings

- **Microsoft Documents**
  - [Tutorial: Get started with Razor Pages in ASP.NET Core](https://docs.microsoft.com/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-5.0&tabs=visual-studio)
  - [ASP.NET Core Fundamentals](https://docs.microsoft.com/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)
  - [App Startup in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/startup?view=aspnetcore-5.0)
  - [Dependency injection in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0)
  - [Razor syntax reference for ASP.NET Core](https://docs.microsoft.com/aspnet/core/mvc/views/razor?view=aspnetcore-5.0)
- **Learn Razor Pages** ([learnrazorpages.com](https://www.learnrazorpages.com/))
  - [Razor Page Files](https://www.learnrazorpages.com/razor-pages)
  - [Razor files](https://www.learnrazorpages.com/razor-pages/files/)
  - [Partial Pages](https://www.learnrazorpages.com/razor-pages/partial-pages)
  - [Layout Files](https://www.learnrazorpages.com/razor-pages/files/layout)
  - [ViewImports File](https://www.learnrazorpages.com/razor-pages/files/viewimports)
  - [ViewStart File](https://www.learnrazorpages.com/razor-pages/files/viewstart)
  - [Razor Syntax](https://www.learnrazorpages.com/razor-syntax)
  - [Page Models](https://www.learnrazorpages.com/razor-pages/pagemodel)
  - [Handler Methods](https://www.learnrazorpages.com/razor-pages/handler-methods)
  - [Action Results](https://www.learnrazorpages.com/razor-pages/action-results)
  - [Tag Helpers](https://www.learnrazorpages.com/razor-pages/tag-helpers/)
  - [Routing and URLs](https://www.learnrazorpages.com/razor-pages/routing)
  - [Startup](https://www.learnrazorpages.com/startup)
  - [Configuration](https://www.learnrazorpages.com/configuration)
  - [Middleware](https://www.learnrazorpages.com/configuration)
  - [Dependency Injection](https://www.learnrazorpages.com/advanced/dependency-injection)
  - [Working with Forms](https://www.learnrazorpages.com/razor-pages/forms) (and subpages)
  - [Validation](https://www.learnrazorpages.com/razor-pages/validation)
  - [Model Binding](https://www.learnrazorpages.com/razor-pages/model-binding)
  - [TempData in Razor Pages](https://www.learnrazorpages.com/razor-pages/tempdata)
- **Tutorials Teacher** ([tutorialsteacher.com/core](https://www.tutorialsteacher.com/core))
  - [Create ASP.NEt Core App](https://www.tutorialsteacher.com/core/first-aspnet-core-application)
  - [Project Structure](https://www.tutorialsteacher.com/core/aspnet-core-application-project-structure)
  - [Dependency Injection](https://www.tutorialsteacher.com/core/dependency-injection-in-aspnet-core)
  - [Middleware](https://www.tutorialsteacher.com/core/aspnet-core-middleware)
  - [Logging in .NET Core](https://www.tutorialsteacher.com/core/fundamentals-of-logging-in-dotnet-core)
  - [Logging in ASP.NET Core](https://www.tutorialsteacher.com/core/aspnet-core-logging)


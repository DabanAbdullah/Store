📌 README for Online Store Project
🛒 Online Store
A modern Online Store built using C#, ASP.NET Core, Entity Framework (EF) Core, and SQL Server, following the Repository Pattern for maintainable and scalable data access.

🚀 Features
✔️ User Authentication & Authorization (Login, Register, Role-based access)
✔️ Product Management (Add, Edit, Delete, List Products)
✔️ Shopping Cart (Add to cart, Remove, Checkout)
✔️ Order Processing (Place orders, Order history)
✔️ Admin Dashboard (Manage users, orders, and inventory)
✔️ EF Core with Repository Pattern (Separation of concerns, maintainable code)
✔️ Secure Payment Integration (Optional)
✔️ SQL Server Database (Optimized for scalability)

🏗️ Tech Stack
Backend: ASP.NET Core, C#
Frontend: Razor Pages / MVC
Database: SQL Server
ORM: Entity Framework Core
Architecture: Repository Pattern, Dependency Injection
📸 Project Screenshot

🔧 Installation
1️⃣ Clone the repository:

sh
Copy
Edit
git clone [https://github.com/YourUsername/OnlineStore.git](https://github.com/DabanAbdullah/Store.git)](https://github.com/DabanAbdullah/Store.git)
cd OnlineStore
2️⃣ Configure the Database Connection:
Modify appsettings.json with your SQL Server connection string:

json
Copy
Edit
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=OnlineStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
3️⃣ Run Migrations & Seed Database:

sh
Copy
Edit
dotnet ef database update
4️⃣ Run the Application:

sh
Copy
Edit
dotnet run
The app will be available at: http://localhost:5000/

📂 Project Structure
pgsql
Copy
Edit
📂 OnlineStore
 ┣ 📂 Controllers
 ┃ ┣ 📜 ProductController.cs
 ┃ ┣ 📜 CartController.cs
 ┃ ┣ 📜 OrderController.cs
 ┃ ┗ 📜 AdminController.cs
 ┣ 📂 Models
 ┃ ┣ 📜 Product.cs
 ┃ ┣ 📜 Order.cs
 ┃ ┣ 📜 CartItem.cs
 ┃ ┗ 📜 User.cs
 ┣ 📂 Repositories
 ┃ ┣ 📜 IProductRepository.cs
 ┃ ┣ 📜 IOrderRepository.cs
 ┃ ┣ 📜 ICartRepository.cs
 ┃ ┗ 📜 GenericRepository.cs
 ┣ 📂 Views
 ┃ ┣ 📜 Home.cshtml
 ┃ ┣ 📜 ProductList.cshtml
 ┃ ┣ 📜 Cart.cshtml
 ┃ ┗ 📜 OrderHistory.cshtml
 ┣ 📂 wwwroot
 ┃ ┣ 📂 css
 ┃ ┣ 📂 js
 ┃ ┗ 📂 images
 ┣ 📜 appsettings.json
 ┗ 📜 Program.cs
💡 Design Pattern: Repository Pattern
The Repository Pattern is used to separate business logic from data access.
Each repository handles data retrieval and modification, improving code maintainability.

## 📸 Project Screenshot  

![Online Store Screenshot](https://drive.google.com/drive/u/0/folders/1NWppGlv-WyfN31tD190QN1lmgWY4gOfs)


👨‍💻 Daban Salahaddin Abdullah

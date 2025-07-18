# Bakery Shop System

## Overview
The Bakery Shop System is a web application designed to manage a bakery's online presence, including product listings, user accounts, order management, and more. It is built using ASP.NET Web Forms and utilizes a SQL Server database for data storage.

## Features
- User registration and login
- Product catalog and menu display
- Shopping cart and checkout process
- Order management and tracking
- Admin and staff management interfaces
- Profile management for users and admins
- Password recovery functionality

## Folder Structure
```
Bakery_Shop_System/
│
├── asg/                # Main ASP.NET Web Forms application
│   ├── App_Data/       # Database files (BakeryDB.mdf, BakeryDB_log.ldf)
│   ├── Images/         # General images for the site
│   ├── Product/        # Product images
│   ├── ProfilePic/     # User profile pictures
│   ├── *.aspx          # Web Forms pages (e.g., Login, Register, Cart, etc.)
│   ├── *.cs            # Code-behind files
│   ├── *.css           # Stylesheets
│   └── ...
│
├── product/            # Duplicate or backup of product images
├── packages/           # NuGet packages and dependencies
├── asg.sln             # Visual Studio solution file
└── README.md           # Project documentation (this file)
```

## Setup Instructions
1. **Requirements:**
   - Visual Studio 2017 or later
   - .NET Framework 4.5 or later
   - SQL Server Express (for local database)

2. **Clone the Repository:**
   ```
   git clone <repository-url>
   ```

3. **Open the Solution:**
   - Open `asg.sln` in Visual Studio.

4. **Restore NuGet Packages:**
   - Visual Studio should automatically restore packages. If not, right-click the solution and select `Restore NuGet Packages`.

5. **Database Setup:**
   - The database file (`BakeryDB.mdf`) is located in `asg/App_Data/`.
   - Ensure SQL Server Express is installed and running.
   - The application should connect automatically if using the default connection string. Adjust `Web.config` if needed.

6. **Run the Application:**
   - Press `F5` or click `Start` in Visual Studio to launch the development server.

## Usage
- Access the application via the browser at the local development URL provided by Visual Studio.
- Register as a new user or log in as an admin/staff to access management features.
- Browse products, add to cart, and proceed to checkout.
- Admins can manage products, orders, and users via the admin interface.

## Technologies Used
- ASP.NET Web Forms (C#)
- SQL Server (LocalDB)
- HTML, CSS
- Visual Studio

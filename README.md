# CareYourSkin

## Project Purpose  
**CareYourSkin** is a skincare application designed to help users track their skincare routine, get product recommendations, and learn about skincare ingredients. Built using **ASP.NET**, this project provides a seamless experience for users who want to improve their skincare habits.

## Features  
-  **Personalized Skincare Routine** – Users can log and track their skincare steps.  
-  **Product Recommendations** – Suggests products based on skin type.  
-  **Ingredient Checker** – Learn about ingredients in skincare products.  
-  **Progress Tracking** – Visual insights into skincare improvements.  

---

##  How to Build & Run the Project  

### **1. Clone the Repository**  
```bash
git clone https://github.com/shreyarenison/CareYourSkin.git
cd CareYourSkin
```

### **2. Open in Visual Studio**  
- Open `CareYourSkin.sln` in **Visual Studio 2022 or later**.  

### **3. Install Dependencies**  
Ensure **.NET SDK 8** is installed, then restore dependencies:  
```bash
dotnet restore
```

### **4. Set Up Database**  
- Configure **appsettings.json** with your database connection string.  
- Run migrations:  
```bash
dotnet ef database update
```

### **5. Run the Project**  
```bash
dotnet run
```
The application will be available at `http://localhost:5000/`.

---

##  License  
This project is licensed under the **MIT License**, allowing anyone to use, modify, and distribute the software with proper attribution.  

### **Why MIT?**  
The **MIT License** was chosen because:  
1. It offers **flexibility** for developers to use and contribute.  
2. It allows both **open-source and commercial use** with minimal restrictions.  
3. It ensures **attribution** while keeping the project open to improvements.  


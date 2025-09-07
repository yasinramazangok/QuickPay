# 🚀 QuickPay

> QuickPay is a learning project built with .NET 9 that simulates a simple card payment system.
> The goal is to understand payment domain concepts, practice .NET 9 API + React integration, and build a production-like containerized architecture with Docker.
> It allows users to enter card details and test payment flows.

---

## 🧰 Technologies & Tools

- ⚙️ **Backend:** ASP.NET Core (.NET 9), Onion Architecture, EF Core
- 🎨 **Frontend:** React (Dockerized, served via Nginx)
- 🗃️ **Database:** SQL Server (running in Docker)
- 📦 **Infrastructure:** Docker, WSL2 (Ubuntu), Nginx Reverse Proxy
- 🧪 **Testing:** xUnit, Moq, FluentAssertions, Coverlet, ReportGenerator
- 📖 **Documentation:** Swagger / OpenAPI

---

## ✨ Features

- ✅ **Payment Simulation:** Card validation, expiry and CVV checks, limit and risk rules
- 🔄 **Containerized Architecture:** API, React, and SQL Server running on the same Docker network
- 🌐 **Reverse Proxy:** Frontend requests managed via Nginx
- 📊 **Unit Testing & Coverage:** PaymentService unit tests with 99% line coverage
- 🧭 **Branching Strategy:** master → development → feature/* for modular development

---

## 📷 Demo / Screenshots

> You can test the API endpoints via Swagger UI or other screenshots.

```
📂 QuickPay/
├── Backend/
│   ├── QuickPay.API/
│   ├── QuickPay.Application/
│   ├── QuickPay.Domain/
│   ├── QuickPay.Infrastructure/
│   └── QuickPay.Test/
├── Frontend/
│   └── React App
├── Nginx/
│   ├── Dockerfile
│   └── nginx.conf
├── .gitattributes
├── .gitignore/
├── LICENSE
└── README.md
```

---

## 📦 Installation & Run

Follow the steps below to clone and run this project locally.

```bash
# 1. Clone the repo
git clone https://github.com/yasinramazangok/QuickPay

# 2. Create Docker network
docker network create quickpay-net

# 3. SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1435:1433 --network quickpay-net --name quickpay-sql -d mcr.microsoft.com/mssql/server:2022-latest

# 4. Build & run API container
docker build -t quickpay-api .
docker run -p 5000:8080 --network quickpay-net --name quickpay-api-c quickpay-api

# 5. Build & run React container
docker build -t quickpay-react .
docker run -p 3000:3000 --network quickpay-net --name quickpay-react-c quickpay-react

# 6. Build & run Nginx container
docker build -t quickpay-nginx .
docker run -p 80:80 --network quickpay-net --name quickpay-nginx-c quickpay-nginx

```

---

## 🧠 What I Learned

- 💵 Payment and card operation fundamentals
- 🧅 Implementing Onion Architecture for layered backend design
- 🐳 Docker networking and container-to-container communication
- 🌐 Configuring Nginx reverse proxy for frontend requests
- 🧪 Writing unit tests with xUnit, Moq, and FluentAssertions
- 📊 Measuring code coverage with Coverlet + ReportGenerator
- 🔀 Applying branching strategy for modular development

---

## 📌 Future Improvements

- 🔐 Add JWT Authentication
- 💳 Integrate with real payment APIs (Stripe / PayPal)
- 🧪 Add integration tests (with InMemory DB for repository)
- 📱 Improve frontend UI and responsiveness
- ☁️ Set up CI/CD pipelines (GitHub Actions / Azure DevOps)

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

Feel free to Fork this repo and submit a Pull Request.

---

## 📬 Contact

**Yasin Ramazan GÖK** 

🌐 LinkedIn: [@yasinramazangok](https://linkedin.com/in/yasinramazangok/)  

📧 Email: [yasinrmzngok@gmail.com](mailto:yasinrmzngok@gmail.com) 

🐙 GitHub: [@yasinramazangok](https://github.com/yasinramazangok)

---

## 📝 License

This project is licensed under the **MIT License** – see the `LICENSE` file for details.


---

You can run the project Using **Docker** 

---

## 🐳 Run with Docker Compose

### 🛠️ Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (for running tests or manual builds)

### ▶️ Steps

```bash
# 1. Clone the repository
git clone https://github.com/saimankhatiwada/GharKhoj.git
cd GharKhoj

# 2. Run containers (app + SQL Server)
docker-compose up -d

# 3. The API will be available at:
http://localhost:5000

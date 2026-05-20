# 📝 TODO App API

Uma **API REST profissional** construída com **ASP.NET Core**, **JWT Authentication** e **SQL Server**. 

## 🎯 Features

- ✅ Autenticação e autorização com JWT
- ✅ CRUD completo de tarefas
- ✅ Banco de dados com Entity Framework Core
- ✅ Validações robustas
- ✅ Testes unitários (xUnit)
- ✅ Documentação com Swagger
- ✅ Containerização com Docker

## 🚀 Quick Start

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-2022)

### Executar Localmente

```bash
# Clone o repositório
git clone https://github.com/Lgi0/TodoApp.git
cd TodoApp/TodoApi

# Restaure dependências
dotnet restore

# Execute as migrations
dotnet ef database update

# Rode a aplicação
dotnet run

# Acesse a documentação
https://localhost:5235/swagger
```

### Com Docker

```bash
cd C:\Dev\TodoApp
docker build -t todoapp:latest .
docker run -p 5000:80 todoapp:latest

# Acesse
http://localhost:5000/swagger
```

## 📚 Estrutura do Projeto

```
TodoApi/
├── Controllers/        # Endpoints HTTP
├── Models/            # Entidades do banco
├── DTOs/              # Data Transfer Objects
├── Services/          # Lógica de negócio
├── Data/              # DbContext
└── Program.cs         # Configuração
```

## 🔗 Endpoints Principais

### Autenticação
```
POST   /api/auth/register   # Registrar usuário
POST   /api/auth/login      # Fazer login
```

### Tarefas (requer autenticação)
```
GET    /api/tasks           # Listar tarefas
POST   /api/tasks           # Criar tarefa
PUT    /api/tasks/{id}      # Atualizar tarefa
DELETE /api/tasks/{id}      # Deletar tarefa
PATCH  /api/tasks/{id}/complete  # Marcar como concluída
```

## 🧪 Testes

```bash
cd C:\Dev\TodoApp
dotnet test
```

## 🔐 Segurança

- **JWT** com expiração de 1 hora
- **BCrypt** para hash de senhas
- **HTTPS** em produção
- Validação de dados com Data Annotations

## 📊 Stack Técnico

- **C# 11** / **.NET 10**
- **ASP.NET Core 10**
- **Entity Framework Core**
- **SQL Server**
- **JWT** (System.IdentityModel.Tokens.Jwt)
- **BCrypt.Net-Next**
- **Swashbuckle.AspNetCore** (Swagger)
- **xUnit** + **Moq** (Testes)
- **Docker**

## 🛠️ Configuração

Crie um arquivo `appsettings.json` com:

```json
{
  "Jwt": {
    "SecretKey": "sua-chave-super-secreta-com-mais-de-32-caracteres",
    "Issuer": "TodoApp",
    "Audience": "TodoAppUsers",
    "ExpirationMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoDb;Trusted_Connection=true;Encrypt=false;"
  }
}
```

## 📚 Recursos Adicionais

- [ASP.NET Core Docs](https://docs.microsoft.com/pt-br/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core)
- [JWT.io](https://jwt.io)

## 📄 License

MIT

---

**Status:** ✅ Completo e em produção

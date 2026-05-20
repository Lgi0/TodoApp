# 📝 TODO App API

Uma **API REST profissional** construída com **ASP.NET Core**, **Entity Framework Core**, **JWT Authentication** e **SQL Server**. Este projeto demonstra uma arquitetura moderna, testes unitários, containerização com Docker e deployment em produção.

## 🎯 Objetivo

Criar uma API RESTful completa que demonstra:
- ✅ Autenticação e autorização com JWT
- ✅ CRUD completo de tarefas
- ✅ Banco de dados relacional com migrations
- ✅ Validações robustas
- ✅ Testes unitários (xUnit)
- ✅ Documentação automática (Swagger)
- ✅ Containerização com Docker
- ✅ CI/CD com GitHub Actions
- ✅ Deploy em produção

## 🚀 Quick Start

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (ou SQLite para desenvolvimento)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional, para containerização)

### Instalação Local

```bash
# Clone o repositório
git clone https://github.com/Lgi0/TodoApp.git
cd TodoApp

# Restaure dependências
dotnet restore

# Execute as migrations (criar banco de dados)
cd TodoApi
dotnet ef database update

# Rode a API
dotnet run

# Acesse a documentação
https://localhost:5235/swagger
```

### Com Docker

```bash
# Build da imagem
docker build -t todoapp:latest .

# Execute o container
docker run -p 5000:80 todoapp:latest

# Acesse
http://localhost:5000/swagger
```

## 📚 Arquitetura

```
TodoApi/
├── Controllers/        # Endpoints HTTP
│   ├── AuthController.cs
│   ├── TasksController.cs
│   └── HealthController.cs
├── Models/            # Entidades do banco
│   ├── User.cs
│   └── Task.cs
├── DTOs/              # Data Transfer Objects
│   ├── UserDto.cs
│   ├── RegisterDto.cs
│   ├── LoginDto.cs
│   ├── TaskDto.cs
│   └── ...
├── Services/          # Lógica de negócio
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── ITaskService.cs
│   └── TaskService.cs
├── Data/              # Acesso ao banco
│   └── AppDbContext.cs
├── Program.cs         # Configuração da aplicação
└── appsettings.json   # Variáveis de ambiente
```

## 🔗 Endpoints

### 🔐 Autenticação (Auth)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| POST | `/api/auth/register` | Registrar novo usuário | ❌ Não |
| POST | `/api/auth/login` | Fazer login | ❌ Não |
| POST | `/api/auth/refresh` | Renovar token | ❌ Não |

**Exemplo: Register**
```bash
curl -X POST http://localhost:5235/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Senha123!",
    "firstName": "João",
    "lastName": "Silva"
  }'
```

**Resposta:**
```json
{
  "success": true,
  "message": "Usuário registrado com sucesso!",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123...",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "João",
    "lastName": "Silva"
  }
}
```

### 📋 Tarefas (Tasks)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/tasks` | Listar suas tarefas | ✅ Sim |
| GET | `/api/tasks/{id}` | Obter uma tarefa | ✅ Sim |
| POST | `/api/tasks` | Criar nova tarefa | ✅ Sim |
| PUT | `/api/tasks/{id}` | Atualizar tarefa | ✅ Sim |
| DELETE | `/api/tasks/{id}` | Deletar tarefa | ✅ Sim |
| PATCH | `/api/tasks/{id}/complete` | Marcar como concluída | ✅ Sim |

**Exemplo: Criar Tarefa**
```bash
curl -X POST http://localhost:5235/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "title": "Estudar C#",
    "description": "Aprender Entity Framework Core",
    "priority": 4,
    "dueDate": "2024-12-31"
  }'
```

### 🏥 Health Check

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/health/test-db` | Verificar conexão com banco |

## 🧪 Testes

```bash
# Rodar todos os testes
dotnet test

# Com verbose
dotnet test --verbosity normal

# Verificar cobertura
dotnet test /p:CollectCoverage=true
```

**Testes Inclusos:**
- ✅ AuthService: Register com dados válidos
- ✅ AuthService: Register com email duplicado
- ✅ AuthService: Login com credenciais válidas
- ✅ AuthService: Login com senha inválida

## 🐳 Docker

### Build
```bash
docker build -t todoapp:latest .
```

### Run Local
```bash
docker run -p 5000:80 todoapp:latest
```

### Run em Produção
```bash
docker push seu-usuario/todoapp:latest
# Na nuvem:
docker run -p 80:80 seu-usuario/todoapp:latest
```

## 🔐 Segurança

### JWT (JSON Web Tokens)
- Tokens expiram em **1 hora**
- Refresh tokens para renovação
- Assinados com chave secreta HMAC SHA256

### Password Hashing
- **BCrypt** para hash de senhas
- Senhas NUNCA são armazenadas em texto plano
- Verificação segura com BCrypt.Verify()

### HTTPS
- Redirecionamento automático em produção
- Certificados SSL/TLS recomendados

## 📊 Banco de Dados

### Tabelas
- **Users**: Usuários registrados
- **Tasks**: Tarefas dos usuários

### Relacionamento
```
Users (1) ──→ (N) Tasks
Um usuário pode ter muitas tarefas
Deletar usuário = deletar suas tarefas (CASCADE)
```

### Migrations
```bash
# Criar nova migration
dotnet ef migrations add NomeMigration

# Aplicar migrations
dotnet ef database update

# Desfazer última migration
dotnet ef migrations remove
```

## 🚀 Deployment

### Heroku
```bash
heroku login
heroku create seu-app-name
git push heroku main
```

### Azure
```bash
az login
az webapp create --resource-group mygroup --plan myplan --name myapp
git push azure main
```

## 📈 Performance

- **InMemoryDatabase** para testes (rápido)
- **Async/await** para operações não-bloqueantes
- **Entity Framework Core** otimizado com LINQ
- Docker multi-stage para imagem mínima

## 🔧 Stack Técnico

| Componente | Versão |
|-----------|--------|
| .NET | 10.0 |
| ASP.NET Core | 10.0 |
| Entity Framework Core | 10.0 |
| SQL Server | 2019+ |
| JWT | System.IdentityModel.Tokens.Jwt |
| BCrypt | BCrypt.Net-Next |
| Swagger | Swashbuckle.AspNetCore |
| xUnit | 2.6+ |
| Moq | 4.20+ |
| Docker | Latest |

## 📝 Configuração

### appsettings.json
```json
{
  "Jwt": {
    "SecretKey": "sua-chave-super-secreta-com-32-chars-minimo",
    "Issuer": "TodoApp",
    "Audience": "TodoAppUsers",
    "ExpirationMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoDb;Trusted_Connection=true;Encrypt=false;"
  }
}
```

## 🐛 Troubleshooting

### "Database connection failed"
```
Verificar se SQL Server está rodando
Confirmar connection string em appsettings.json
Executar: dotnet ef database update
```

### "Port already in use"
```bash
# Mudar porta
dotnet run --urls "https://localhost:7191"
```

### "Docker build fails"
```bash
# Limpar cache
docker system prune -a
# Tentar novamente
docker build -t todoapp:latest .
```

## 📚 Recursos Adicionais

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core)
- [JWT.io](https://jwt.io)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices)

## 🤝 Contribuição

Este é um projeto pessoal para demonstrar habilidades de desenvolvimento. Sugestões são bem-vindas!

## 📄 License

MIT

## 👨‍💻 Autor

**Giovana Lupo**
- GitHub: [@Lgi0](https://github.com/Lgi0)
- Email: seu.email@example.com

---

## ✨ Destaques do Projeto

- ✅ **Arquitetura profissional** com separação de responsabilidades
- ✅ **Testes unitários** com InMemoryDatabase
- ✅ **Documentação automática** com Swagger/OpenAPI
- ✅ **Segurança** com JWT e BCrypt
- ✅ **Containerização** pronta para produção
- ✅ **Git** com commits semânticos
- ✅ **Clean Code** seguindo boas práticas

---

**Última atualização:** Dezembro 2024
**Status:** ✅ Completo e pronto para produção

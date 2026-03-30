# 🚀 Projeto BlogSimples

API desenvolvida para gerenciamento de postagens com autenticação e suporte a notificações via WebSocket.

---

## 📦 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- Docker (opcional)
- Visual Studio (ou .NET SDK compatível)
- PostgreSQL (opcional, caso não use Docker)

---

## Questões
As questões se encontra em \ProjetoBlogSimples\Questoes C#.doc


# ▶️ 1. Executando via código

## 1.1 Banco de dados (PostgreSQL)

É necessário ter uma instância do PostgreSQL rodando.

Caso não tenha, execute:

```bash
cd \ProjetoBlogSimples\Docker\postgres
docker compose up -d
```

---

## 1.2 Executar a aplicação

1. Abra o projeto no **Visual Studio**
2. Compile e execute a aplicação

---

## 1.3 Acessar o Swagger

Após iniciar, a API estará disponível em:

http://localhost:5000/swagger/index.html

---

## 1.4 Notificações (WebSocket)

Para receber notificações em tempo real:

Execute o projeto:

\ProjetoBlogSimples\BlogSimplesNotificacaoCliente

---

# 🐳 2. Executando via Docker

## 2.1 Criar a imagem

```bash
docker build -f docker/Dockerfile -t blogsimples .
```

---

## 2.2 Subir os containers

```bash
docker-compose -f docker/docker-compose.yml up -d --build
```

---

## 2.3 Acessar a aplicação

Após subir os containers, a API estará disponível em:

http://localhost:8080/swagger/index.html

---

# 📘 3. Como usar a API

## 3.1 Cadastrar usuário

Endpoint:

POST /api/v1/usuario

---

## 3.2 Autenticar usuário

Endpoint:

POST /api/v1/autenticar

---

## 3.3 Obter token

Após autenticação, copie o **token JWT** retornado.

---

## 3.4 Criar postagem

Endpoint:

POST /api/v1/postagem

⚠️ É necessário informar o token no header:

Authorization: Bearer {seu_token}

---

# 🧪 Dicas úteis

- Utilize o Swagger para testar os endpoints
- Certifique-se de que o banco está ativo antes de iniciar a API
- Caso tenha problemas, verifique os logs do Docker:

```bash
docker compose logs
```

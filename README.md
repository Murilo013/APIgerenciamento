# API de Gerenciamento de Transporte de Cargas

## Descrição

Esta API é uma aplicação de gerenciamento para transporte de cargas, permitindo o gerenciamento completo de motoristas, caminhões e entregas. A API oferece operações CRUD para cada entidade e suporta a associação de motoristas a caminhões e a atribuição de entregas a motoristas e caminhões.

## Funcionalidades

- **Motoristas:** Cadastro, listagem, atualização e exclusão de motoristas.
- **Caminhões:** Cadastro, listagem, atualização e exclusão de caminhões.
- **Entregas:** Cadastro, listagem, atualização e exclusão de entregas.
- **Associação:** Associar motoristas a caminhões e atribuir uma entrega.

## Tecnologias Utilizadas

- **Backend:** C# e .NET Core
- **Banco de Dados:** PostgreSQL
- **Documentação:** OpenAPI/Swagger

## Instalação

1. Clone o repositório:
- git clone https://github.com/Murilo013/APIgerenciamento.git

2. Vá até diretorio  
- cd nome-do-repositorio

3. Restaure as dependências
- dotnet restore

4. O Projeto está conectado a um banco de dados na nuvem para que seja possível efetuar testes porem se preferirem podem alterar a string de conexão que está na pasta Infra/ConexãoContext
- "Server=;" +
- "Port=;" +
- "Database=;" +
- "User Id=;" +
- "Password=;"

5. Atualize o banco de dados:
- dotnet ef database update

6. Inicie a API:
- dotnet run



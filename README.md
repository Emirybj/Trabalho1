# Projeto Estacionamento:
Sistema completo para gerenciamento de estacionamento de veículos, com back-end em .NET 7 e front-end em React + TypeScript.

## Estrutura do Projeto
```
Trabalho1/
│
├── frontend/              # Aplicação React + TypeScript
└── Trabalho1/             # API ASP.NET Core (.NET 7)
```

## Tecnologias Utilizadas

### Back-end
- .NET 7  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQLite  
- Swagger  

### Front-end
- React  
- TypeScript  
- Axios  
- React Router DOM  
- CSS  

## Instruções para Execução do Projeto:
Rodando o Back-end (.NET)

O back-end deve estar rodando **antes** de iniciar o front-end.

1. Abra o terminal e clone o repositório:

   ```bash
   git clone https://github.com/seuusuario/seuprojeto.git
   cd Trabalho1/Trabalho1
   ```

2. Restaure o banco e inicie a aplicação:

   ```bash
   dotnet ef database update
   dotnet run
   ```

3. Acesse o Swagger para testar manualmente os endpoints:

   ```
   https://localhost:{porta}/swagger
   ```

### Rodando o Front-end (React):
Com o back-end já rodando em uma aba separada do terminal:

1. Abra uma nova aba no terminal:

   ```bash
   cd Trabalho1/frontend
   npm install
   npm start
   ```

2. O projeto será aberto automaticamente em:

   ```
   http://localhost:3000
   ```
## Ordem Sugerida Para Testes:
No Swagger (Back-end)
1. **Cadastrar Tipo de Veículo**  
   `POST /api/TipoVeiculos`
2. **Cadastrar Veículo**  
   `POST /api/Veiculos`
3. **Cadastrar Vaga**  
   `POST /api/Vaga`
4. **Registrar Ticket (Entrada de Veículo)**  
   `POST /api/Ticket`

### Na Web (Front-end)
1. **Cadastrar Tipo de Veículo**  
   [http://localhost:3000/cadastrar-tipo-veiculo](http://localhost:3000/cadastrar-tipo-veiculo)
2. **Gerenciar Vagas**  
   [http://localhost:3000/cadastrar-vaga](http://localhost:3000/cadastrar-vaga)
3. **Registrar Ticket (Home)**  
   [http://localhost:3000/](http://localhost:3000/)
4. **Retirar Veículo**  
   [http://localhost:3000/retirar](http://localhost:3000/retirar)
5. **Histórico de Tickets**  
   [http://localhost:3000/historico](http://localhost:3000/historico)
6. **Listar Vagas**  
   [http://localhost:3000/vagas](http://localhost:3000/vagas)

## Entidades do Sistema
- **Veículo**: Placa, modelo, tipo.  
- **Tipo de Veículo**: Ex: Carro, Moto, etc.  
- **Vaga**: Representa uma vaga disponível ou ocupada.  
- **Ticket**: Entrada, saída e valor a pagar.

##  Endpoints Principais (API)

### Veículos (`/api/Veiculos`)

- `GET /api/Veiculos`
- `GET /api/Veiculos/{id}`
- `POST /api/Veiculos`
- `PUT /api/Veiculos/{id}`
- `DELETE /api/Veiculos/{id}`

### Tipos de Veículo (`/api/TipoVeiculos`)

- `GET /api/TipoVeiculos`
- `GET /api/TipoVeiculos/{id}`
- `POST /api/TipoVeiculos`
- `PUT /api/TipoVeiculos/{id}`
- `DELETE /api/TipoVeiculos/{id}`

### Vagas (`/api/Vaga`)

- `GET /api/Vaga`
- `GET /api/Vaga/Livres`
- `GET /api/Vaga/{id}`
- `POST /api/Vaga`
- `PUT /api/Vaga/{id}`
- `DELETE /api/Vaga/{id}`

### Tickets (`/api/Ticket`)

- `GET /api/Ticket`
- `GET /api/Ticket/{id}`
- `POST /api/Ticket`
- `PUT /api/Ticket/{id}`

## Regras de Negócio

- Um veículo só pode ter **um ticket ativo por vez**.  
- Só é possível usar **vagas livres**.  
- **Cobrança**: R$3,00 por hora (arredondado para cima).  
- **Não excluir** tipo de veículo se estiver em uso.  
- **Não excluir** vaga ocupada.




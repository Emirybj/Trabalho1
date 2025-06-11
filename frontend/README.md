#  Projeto Estacionamento - Front-end react

Interface de usuário para o sistema de gerenciamento de estacionamento de veículos.


##  Tecnologias Utilizadas
- React
- TypeScript
- Axios
- React Router DOM
- CSS

##  Rotas Principais (Páginas)
- **Home**: Registrar ticket.
- **vagas**: Listar Vagas.
- **retirar**: Retirar veiculo.
- **historico**: Historico de Tickets.
- **cadastrar-tipo-veiculo**: Cadastrar tipo de veiculo
- **cadastrar-vaga**: Gerenciar vagas.

##  Como Rodar o Projeto
1. Clone o repositório:
   ```bash
   git clone https://github.com/seuusuario/seuprojeto.git

    cd Documents
    cd Trabalho1
    cd Trabalho1
    dotnet ef database update
    dotnet run 
    - deixe essa aba aberta pois precisa dela para rodar o npm start

2. Abra outra aba no bash 
    cd Documents
    cd Trabalho1
    cd frontend
    npm start (ele abrira automaticamente na web)

## Ordem Sugerida Para Testar no Swagger
1. Cadastrar Tipo de Veículo: http://localhost:3000/cadastrar-tipo-veiculo
2. Gerenciar Vagas: http://localhost:3000/cadastrar-vaga
3. Registrar Ticket (Home): http://localhost:3000/
4. Retirar Veículo: http://localhost:3000/retirar
5. Histórico de Tickets: http://localhost:3000/historico
6. Listar Vagas: http://localhost:3000/vagas





## Projeto Estacionamento - Front-end React
Interface de usuário para o sistema de gerenciamento de estacionamento de veículos.

## Tecnologias Utilizadas:
React
TypeScript
Axios
React Router DOM
CSS

## Rotas Principais (Páginas):
- / (Home - Registrar Ticket)
- /vagas (Listar Vagas)
- /retirar (Retirar Veículo)
- /historico (Histórico de Tickets)
- /cadastrar-tipo-veiculo (Cadastrar Tipo de Veículo)
- /cadastrar-vaga (Gerenciar Vagas)

## Como Rodar o Projeto: 
- Pré-requisitos:
Node.js e npm (ou Yarn) instalados.
O Back-end API (.NET 7) deve estar rodando em http://localhost:5285.
Clone o repositório (se ainda não o fez):
git clone https://github.com/seuusuario/seuprojeto.git

- Navegue para a pasta do Front-end:
cd seuprojeto/frontend
Instale as dependências:
npm install
ou
yarn install

## Configure a URL da API:
- Na pasta raiz do Front-end (frontend/), crie um arquivo chamado .env e adicione a seguinte linha:
REACT_APP_API_URL=http://localhost:5285/api
Inicie o Back-end (em um terminal SEPARADO):
Abra uma nova janela do terminal e navegue até a pasta raiz do seu projeto Back-end (ex: seuprojeto/Trabalho1).
dotnet run
(O Back-end geralmente será executado em https://localhost:5285 ou http://localhost:5285).

- Inicie a aplicação React (em OUTRO terminal SEPARADO):
Volte para o terminal onde você está na pasta Front-end (seuprojeto/frontend).
npm start
ou
yarn start
A aplicação será aberta automaticamente no seu navegador em http://localhost:3000.

## Ordem Sugerida Para Testar:

Cadastrar Tipo de Veículo: http://localhost:3000/cadastrar-tipo-veiculo

Gerenciar Vagas: http://localhost:3000/cadastrar-vaga

Registrar Ticket (Home): http://localhost:3000/

Retirar Veículo: http://localhost:3000/retirar

Histórico de Tickets: http://localhost:3000/historico

Listar Vagas: http://localhost:3000/vagas
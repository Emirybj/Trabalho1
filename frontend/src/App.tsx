import { BrowserRouter, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import './global.css';


import ListarVagas from "./pages/ListarVagas"; 
import RegistrarTicket from "./pages/RegistrarTicket";
import RetirarVeiculo from "./pages/RetirarVeiculo";
import HistoricoTickets from "./pages/HistoricoTickets";
import CadastroTipoVeiculo from "./pages/CadastroTipoVeiculo";
import CadastroVaga from "./pages/CadastroVaga";

function App() {
  return (
    <div className="app-container">
      <BrowserRouter>
        <Header />

        <main className="app-main-content">
          <Routes>
            <Route path="/" element={<RegistrarTicket />} />
            <Route path="/vagas" element={<ListarVagas/>} />
            <Route path="/retirar" element={<RetirarVeiculo />} />
            <Route path="/historico" element={<HistoricoTickets />} />
            <Route path="/cadastrar-tipo-veiculo" element={<CadastroTipoVeiculo />} />
            <Route path="/cadastrar-vaga" element={<CadastroVaga />} />
          </Routes>
        </main>
      </BrowserRouter>
    </div>
  );
}

export default App;





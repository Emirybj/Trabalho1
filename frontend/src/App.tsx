import { BrowserRouter, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import './global.css';

// Importe seus componentes de página
import ListarVagas from "./pages/ListarVagas"; // Manter importação, pois a rota ainda pode existir
import RegistrarTicket from "./pages/RegistrarTicket";
import RetirarVeiculo from "./pages/RetirarVeiculo";
import HistoricoTickets from "./pages/HistoricoTickets";
import CadastroTipoVeiculo from "./pages/CadastroTipoVeiculo";
import CadastroVaga from "./pages/CadastroVaga"; // Este agora gerencia vagas

function App() {
  return (
    <div className="app-container">
      <BrowserRouter>
        <Header />

        <main className="app-main-content">
          <Routes>
            <Route path="/" element={<RegistrarTicket />} />
            {/* Manter a rota /vagas se o componente ListarVagas.tsx ainda for acessível de outra forma,
                ou se você quiser mantê-la como uma página de "apenas visualização"
                <Route path="/vagas" element={<ListarVagas />} />
            */}
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





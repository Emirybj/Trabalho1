import { Link } from 'react-router-dom';
import './Header.css';

function Header() {
    return (
        <header className="app-header">
            <nav className="navbar">
                <div className="logo">Estacionamento</div>
                <ul className="nav-links">
                    <li>
                        <Link to="/" className="nav-link">
                            Home (Registrar Ticket)
                        </Link>
                    </li>
                    {}
                    {}
                    <li>
                        <Link to="/retirar" className="nav-link">
                            Retirar Veículo
                        </Link>
                    </li>
                    <li>
                        <Link to="/historico" className="nav-link">
                            Histórico de Tickets
                        </Link>
                    </li>
                    <li>
                        <Link to="/cadastrar-tipo-veiculo" className="nav-link">
                            Cadastrar Tipo de Veículo
                        </Link>
                    </li>
                    <li>
                        {}
                        <Link to="/cadastrar-vaga" className="nav-link">
                            Gerenciar Vagas
                        </Link>
                    </li>
                </ul>
            </nav>
        </header>
    );
}

export default Header;


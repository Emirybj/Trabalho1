import { useState, useEffect } from 'react';
import axios from 'axios';
import { TipoVeiculo } from '../models/TipoVeiculo';
import './RegistrarTicket.css';
import { useNavigate } from 'react-router-dom';

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

function RegistrarTicket() {
    const [placa, setPlaca] = useState<string>('');
    const [modelo, setModelo] = useState<string>('');
    const [tipoVeiculoId, setTipoVeiculoId] = useState<number | undefined>(undefined);
    const [tipos, setTipos] = useState<TipoVeiculo[]>([]);
    const [erro, setErro] = useState<string>('');
    const [sucesso, setSucesso] = useState<string>('');
    const [carregando, setCarregando] = useState<boolean>(true);

    const navigate = useNavigate();

    useEffect(() => {
        setCarregando(true);
        setErro('');
        axios.get(`${API_BASE_URL}/TipoVeiculos`)
            .then(response => {
                setTipos(response.data);
                if (response.data.length > 0) {
                    setTipoVeiculoId(response.data[0].id);
                } else {
                    setErro("Nenhum tipo de veículo disponível. Por favor, cadastre um na página 'Cadastrar Tipo de Veículo'.");
                }
            })
            .catch(error => {
                console.error("Erro ao carregar tipos de veículo:", error);
                setErro("Erro ao carregar tipos de veículo. Verifique sua conexão ou tente novamente.");
            })
            .finally(() => {
                setCarregando(false);
            });
    }, []);

    const salvar = async (e: React.FormEvent) => {
        e.preventDefault();
        setErro('');
        setSucesso('');

        if (!placa.trim()) {
            setErro("A placa é obrigatória.");
            return;
        }
        if (!modelo.trim()) {
            setErro("O modelo é obrigatório.");
            return;
        }
        if (tipoVeiculoId === undefined) {
            setErro("Por favor, selecione um tipo de veículo.");
            return;
        }

        try {
            // AGORA, SÓ HÁ UMA CHAMADA POST PARA O TICKETCONTROLLER
            // Ele vai lidar com a criação/busca do ve\u00EDculo e cria\u00E7\u00E3o do ticket.
            const requestData = {
                placa,
                modelo,
                tipoVeiculoId
            };
            await axios.post(`${API_BASE_URL}/Ticket`, requestData); // Envia tudo para /api/Ticket

            setSucesso("Ticket registrado com sucesso! Você pode ver o histórico ou as vagas.");
            setPlaca('');
            setModelo('');
            setTipoVeiculoId(tipos.length > 0 ? tipos[0].id : undefined);

        } catch (error: any) {
            console.error("Erro ao registrar ticket:", error);
            if (error.response && error.response.data) {
                // Tenta extrair a mensagem de erro do backend para o usu\u00E1rio
                if (typeof error.response.data === 'string') {
                     setErro(`Erro ao registrar ticket: ${error.response.data}`);
                } else if (error.response.data.title) {
                    setErro(`Erro ao registrar ticket: ${error.response.data.title} - ${error.response.data.detail || ''}`);
                } else if (error.response.data.errors) {
                    const validationErrors = Object.values(error.response.data.errors).flat();
                    setErro(`Erro de validação: ${validationErrors.join(' ')}`);
                } else {
                    setErro("Erro ao registrar ticket. Verifique os dados e tente novamente.");
                }
            } else if (error.request) {
                setErro("Erro de rede: Não foi possível conectar ao servidor. Verifique sua conexão.");
            } else {
                setErro("Erro na configuração da requisição. Tente novamente.");
            }
        }
    };

    // ... (restante do seu componente RegistrarTicket.tsx)
    return (
        <div className="home-container">
            <h1>Registrar Novo Ticket</h1>
            {carregando && <p className="loading-message">Carregando tipos de veículo...</p>}
            {erro && <p className="error-message">{erro}</p>}
            
            {sucesso && (
                <div className="success-message">
                    {sucesso}
                    <div className="navigation-options">
                        <button 
                            className="button secondary" 
                            onClick={() => navigate('/historico')}
                        >
                            Ver Histórico de Tickets
                        </button>
                        <button 
                            className="button secondary" 
                            onClick={() => navigate('/vagas')} // Mudei para /cadastrar-vaga se essa é sua página de gestão
                        >
                            Ver Vagas Ocupadas
                        </button>
                    </div>
                </div>
            )}
            
            {!carregando && !erro && !sucesso && (
                <form onSubmit={salvar} className="parking-form">
                    <div className="form-group">
                        <label htmlFor="placa">Placa:</label>
                        <input
                            id="placa"
                            value={placa}
                            onChange={e => setPlaca(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="modelo">Modelo:</label>
                        <input
                            id="modelo"
                            value={modelo}
                            onChange={e => setModelo(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="tipoVeiculo">Tipo de Veículo:</label>
                        <select
                            id="tipoVeiculo"
                            value={tipoVeiculoId ?? ''}
                            onChange={e => setTipoVeiculoId(Number(e.target.value))}
                            required
                        >
                            {tipos.length === 0 && <option value="">Nenhum tipo disponível</option>}
                            {tipos.map(tipo => (
                                <option key={tipo.id} value={tipo.id}>{tipo.nome}</option>
                            ))}
                        </select>
                    </div>
                    <button type="submit" className="button primary" disabled={carregando || tipos.length === 0}>
                        Registrar Ticket
                    </button>
                </form>
            )}
        </div>
    );
}

export default RegistrarTicket;

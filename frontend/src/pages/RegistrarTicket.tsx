import { useState, useEffect } from 'react'; 
import axios from 'axios'; 
import { TipoVeiculo } from '../models/TipoVeiculo'; 
import './RegistrarTicket.css';
import { useNavigate } from 'react-router-dom'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api"; 

function RegistrarTicket() {
    // Estados para os campos do formulário
    const [placa, setPlaca] = useState<string>(''); // Estado para a placa do veículo
    const [modelo, setModelo] = useState<string>(''); // Estado para o modelo do veículo
    const [tipoVeiculoId, setTipoVeiculoId] = useState<number | undefined>(undefined); // Estado para o ID do tipo de veículo selecionado
    const [tipos, setTipos] = useState<TipoVeiculo[]>([]); // Estado para armazenar a lista de tipos de veículo disponíveis

    // Estados para feedback ao usuário
    const [erro, setErro] = useState<string>(''); // Estado para exibir mensagens de erro
    const [sucesso, setSucesso] = useState<string>(''); // Estado para exibir mensagens de sucesso
    const [carregando, setCarregando] = useState<boolean>(true); // Estado para controlar o carregamento de dados (tipos de veículo)

    const navigate = useNavigate(); // Hook para navegar entre rotas da aplicação

    // Efeito colateral para carregar os tipos de veículo assim que o componente é montado
    useEffect(() => {
        setCarregando(true); // Inicia o estado de carregamento
        setErro(''); // Limpa mensagens de erro anteriores
        axios.get(`${API_BASE_URL}/TipoVeiculos`) // Faz uma requisição GET para buscar os tipos de veículo
            .then(response => {
                setTipos(response.data); // Atualiza o estado com os tipos de veículo recebidos
                if (response.data.length > 0) {
                    setTipoVeiculoId(response.data[0].id); // Seleciona o primeiro tipo como padrão se houver
                } else {
                    setErro("Nenhum tipo de veículo disponível. Por favor, cadastre um na página 'Cadastrar Tipo de Veículo'."); // Mensagem se não houver tipos
                }
            })
            .catch(error => {
                console.error("Erro ao carregar tipos de veículo:", error); // Loga o erro no console
                setErro("Erro ao carregar tipos de veículo. Verifique sua conexão ou tente novamente."); // Mensagem de erro para o usuário
            })
            .finally(() => {
                setCarregando(false); // Finaliza o estado de carregamento
            });
    }, []); // Array de dependências vazio, executa apenas na montagem

    // Função assíncrona para lidar com o envio do formulário (salvar ticket)
    const salvar = async (e: React.FormEvent) => {
        e.preventDefault(); // Previne o comportamento padrão de recarregar a página

        setErro(''); // Limpa mensagens de erro anteriores
        setSucesso(''); // Limpa mensagens de sucesso anteriores

        // Validações dos campos do formulário
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
            const requestData = {
                placa,
                modelo,
                tipoVeiculoId
            };
            // Envia uma requisição POST para a API para registrar o ticket
            await axios.post(`${API_BASE_URL}/Ticket`, requestData);

            setSucesso("Ticket registrado com sucesso! Você pode ver o histórico ou as vagas."); // Define mensagem de sucesso
            // Limpa os campos do formulário após o sucesso
            setPlaca('');
            setModelo('');
            setTipoVeiculoId(tipos.length > 0 ? tipos[0].id : undefined);

        } catch (error: any) {
            console.error("Erro ao registrar ticket:", error); // Loga o erro completo para depuração

            // Lógica para tratar e exibir erros da API ou de rede
            if (error.response && error.response.data) {
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

    return (
        <div className="home-container">
            <h1>Registrar Novo Ticket</h1>
            {/* Exibe mensagem de carregamento */}
            {carregando && <p className="loading-message">Carregando tipos de veículo...</p>}
            {/* Exibe mensagem de erro */}
            {erro && <p className="error-message">{erro}</p>}
            
            {/* Se o ticket foi registrado com sucesso, exibe mensagem e botões de navegação */}
            {sucesso && (
                <div className="success-message">
                    {sucesso}
                    <div className="navigation-options">
                        <button 
                            className="button secondary" 
                            onClick={() => navigate('/historico')} // Navega para a página de histórico de tickets
                        >
                            Ver Histórico de Tickets
                        </button>
                        <button 
                            className="button secondary" 
                            onClick={() => navigate('/cadastrar-vaga')} // Navega para a página de gerenciamento de vagas
                        >
                            Ver Vagas Ocupadas
                        </button>
                    </div>
                </div>
            )}
            
            {/* Exibe o formulário se não estiver carregando, não houver erros e não houver sucesso */}
            {!carregando && !erro && !sucesso && (
                <form onSubmit={salvar} className="parking-form">
                    <div className="form-group">
                        <label htmlFor="placa">Placa:</label>
                        <input
                            id="placa"
                            value={placa}
                            onChange={e => setPlaca(e.target.value)} // Atualiza o estado da placa
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="modelo">Modelo:</label>
                        <input
                            id="modelo"
                            value={modelo}
                            onChange={e => setModelo(e.target.value)} // Atualiza o estado do modelo
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="tipoVeiculo">Tipo de Veículo:</label>
                        <select
                            id="tipoVeiculo"
                            value={tipoVeiculoId ?? ''}
                            onChange={e => setTipoVeiculoId(Number(e.target.value))} // Atualiza o ID do tipo de veículo
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
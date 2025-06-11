import { useEffect, useState } from 'react'; 
import axios from 'axios'; 
import { Vaga } from '../models/Vaga'; 
import './listar-vagas-modulo.css'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api"; 

function ListarVagas() {
    // Estado para armazenar a lista de vagas recebida da API
    const [vagas, setVagas] = useState<Vaga[]>([]);
    // Estado para armazenar e exibir mensagens de erro
    const [erro, setErro] = useState<string>('');
    // Estado para controlar o carregamento dos dados (true enquanto carregando, false depois)
    const [carregando, setCarregando] = useState<boolean>(true);

    // Função para buscar os dados das vagas da API
    function carregarVagas() {
        setCarregando(true); // Indica que o carregamento começou
        setErro(''); // Limpa qualquer erro anterior
        
        axios.get<Vaga[]>(`${API_BASE_URL}/Vaga`) // Faz uma requisição GET para a rota de Vagas
            .then(response => {
                setVagas(response.data); // Atualiza o estado 'vagas' com os dados recebidos
            })
            .catch(error => {
                console.error("Erro ao carregar as vagas:", error); // Loga o erro no console para depuração
                setErro("Erro ao carregar as vagas. Verifique sua conexão ou tente novamente."); // Define uma mensagem de erro para o usuário
            })
            .finally(() => {
                setCarregando(false); // Indica que o carregamento terminou (com ou sem sucesso)
            });
    }

    //Executa 'carregarVagas' uma vez ao montar o componente
    useEffect(() => {
        carregarVagas();
    }, []); // Array de dependências vazio significa que a função só roda na montagem

    return (
        <div className="vagas-container">
            <h1 className="page-title">Lista de Vagas</h1>

            {/* Exibe mensagem de carregamento enquanto os dados estão sendo buscados */}
            {carregando && <p className="loading-message">Carregando vagas...</p>}
            {/* Exibe mensagem de erro se houver algum problema */}
            {erro && <p className="error-message">{erro}</p>}

            {/* Exibe mensagem se não houver vagas e o carregamento terminou sem erros */}
            {!carregando && vagas.length === 0 && !erro && (
                <p className="no-records-message">Nenhuma vaga encontrada.</p>
            )}

            {/* Exibe a tabela de vagas se o carregamento terminou e há vagas */}
            {!carregando && vagas.length > 0 && (
                <div className="table-responsive">
                    <table className="vagas-table">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Número</th>
                                <th>Tipo</th>
                                <th>Status</th>
                                <th>Veículo Ocupante (ID)</th>
                                {/* As colunas 'Andar' e 'Setor' foram removidas desta listagem conforme as instruções anteriores */}
                            </tr>
                        </thead>
                        <tbody>
                            {/* Mapeia a lista de vagas para criar uma linha da tabela para cada vaga */}
                            {vagas.map(vaga => (
                                <tr key={vaga.id}> {/* 'key' é importante para a performance do React */}
                                    <td>{vaga.id}</td>
                                    <td>{vaga.numero}</td>
                                    <td>{vaga.tipo?.nome || "N/A"}</td> {/* Acessa o nome do tipo do veículo (com fallback) */}
                                    <td>
                                        {/* Exibe o status da vaga com um estilo visual diferente */}
                                        <span className={`status-badge ${vaga.ocupada ? 'status-occupied' : 'status-available'}`}>
                                            {vaga.ocupada ? "Ocupada" : "Livre"}
                                        </span>
                                    </td>
                                    {/* Mostra o ID do veículo se a vaga estiver ocupada, senão exibe "-" */}
                                    <td>{vaga.ocupada ? (vaga.veiculoId || "-") : "-"}</td>
                                    {/* As células de dados para 'Andar' e 'Setor' foram removidas desta listagem */}
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default ListarVagas;
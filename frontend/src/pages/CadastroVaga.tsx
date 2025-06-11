import { useState, useEffect } from 'react';
import axios from 'axios';
import { TipoVeiculo } from '../models/TipoVeiculo';
import { Vaga } from '../models/Vaga'; 
import './CadastroVaga.css'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

/**
 * Componente React para cadastrar e gerenciar vagas.
 * Permite ao usuário criar novas vagas e remover vagas existentes.
 */
function CadastroVaga() {
    // Estados para o formulário de Cadastro
    const [numero, setNumero] = useState<string>('');
    const [andar, setAndar] = useState<string>('');
    const [setor, setSetor] = useState<string>('');
    const [tipoVeiculoId, setTipoVeiculoId] = useState<number | undefined>(undefined);
    const [tipos, setTipos] = useState<TipoVeiculo[]>([]); // Para o dropdown de tipos
    const [carregandoTipos, setCarregandoTipos] = useState<boolean>(true);

    // --- Estados para a listagem de Vagas Existentes ---
    const [vagasExistentes, setVagasExistentes] = useState<Vaga[]>([]);
    const [carregandoVagas, setCarregandoVagas] = useState<boolean>(true);

    // Estados para mensagens de feedback
    const [erro, setErro] = useState<string>('');
    const [sucesso, setSucesso] = useState<string>('');

    // Funções de Carregamento de Dados 

    // Carrega os tipos de veículo para o dropdown
    useEffect(() => {
        setCarregandoTipos(true);
        setErro('');
        axios.get<TipoVeiculo[]>(`${API_BASE_URL}/TipoVeiculos`)
            .then(response => {
                setTipos(response.data);
                if (response.data.length > 0) {
                    setTipoVeiculoId(response.data[0].id);
                } else {
                    setErro("Nenhum tipo de veículo disponível. Cadastre um para poder associar a vagas.");
                }
            })
            .catch(error => {
                console.error("Erro ao carregar tipos de veículo para o cadastro de vaga:", error);
                setErro("Erro ao carregar tipos de veículo. Verifique sua conexão ou tente novamente.");
            })
            .finally(() => {
                setCarregandoTipos(false);
            });
    }, []);

    // Carrega a lista de vagas existentes
    const carregarVagasExistentes = async () => {
        setCarregandoVagas(true);
        setErro('');
        try {
            const response = await axios.get<Vaga[]>(`${API_BASE_URL}/Vaga`);
            setVagasExistentes(response.data);
        } catch (error) {
            console.error("Erro ao carregar vagas existentes:", error);
            setErro("Erro ao carregar vagas existentes. Verifique sua conexão ou tente novamente.");
        } finally {
            setCarregandoVagas(false);
        }
    };

    // Recarrega vagas existentes ao montar e após criar/remover
    useEffect(() => {
        carregarVagasExistentes();
    }, []); // Executa apenas uma vez ao montar

    // Funções de Ação Submit e Delete

    // Lida com o envio do formulário de criação de vaga
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErro('');
        setSucesso('');

        const numeroVaga = parseInt(numero, 10);
        if (isNaN(numeroVaga) || numeroVaga < 1 || numeroVaga > 999) {
            setErro("Número da vaga deve ser um valor entre 1 e 999.");
            return;
        }
        if (tipoVeiculoId === undefined || tipoVeiculoId === null) {
            setErro("Por favor, selecione um tipo de veículo para a vaga.");
            return;
        }
        if (tipos.length === 0) {
            setErro("Não há tipos de veículo cadastrados para associar a vaga. Cadastre um tipo primeiro.");
            return;
        }

        try {
            const novaVaga = {
                numero: numeroVaga,
                ocupada: false,
                veiculoId: null,
                tipoVeiculoId: tipoVeiculoId,
                andar: andar.trim() === '' ? null : andar.trim(),
                setor: setor.trim() === '' ? null : setor.trim(),
            };

            await axios.post(`${API_BASE_URL}/Vaga`, novaVaga);

            setSucesso(`Vaga ${numeroVaga} cadastrada com sucesso!`);
            // Limpa o formulário e recarrega a lista de vagas
            setNumero('');
            setAndar('');
            setSetor('');
            setTipoVeiculoId(tipos.length > 0 ? tipos[0].id : undefined);
            carregarVagasExistentes(); // Recarrega a lista para mostrar a nova vaga

        } catch (error: any) {
            console.error("Erro ao cadastrar vaga:", error);
            if (error.response && error.response.data) {
                setErro(`Erro ao cadastrar vaga: ${error.response.data}`);
            } else {
                setErro("Erro ao cadastrar vaga. Verifique sua conexão ou tente novamente.");
            }
        }
    };

    // Para exclusão de uma vaga
    const handleDelete = async (idVaga: number) => {
        setErro('');
        setSucesso('');

        if (!window.confirm(`Tem certeza que deseja remover a vaga ID ${idVaga}?`)) {
            return;
        }

        try {
            await axios.delete(`${API_BASE_URL}/Vaga/${idVaga}`);
            setSucesso(`Vaga ID ${idVaga} removida com sucesso!`);
            carregarVagasExistentes(); // Vai recarregar a lista após a remoção
        } catch (error: any) {
            console.error("Erro ao remover vaga:", error);
            if (error.response && error.response.data) {
                setErro(`Erro ao remover vaga: ${error.response.data}`);
            } else {
                setErro("Erro ao remover vaga. Verifique a conexão.");
            }
        }
    };

    // Renderização do Componente

    return (
        <div className="management-container"> {/* Novo container principal */}
            <h1 className="management-title">Gestão de Vagas</h1>

            {}
            {erro && (
                <div className="alert error-alert" role="alert">
                    <strong>Erro!</strong> {erro}
                </div>
            )}
            {sucesso && (
                <div className="alert success-alert" role="alert">
                    <strong>Sucesso!</strong> {sucesso}
                </div>
            )}

            {/* Seção de Cadastro de Nova Vaga */}
            <section className="management-section">
                <h2 className="section-title">Cadastrar Nova Vaga</h2>
                {carregandoTipos ? (
                    <p className="loading-message">Carregando tipos de veículo...</p>
                ) : (
                    <form onSubmit={handleSubmit} className="standard-form">
                        <div className="form-group">
                            <label htmlFor="numeroVaga">Número da Vaga:</label>
                            <input
                                type="number"
                                id="numeroVaga"
                                value={numero}
                                onChange={(e) => setNumero(e.target.value)}
                                required
                                min="1"
                                max="999"
                                placeholder="Ex: 101"
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="andarVaga">Andar (Opcional):</label>
                            <input
                                type="text"
                                id="andarVaga"
                                value={andar}
                                onChange={(e) => setAndar(e.target.value)}
                                placeholder="Ex: Térreo, Subsolo 1"
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="setorVaga">Setor (Opcional):</label>
                            <input
                                type="text"
                                id="setorVaga"
                                value={setor}
                                onChange={(e) => setSetor(e.target.value)}
                                placeholder="Ex: A, B, C"
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="tipoVeiculoVaga">Tipo de Veículo para a Vaga:</label>
                            <select
                                id="tipoVeiculoVaga"
                                value={tipoVeiculoId ?? ''}
                                onChange={(e) => setTipoVeiculoId(Number(e.target.value))}
                                required
                                disabled={tipos.length === 0}
                            >
                                {tipos.length === 0 ? (
                                    <option value="">Carregando tipos...</option>
                                ) : (
                                    tipos.map(tipo => (
                                        <option key={tipo.id} value={tipo.id}>
                                            {tipo.nome}
                                        </option>
                                    ))
                                )}
                            </select>
                            {tipos.length === 0 && !carregandoTipos && (
                                <p className="help-text error">
                                    Por favor, cadastre tipos de veículo primeiro.
                                </p>
                            )}
                        </div>
                        <div className="form-actions">
                            <button
                                type="submit"
                                className="button submit-button"
                                disabled={carregandoTipos || tipos.length === 0}
                            >
                                Cadastrar Vaga
                            </button>
                        </div>
                    </form>
                )}
            </section>

            {/* Seção de Vagas Existentes */}
            <section className="management-section existing-vagas-section">
                <h2 className="section-title">Vagas Existentes</h2>
                {carregandoVagas ? (
                    <p className="loading-message">Carregando vagas...</p>
                ) : vagasExistentes.length === 0 ? (
                    <p className="no-records-message">Nenhuma vaga encontrada.</p>
                ) : (
                    <div className="table-responsive">
                        <table className="vagas-table">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Número</th>
                                    <th>Tipo</th>
                                    <th>Status</th>
                                    <th>Andar</th>
                                    <th>Setor</th>
                                    <th>Ações</th> {}
                                </tr>
                            </thead>
                            <tbody>
                                {vagasExistentes.map(vaga => (
                                    <tr key={vaga.id}>
                                        <td data-label="ID">{vaga.id}</td>
                                        <td data-label="Número">{vaga.numero}</td>
                                        <td data-label="Tipo">{vaga.tipo?.nome || "N/A"}</td>
                                        <td data-label="Status">
                                            <span className={`status-badge ${vaga.ocupada ? 'status-occupied' : 'status-available'}`}>
                                                {vaga.ocupada ? "Ocupada" : "Livre"}
                                            </span>
                                        </td>
                                        <td data-label="Andar">{vaga.andar || "-"}</td>
                                        <td data-label="Setor">{vaga.setor || "-"}</td>
                                        <td data-label="Ações">
                                            <button 
                                                onClick={() => handleDelete(vaga.id)} 
                                                className="button remove-button"
                                                disabled={vaga.ocupada} // Não permite remover vaga ocupada (backend também valida)
                                            >
                                                Remover
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </section>
        </div>
    );
}

export default CadastroVaga;

import { useEffect, useState } from 'react';
import axios from 'axios';
import { Vaga } from '../models/Vaga';
import './listar-vagas-modulo.css';

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api";

function ListarVagas() {
    const [vagas, setVagas] = useState<Vaga[]>([]);
    const [erro, setErro] = useState<string>('');
    const [carregando, setCarregando] = useState<boolean>(true);

    function carregarVagas() {
        setCarregando(true);
        setErro('');
        
        axios.get<Vaga[]>(`${API_BASE_URL}/Vaga`)
            .then(response => {
                setVagas(response.data); // Os dados vão vir no formato certo
            })
            .catch(error => {
                console.error("Erro ao carregar as vagas:", error);
                setErro("Erro ao carregar as vagas. Verifique sua conexão ou tente novamente.");
            })
            .finally(() => {
                setCarregando(false);
            });
    }

    useEffect(() => {
        carregarVagas();
    }, []);

    return (
        <div className="vagas-container">
            <h1 className="page-title">Lista de Vagas</h1>

            {carregando && <p className="loading-message">Carregando vagas...</p>}
            {erro && <p className="error-message">{erro}</p>}

            {!carregando && vagas.length === 0 && !erro && (
                <p className="no-records-message">Nenhuma vaga encontrada.</p>
            )}

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
                                <th>Andar</th>
                                <th>Setor</th>
                            </tr>
                        </thead>
                        <tbody>
                            {vagas.map(vaga => (
                                <tr key={vaga.id}> {/* Chave única para cada linha */}
                                    <td>{vaga.id}</td>
                                    <td>{vaga.numero}</td>
                                    <td>{vaga.tipo?.nome || "N/A"}</td> {/* Acessa o nome do tipo de veículo */}
                                    <td>
                                        <span className={`status-badge ${vaga.ocupada ? 'status-occupied' : 'status-available'}`}>
                                            {vaga.ocupada ? "Ocupada" : "Livre"}
                                        </span>
                                    </td>
                                    <td>{vaga.ocupada ? (vaga.veiculoId || "-") : "-"}</td> {/* Mostra ID do veículo se ocupada */}
                                    <td>{vaga.andar || "-"}</td>
                                    <td>{vaga.setor || "-"}</td>
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


